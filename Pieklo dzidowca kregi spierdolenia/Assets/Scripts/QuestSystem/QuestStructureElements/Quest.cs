using System;
using System.Collections.Generic;
using System.Linq;
using jbzd.Common;
using jbzd.Common.RunnerThing;
using jbzd.QuestSystem.Goals;
using UnityEngine;
using Zenject;

namespace jbzd.QuestSystem.QuestStructureElements
{
    public class Quest : MonoBehaviour
    {
        #region Variables

        [field: Header("Quest Settings")]
        [field:SerializeField] public QuestSO QuestData { get; set; }
        [field:SerializeField] public List<Actor> Actors { get; set; } = new();

        private QuestManager _questManager;
        private RunnerFactory _runnerFactory;
        
        #region Quest Tracker Variables

        [field: Header("Quest Tracker Variables")]
        [field: SerializeField]
        [field: JbzdReadOnly]
        public bool IsCompleted { get; set; }

        [field: SerializeField]
        [field: JbzdReadOnly]
        public TaskSO ActiveTask { get; set; }

        [field: SerializeField]
        [field: JbzdReadOnly]
        public List<TaskSO> FinishedTasks { get; set; } = new();

        [field: SerializeField]
        [field: JbzdReadOnly]
        public List<GoalSO> FinishedGoals { get; set; } = new();

        [field: Header("Additional track variables")]
        [field: SerializeField]
        [field: JbzdReadOnly]
        public List<int> GoalItemCollected { get; set; } = new();
        [field: SerializeField]
        [field: JbzdReadOnly]
        public List<int> GoalCompletionItemCount { get; set; } = new();
        [field: SerializeField]
        [field: JbzdReadOnly]
        public List<string> GoalName { get; set; } = new();
        
        #endregion
        
        #endregion

        [Inject]
        public void Constructor(QuestManager questManager, RunnerFactory runnerFactory)
        {
            _questManager = questManager;
            _runnerFactory = runnerFactory;
        }

        private void Awake()
        {
            _questManager.AllQuestsOnActiveMap.Add(this);

            foreach (var goal in QuestData.Tasks.SelectMany(task => task.Goals))
            {
                if (goal is not PickUpItemGoal pickUpItemGoal) continue;
                
                GoalName.Add(goal.name);
                GoalItemCollected.Add(0);
                GoalCompletionItemCount.Add(pickUpItemGoal.GoalCompletionItemCount);
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
                this
            });
            
            CheckFinishCondition(passedGoal, actorsInPassedGoal);
        }

        private void CheckFinishCondition(GoalSO goalToAct, List<Actor> actorsInPassedGoal)
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