using System;
using System.Collections.Generic;
using System.Linq;
using jbzd.Common;
using jbzd.Common.AssetsHelper;
using UnityEngine;

namespace jbzd.QuestSystem.QuestStructureElements
{
    [CreateAssetMenu(menuName = "Quest/Task", fileName = "New Task Data")]
    public class TaskSO : DuplicatedScriptableObjects
    {
        
        [field:SerializeField]
        public List<GoalSO> Goals { get; set; }
        [field:SerializeField]
        public int Order { get; set; }

        [TextArea]
        public string Description;
        
        /// <summary>
        /// Checks if there is a goal of type T in this task
        /// </summary>
        /// <param name="goals">found goals</param>
        /// <typeparam name="T">type of goals to look for</typeparam>
        /// <returns>bool that indicates if we found a goal in task or not</returns>
        public bool IsThereAGoalOfType<T>(out List<T> goals) where T : GoalSO
        {
            goals = Goals.Where(goal => goal.GetType() == typeof(T)).Cast<T>().ToList();

            return goals.Count is not 0;
        }
    }
}