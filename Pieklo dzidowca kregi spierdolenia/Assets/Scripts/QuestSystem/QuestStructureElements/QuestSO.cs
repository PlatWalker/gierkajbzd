using System;
using System.Collections.Generic;
using System.Linq;
using jbzd.Common;
using UnityEngine;


namespace jbzd.QuestSystem.QuestStructureElements
{
    [CreateAssetMenu(menuName = "Quest/Quest", fileName = "New Quest Data")]
    public class QuestSO : DuplicatedScriptableObjects
    {     
        
        [SerializeField] public List<TaskSO> Tasks = new();

        public void Awake()
        {
            if(Tasks.Count != 0) TaskOrderingValidation();
        }
        
        /// <summary>
        /// Checks is there a goal in any task in this quest. 
        /// </summary>
        /// <param name="goalToFind">Goal which existence in quest we looking for</param>
        /// <param name="inTask">Task in which we found a goal. Null if not found.</param>
        /// <returns>bool that indicates if we found a goal in quest or not</returns>
        public bool IsThereAGoal(GoalSO goalToFind, out TaskSO inTask)
        {
            var tasksWithGoalToFind = Tasks.Where(task => task.Goals.Any(goal => goal == goalToFind)).ToList();

            switch (tasksWithGoalToFind.Count)
            {
                case 0:
                    inTask = null;
                    return false;
                case 1:
                    inTask = tasksWithGoalToFind.First();
                    return true;
                case >=2:
                    inTask = tasksWithGoalToFind.First();
                    Debug.LogWarning("One goalSO is in two or more taskSO. There will be only first one used!");
                    return true;
                default:
                    inTask = null;
                    Debug.LogError("Something went very wrong");
                    return false;
            }
        }

        /// <summary>
        /// Checks if there is a goal of type T in this quest
        /// </summary>
        /// <param name="goals">found goals</param>
        /// <typeparam name="T">type of goals to look for</typeparam>
        /// <returns>bool that indicates if we found a goal in quest or not</returns>
        public bool IsThereAGoalOfType<T>(out List<T> goals) where T : GoalSO
        {
            goals = new List<T>();
            
            foreach (var task in Tasks)
            {
                goals.AddRange((List<T>)task.Goals.Where(goal => goal.GetType() == typeof(T)));
            }

            return goals.Count is not 0;
        }
        
        /// <summary>
        /// Checks if there is an passed actor in any of goals.
        /// </summary>
        /// <param name="actorData">Actor to look for</param>
        /// <returns></returns>
        public bool IsThereAnActor(ActorSO actorData)
        {
            return Tasks.Any(task => task.Goals.Any(goal => goal.ActorsData.Contains(actorData)));
        }
        
        private void TaskOrderingValidation()
        {
            var tasksOrders = Tasks.Select(task => task.Order).ToList();
                
            Debug.Assert(tasksOrders.Distinct().Count() == tasksOrders.Count(),
                $"Each order numbering in tasks need to be unique. QuestSO: {name}");
            Debug.Assert(tasksOrders.Zip(tasksOrders.Skip(1), (a,b) => (a+1) == b).All(x => x),
                $"Order of tasks need to be sequential (increment by one). QuestSO: {name}");
            Debug.Assert(tasksOrders.Contains(0),
                $"In tasks ordering there need to be order equal to 0. QuestSO: {name}");
        }
        
        
    }
}