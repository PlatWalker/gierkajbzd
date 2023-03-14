using System.Collections.Generic;
using jbzd.Common.RunnerThing;
using jbzd.QuestSystem.QuestStructureElements;
using JetBrains.Annotations;
using UnityEngine;

namespace jbzd.QuestSystem.Goals
{
    [CreateAssetMenu(menuName = "Quest/Goals/Pick Up Item Goal", fileName = "New PickUpItemGoal")]
    public class PickUpItemGoal : GoalSO
    {
        [field:SerializeField]
        public int GoalCompletionItemCount { get; private set; }

        [RunMethod]
        [UsedImplicitly]
        public void ExecuteGoalScenario(Quest questWithThisGoal)
        {
            var index = questWithThisGoal.GoalName.IndexOf(name);
            questWithThisGoal.GoalItemCollected[index] += 1;
        }

        public override bool GoalEndCondition(Quest questWithThisGoal, List<Actor> actors)
        {
            var index = questWithThisGoal.GoalName.IndexOf(name);
            
            Debug.Assert(questWithThisGoal.GoalItemCollected[index] <= GoalCompletionItemCount, 
                "GoalItemCollected property out of 'index' of max items collected");
            
            return questWithThisGoal.GoalItemCollected[index] == GoalCompletionItemCount;
        }
    }
}