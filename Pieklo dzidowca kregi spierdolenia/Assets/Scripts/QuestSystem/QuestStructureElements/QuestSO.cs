using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace jbzd.QuestSystem.QuestStructureElements
{
    [CreateAssetMenu(menuName = "Quest/Quest", fileName = "New Quest Data")]
    public class QuestSO : ScriptableObject
    {
        
        [SerializeField] public List<TaskSO> Tasks = new List<TaskSO>();

        public void OnEnable()
        {
            if(Tasks.Count != 0) TaskOrderingValidation();
        }

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

        private void TaskOrderingValidation()
        {
            var tasksOrders = Tasks.Select(task => task.Order).ToList();
                
            Debug.Assert(tasksOrders.Distinct().Count() == tasksOrders.Count(),
                $"Each order numbering in tasks need to be unique! QuestSO: {name}");
            Debug.Assert(tasksOrders.Zip(tasksOrders.Skip(1), (a,b) => (a+1) == b).All(x => x),
                $"Order of tasks need to be sequential (increment by one) QuestSO: {name}");
            Debug.Assert(tasksOrders.Contains(0),
                $"In tasks ordering there need to be order equal to 0. QuestSO: {name}");
        }
    }
}