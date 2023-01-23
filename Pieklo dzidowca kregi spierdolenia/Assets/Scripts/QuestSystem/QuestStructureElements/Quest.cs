using System.Collections.Generic;
using System.Linq;
using jbzd.Common;
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

        #endregion
        
        #endregion

        [Inject]
        public void Constructor(QuestManager questManager)
        {
            _questManager = questManager;
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
                Debug.Log($"Actor {actor.name} played scenario in {passedGoal.name}!");
            }
            passedGoal.ExecuteGoalScenario(actorsInPassedGoal);
            
            CheckFinishCondition(passedGoal);
        }

        private void CheckFinishCondition(GoalSO goalToAct)
        {
            if (goalToAct.GoalEndCondition())
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