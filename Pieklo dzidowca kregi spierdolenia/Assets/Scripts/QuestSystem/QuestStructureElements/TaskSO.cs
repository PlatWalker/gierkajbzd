using System.Collections.Generic;
using UnityEngine;

namespace jbzd.QuestSystem.QuestStructureElements
{
    [CreateAssetMenu(menuName = "Quest/Task", fileName = "New Task Data")]
    public class TaskSO : ScriptableObject
    {
        [field:SerializeField]
        public List<GoalSO> Goals { get; set; }
        [field:SerializeField]
        public int Order { get; set; }
    }
}