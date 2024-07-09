using System;
using System.Collections.Generic;
using System.Linq;
using jbzd.Common;
using jbzd.Common.RunnerThing;
using jbzd.Cutscenes;
using jbzd.Dialogues;
using jbzd.SavingSystem;
using jbzd.SavingSystem.SaveData;
using jbzd.UI;
using jbzd.UI.Dialogues;
using UnityEngine;
using Zenject;

namespace jbzd.QuestSystem.QuestStructureElements
{
    public class Quest : MonoBehaviour, ISaveable
    {
        #region Variables

        public delegate void QuestEventOccured(Quest quest);

        public delegate void TaskEventOccured(Quest questInvoked, TaskSO taskUpdated);
        [Header("Quest Settings")]
        //There is no {get;set;} here because it messes up serialization in QuestEditor.cs
        [SerializeField] public QuestSO QuestData;
        [SerializeField]
        [JbzdReadOnly]
        [Tooltip("ReadOnly field, dont try too add anything here")]
        public List<Actor> Actors = new();
        public event TaskEventOccured OnTaskStarted;
        public event TaskEventOccured OnTaskEnded;
        public event QuestEventOccured onQuestEnded;
        private QuestManager _questManager;
        private RunnerFactory _runnerFactory;
        private CutscenesManager _cutscenesManager;
        private DialogueUIController _dialogueController;

        #region Quest Tracker Variables

        [Header("Quest Tracker Variables")]
        [SerializeField]
        [JbzdReadOnly]
        private bool isCompleted;
        public bool IsCompleted {
            get => isCompleted;
            set { 
                isCompleted = value;
                onQuestEnded?.Invoke(this);  
            }
        }

        [SerializeField]
        [JbzdReadOnly]
        private TaskSO activeTask;

        public TaskSO ActiveTask
        {
            get => activeTask;
            set
            {
                activeTask = value;
                OnTaskStarted?.Invoke(this, value);
            }
        }
        
        
        [SerializeField]
        [JbzdReadOnly]
        public List<TaskSO> FinishedTasks = new();

        [SerializeField]
        [JbzdReadOnly]
        public List<GoalSO> FinishedGoals = new();

        [Header("Additional track variables")]
        [SerializeField]
        [JbzdReadOnly]
        public List<int> GoalItemCollected = new();
        [SerializeField]
        [JbzdReadOnly]
        public List<int> GoalCompletionItemCount = new();
        [SerializeField]
        [JbzdReadOnly]
        public List<string> GoalName = new();
        
        #endregion
        
        #endregion

        [Inject]
        public void Constructor(
            QuestManager questManager,
            RunnerFactory runnerFactory,
            CutscenesManager cutscenesManager,
            UserInterfaceManager uiManager)
        {
            _questManager = questManager;
            _runnerFactory = runnerFactory;
            _cutscenesManager = cutscenesManager;
            _dialogueController = uiManager.GetUIController<DialogueUIController>();
        }

        private void Awake()
        {
            _questManager.AllQuestsFromLoadedMaps.Add(this);

            if (QuestData is null)
            {
                Debug.LogError($"Quest {name} doesn't have his QuestData");
                return;
            }
            
            foreach (var goal in QuestData.Tasks.SelectMany(task => task.Goals))
            {
                if (goal is not GoalInvolvingCollectingSO goalInvolvingCollecting) continue;
                
                GoalName.Add(goal.name);
                GoalItemCollected.Add(0);
                GoalCompletionItemCount.Add(goalInvolvingCollecting.GoalCompletionItemCount);
            }
        }

        private void OnDisable()
        {
            _questManager.AllQuestsFromLoadedMaps.Remove(this);
        }

