using System.Collections.Generic;
using UnityEngine;

namespace jbzd.QuestSystem.QuestStructureElements
{
    public abstract class GoalSO : ScriptableObject
    {
        [field:SerializeField] public List<ActorSO> ActorsData { get; set; } = new();
        public abstract bool GoalEndCondition(Quest questWithThisGoal);
    }
}