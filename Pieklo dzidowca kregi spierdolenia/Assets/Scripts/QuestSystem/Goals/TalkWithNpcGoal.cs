using System.Collections.Generic;
using System.Linq;
using jbzd.Common.RunnerThing;
using jbzd.NPC;
using jbzd.QuestSystem.QuestStructureElements;
using jbzd.QuestSystem.TestFolder;
using UnityEngine;

namespace jbzd.QuestSystem.Goals
{
    [CreateAssetMenu(menuName = "Quest/Goals/Talk with Npc Goal", fileName = "New TalkWithNpcGoal")]
    public class TalkWithNpcGoal : GoalSO
    {
        [RunMethod]
        public void ExecuteGoalScenario()
        {
            // nothing here is for purpose: if this goal was executed that means player chose right dialogue options,
            // there is no need for further actions.
        }

        public override bool GoalEndCondition(Quest questWithThisGoal, List<Actor> actors) => true;
    }
}