        public void MakeActorPlayInThisQuest(GoalSO passedGoal)
        {
            if (FinishedGoals.Contains(passedGoal))
            {
                Debug.Log($"This quest {name} has goal {passedGoal.name} but this goal is already finished");
                return;
            }
            
            var actorsInPassedGoal = Actors.FindAll(actor => passedGoal.ActorsData.Contains(actor.ActorData));

            if (actorsInPassedGoal.Count != passedGoal.ActorsData.Count)
            {
                Debug.LogError($"There are missing actors in {name}");
                return;
            }
            
            foreach (var actor in actorsInPassedGoal)
            {
                try
                {
                    //autistic way of checking if gameobject of actor component was destroyed
                    var kek = actor.gameObject;
                }
                catch (MissingReferenceException)
                {
                    continue;
                }
                
                Debug.Log($"Actor {actor.name} played scenario in {passedGoal.name}!");
            }

            var runner = _runnerFactory.Create(passedGoal);
            
            runner.Run(new List<object>
            {
                actorsInPassedGoal,
                this,
                _cutscenesManager
            });
            
            CheckFinishCondition(passedGoal, actorsInPassedGoal);

            if (passedGoal.DialogueToStartOnGoalComplete is not null &&
                passedGoal.GoalEndCondition(this, actorsInPassedGoal))
            {
                _dialogueController.StartDialogue(passedGoal.DialogueToStartOnGoalComplete);
            }
        }

        public void CheckFinishCondition(GoalSO goalToAct, List<Actor> actorsInPassedGoal)
        {
            if (goalToAct.GoalEndCondition(this, actorsInPassedGoal))
            {
                FinishedGoals.Add(goalToAct);
                Debug.Log($"Finished goal {goalToAct.name}");

                if (ActiveTask.Goals.Intersect(FinishedGoals).Count() == ActiveTask.Goals.Count)
                {
                    FinishedTasks.Add(ActiveTask);
                    OnTaskEnded?.Invoke(this, ActiveTask);
                    Debug.Log($"Finished task {ActiveTask.name}");
                    
                    if (QuestData.Tasks.Count == FinishedTasks.Count)
                    {
                        _questManager.EndQuest(this);
                        return;
                    }

                    ActiveTask = QuestData.Tasks.FirstOrDefault(task => task.Order == ActiveTask.Order + 1);
                    OnTaskStarted?.Invoke(this, ActiveTask);
                }
            }
        }

        public void LoadData(GameData gameData)
        {
            var saveData = gameData.questSaveDatas.FirstOrDefault(data => data.questId == QuestData.Id);

            if (saveData is null)
            {
                Debug.LogError($"Something went wrong during loading object: {name}");
                return;
            }

            IsCompleted = saveData.isCompleted;
            ActiveTask = QuestData.Tasks.FirstOrDefault(task => task.Id == saveData.idOfActiveTask);

            foreach (var idOfActor in saveData.idOfActors.Distinct())
            {
                var actors = FindObjectsOfType<Actor>().ToList();

                var actorsWithGivenId = actors.FindAll(actor => actor.ActorData.Id == idOfActor);

                Actors.AddRange(actorsWithGivenId);
            }
            
            foreach (var idOfTask in saveData.idOfFinishedTasks)
            {
                var savedTask = QuestData.Tasks.FirstOrDefault(task => task.Id == idOfTask);

                if (savedTask is null)
                {
                    Debug.LogError($"Something went wrong during loading object: {name}");
                    return;
                }
                
                FinishedTasks.Add(savedTask);
            }

            foreach (var idOfGoal in saveData.idOfFinishedGoals)
            {
                GoalSO savedGoal = null;
                
                foreach (var task in QuestData.Tasks)
                {
                    savedGoal = task.Goals.FirstOrDefault(goal => goal.Id == idOfGoal);

                    if (savedGoal is not null)
                    {
                        break;
                    }
                }
                
                if (savedGoal is null)
                {
                    Debug.LogError($"Something went wrong during loading object: {name}");
                    return;
                }
                    
                FinishedGoals.Add(savedGoal);
            }

            GoalItemCollected = saveData.goalItemCollected;
            GoalCompletionItemCount = saveData.goalCompletionItemCount;
            GoalName = saveData.goalName;
        }

        public void SaveData(ref GameData gameData)
        {
            gameData.questSaveDatas.Add(new QuestSaveData
            {
                questId = QuestData.Id,
                isCompleted = IsCompleted,
                idOfFinishedTasks = FinishedTasks.Select(task => task.Id).ToList(),
                idOfFinishedGoals = FinishedGoals.Select(goal => goal.Id).ToList(),
                goalItemCollected = GoalItemCollected,
                goalCompletionItemCount = GoalCompletionItemCount,
                goalName = GoalName,
                idOfActiveTask = ActiveTask != null ? ActiveTask.Id : null,
                idOfActors = Actors.Select(actor => actor.ActorData.Id).ToList()
            });
        }
    }
}