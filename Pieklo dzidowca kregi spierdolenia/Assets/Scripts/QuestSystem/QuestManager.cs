using System.Collections.Generic;
using System.Linq;
using jbzd.QuestSystem.QuestStructureElements;
using UnityEngine;
using jbzd.Common;
using jbzd.QuestSystem.Goals;
using jbzd.SavingSystem;
using jbzd.SavingSystem.SaveData;
using TMPro;

namespace jbzd.QuestSystem
{
    public class QuestManager : MonoBehaviour, ISaveable
    {
        public delegate void QuestActivated(Quest activatedQuest);

        public event QuestActivated OnQuestActivation;

        enum QuestInfo
        {
            NewQuest,
            EndQuest,
            UpdateQuest
        }
        
        [SerializeField] private TMP_Text _questUpdateUI;
        [SerializeField] private float _questUpdateInfoTime = 3f;

        [field: SerializeField]
        [field: JbzdReadOnly]
        public List<Quest> ActiveQuests { get; set; } = new();
        /// <summary>
        /// Keeps all quests from maps that were loaded at least once. List is filled after Awake has ended.
        /// </summary>
        public List<Quest> AllQuestsFromLoadedMaps { get; } = new();
        
        public void StartQuest(Quest questToStart)
        {
            Debug.Log($"Quest {questToStart.name} started");
            ActiveQuests.Add(questToStart);
            
            if (questToStart.QuestData.Tasks.Count == 0)
            {
                Debug.Log($"Quest {questToStart.name} does not have tasks");
                return;
            }
            ActivateQuestUpdatedUI(QuestInfo.NewQuest);
            questToStart.newUpdate = true;
            
            questToStart.ActiveTask = questToStart.QuestData.Tasks.First(task => task.Order == 0);
            
            OnQuestActivation?.Invoke(questToStart);
        }

        public void EndQuest(Quest questToEnd)
        {
            Debug.Log($"Quest {questToEnd.name} ended");
            ActiveQuests.Remove(questToEnd);
            questToEnd.IsCompleted = true;
            questToEnd.ActiveTask = null;
            questToEnd.newUpdate = true;
            ActivateQuestUpdatedUI(QuestInfo.EndQuest);
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
                if (quest.ActiveTask != inTask)
                {
                    Debug.Log($"Goal that is trying to act ({goalToAct.name})," +
                              $" is in task that is not currently active");
                    return;
                }
                quest.MakeActorPlayInThisQuest(goalToAct);
                if (numberOfActiveQuests == ActiveQuests.Count)
                {
                    quest.newUpdate = true;
                    ActivateQuestUpdatedUI(QuestInfo.UpdateQuest);
                }
                
                // if acting goal finished, and it was last goal in quest thus completing it - quit loop
                if (numberOfActiveQuests != ActiveQuests.Count) return;
            }
        }

        public Quest GetQuestWithThisTaskSo(TaskSO taskToLookFor)
        {
            return AllQuestsFromLoadedMaps.FirstOrDefault(quest => quest.QuestData.Tasks.Contains(taskToLookFor));
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
        private void DeactivateQuestUpdatedUI()
        {
            _questUpdateUI.gameObject.SetActive(false);
        }

        private void ActivateQuestUpdatedUI(QuestInfo questInfo)
        {
            switch (questInfo)
            {
                case QuestInfo.NewQuest:
                    _questUpdateUI.text = "Zadanie rozpoczęte";
                    break;
                case QuestInfo.EndQuest:
                    _questUpdateUI.text = "Zadanie zakończone";
                    break;
                case QuestInfo.UpdateQuest:
                    _questUpdateUI.text = "Zadanie zaktualizowane";
                    break;
            }

            _questUpdateUI.gameObject.SetActive(true);
            Invoke(nameof(DeactivateQuestUpdatedUI), _questUpdateInfoTime);
        }
    }
}