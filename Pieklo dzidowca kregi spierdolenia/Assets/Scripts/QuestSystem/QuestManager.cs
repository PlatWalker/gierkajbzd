using System.Collections.Generic;
using System.Linq;
using jbzd.QuestSystem.QuestStructureElements;
using UnityEngine;
using jbzd.Common;
using jbzd.QuestSystem.Goals;
using jbzd.SavingSystem;
using jbzd.SavingSystem.SaveData;

namespace jbzd.QuestSystem
{
    public class QuestManager : MonoBehaviour, ISaveable
    {
        public delegate void QuestActivated(Quest activatedQuest);

        public event QuestActivated OnQuestActivation;

        [SerializeField] private Canvas _questStartedCanvas;
        [SerializeField] private Canvas _questEndedCanvas;
        [SerializeField] private Canvas _newTaskCanvas;

        [field: SerializeField]
        [field: JbzdReadOnly]
        public List<Quest> ActiveQuests { get; set; } = new();
        /// <summary>
        /// Keeps all quests from maps that were loaded at least once
        /// </summary>
        public List<Quest> AllQuestsFromLoadedMaps { get; } = new();
        
        public void StartQuest(Quest questToStart)
        {
            Debug.Log($"Quest {questToStart.name} started");
            ActiveQuests.Add(questToStart);
            OnQuestActivation?.Invoke(questToStart);
            
            if (questToStart.QuestData.Tasks.Count == 0)
            {
                Debug.Log($"Quest {questToStart.name} does not have tasks");
                return;
            }
            _questStartedCanvas.gameObject.SetActive(true);
            Invoke(nameof(DeactivateQuestStartedCanvas), 3f);
            
            questToStart.ActiveTask = questToStart.QuestData.Tasks.First(task => task.Order == 0);
        }

        public void EndQuest(Quest questToEnd)
        {
            Debug.Log($"Quest {questToEnd.name} ended");
            ActiveQuests.Remove(questToEnd);
            questToEnd.IsCompleted = true;
            questToEnd.ActiveTask = null;
            //activate canvas for 3 seconds
            _questEndedCanvas.gameObject.SetActive(true);
            Invoke(nameof(DeactivateQuestEndedCanvas), 3f);
        }
        
        /// <summary>
        /// Makes actor included in <paramref name="goalToAct"/> play a scenario.  
        /// </summary>
        /// <param name="goalToAct">Goal with actors and scenario</param>
        public void MakeActorPlay(GoalSO goalToAct)
        {
            Debug.Log($"Goal {goalToAct.name} is trying to act");
            TaskSO inTask = null;

            var numberOfActiveQuests = ActiveQuests.Count;
            var questsWithGoalToAct = 
                ActiveQuests.Where(quest => quest.QuestData.IsThereAGoal(goalToAct, out inTask)).ToList();
            
            if (questsWithGoalToAct.Count is 0)
            {
                Debug.Log($"Goal: {goalToAct.name} have tried to act but there were no active quests with such goal");
                return;
            }
            
            foreach (var quest in questsWithGoalToAct)
            {
                //TODO need to be tested, why we are returning if we are iterating through quests?
                if (quest.FinishedTasks.Contains(inTask))
                {
                    Debug.Log($"Goal that trying to act ({goalToAct.name})," +
                              $" is in task{inTask.name} that is already finished");
                    return;
                }

                //TODO need to be tested, why we are returning if we are iterating through quests?
                /*if (quest.ActiveTask != inTask)
                {
                    Debug.Log($"Goal that is trying to act ({goalToAct.name})," +
                              $" is in task that is not currently active");
                    return;
                }*/
                quest.MakeActorPlayInThisQuest(goalToAct);
                if (numberOfActiveQuests == ActiveQuests.Count)
                {
                    _newTaskCanvas.gameObject.SetActive(true);
                    Invoke(nameof(DeactivateNewTaskCanvas), 3f);
                }
                
                // if acting goal finished, and it was last goal in quest thus completing it - quit loop
                if (numberOfActiveQuests != ActiveQuests.Count) return;
            }
        }

        public void LoadData(GameData gameData)
        {
            //TODO can be optymalized with use of Gameobject.FindWithTag and assigning all gameobject with a quest a tag.
            var quests = FindObjectsOfType<Quest>();

            var activeQuests = quests
                .Where(quest => gameData.questManagerSaveData.activeQuestsId.Contains(quest.QuestData.Id)).ToList();
            
            ActiveQuests.AddRange(activeQuests);

            MakeActorsPlayInKillEnemyGoalsOfActiveTasks(gameData);
        }

        public void SaveData(ref GameData gameData)
        {
            gameData.questManagerSaveData = new QuestManagerSaveData
            {
                activeQuestsId = ActiveQuests.Select(questData => questData.QuestData.Id).ToList(),
            };
        }
        
        private void MakeActorsPlayInKillEnemyGoalsOfActiveTasks(GameData gameData)
        {
            foreach (var quest in ActiveQuests)
            {
                var questSaveData = gameData.questSaveDatas.FirstOrDefault(data => data.questId == quest.QuestData.Id);

                if (questSaveData is null)
                {
                    Debug.LogError($"Something went wrong during loading object: {name}");
                    return;
                }

                var activeTaskOfQuest = quest.QuestData.Tasks.FirstOrDefault(task => task.Id == questSaveData.idOfActiveTask);

                if (activeTaskOfQuest is null)
                {
                    Debug.LogError($"Something went wrong during loading object: {name}");
                    return;
                }

                if (activeTaskOfQuest.IsThereAGoalOfType<KillEnemiesGoal>(out var goals))
                {
                    foreach (var goal in goals)
                    {
                        MakeActorPlay(goal);
                    }
                }
            }
        }
        private void DeactivateQuestStartedCanvas()
        {
            _questStartedCanvas.gameObject.SetActive(false);
        }
        private void DeactivateNewTaskCanvas()
        {
            _newTaskCanvas.gameObject.SetActive(false);
        }
        private void DeactivateQuestEndedCanvas()
        {
            _questEndedCanvas.gameObject.SetActive(false);
        }
    }
}