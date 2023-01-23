using System.Collections.Generic;
using UnityEngine;

namespace jbzd.QuestSystem.QuestStructureElements
{
    public abstract class GoalSO : ScriptableObject
    {
        [field:SerializeField] public List<ActorSO> ActorsData { get; set; } = new();
        public abstract void ExecuteGoalScenario(List<Actor> actors);
        public abstract bool GoalEndCondition();
    }
}