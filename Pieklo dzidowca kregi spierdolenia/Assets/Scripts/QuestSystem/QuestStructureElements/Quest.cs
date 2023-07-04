using System;
using System.Collections.Generic;
using System.Linq;
using jbzd.Common;
using jbzd.Common.RunnerThing;
using jbzd.Cutscenes;
using jbzd.Dialogues;
using jbzd.QuestSystem.Goals;
using UnityEngine;
using Zenject;

namespace jbzd.QuestSystem.QuestStructureElements
{
    public class Quest : MonoBehaviour
    {
        #region Variables

        [Header("Quest Settings")]
        //There is no {get;set;} here because it messes up serialization in QuestEditor.cs
        [SerializeField] public QuestSO QuestData;
        [SerializeField] public List<Actor> Actors = new();

        private QuestManager _questManager;
        private RunnerFactory _runnerFactory;
        private CutscenesManager _cutscenesManager;
        private DialogueManager _dialogueManager;
        
        #region Quest Tracker Variables
        
        [Header("Quest Tracker Variables")]
        [SerializeField]
        [JbzdReadOnly]
        public bool IsCompleted;

        [SerializeField]
        [JbzdReadOnly]
        public TaskSO ActiveTask;

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
            DialogueManager dialogueManager)
        {
            _questManager = questManager;
            _runnerFactory = runnerFactory;
            _cutscenesManager = cutscenesManager;
            _dialogueManager = dialogueManager;
        }

        private void Awake()
        {
            _questManager.AllQuestsOnActiveMap.Add(this);

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
            _questManager.AllQuestsOnActiveMap.Remove(this);
        }

        public void MakeActorPlayInThisQuest(GoalSO passedGoal)
        {
            if (FinishedGoals.Contains(passedGoal)) return;
            
            var actorsInPassedGoal = Actors.FindAll(actor => passedGoal.ActorsData.Contains(actor.ActorData));

            if (actorsInPassedGoal.Count != passedGoal.ActorsData.Count)
            {
                Debug.LogError($"There are missing actors in {this.name}");
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
                _dialogueManager.StartDialogue(passedGoal.DialogueToStartOnGoalComplete);
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
                    Debug.Log($"Finished task {ActiveTask.name}");
                    
                    if (QuestData.Tasks.Count == FinishedTasks.Count)
                    {
                        _questManager.EndQuest(this);
                        return;
                    }

                    ActiveTask = QuestData.Tasks.FirstOrDefault(task => task.Order == ActiveTask.Order + 1);
                }
            }
        }
    }
}