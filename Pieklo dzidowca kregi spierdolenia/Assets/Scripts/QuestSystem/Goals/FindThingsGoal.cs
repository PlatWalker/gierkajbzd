using System.Collections.Generic;
using jbzd.Common.RunnerThing;
using jbzd.QuestSystem.QuestStructureElements;
using JetBrains.Annotations;
using UnityEngine;

namespace jbzd.QuestSystem.Goals
{
    [CreateAssetMenu(menuName = "Quest/Goals/Find Things Goal", fileName = "New FindThingsGoal")]
    public class FindThingsGoal : GoalSO
    {
        [RunMethod]
        [UsedImplicitly]
        public void ExecuteGoalScenario()
        {
            // nothing here is for purpose: if this goal was executed that means player found thing,
            // there is no need for further actions.
        }
        
        public override bool GoalEndCondition(Quest questWithThisGoal, List<Actor> actors) => true;
    }
}