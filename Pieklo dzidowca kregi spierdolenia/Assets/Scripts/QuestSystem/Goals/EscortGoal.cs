using System.Collections.Generic;
using System.Linq;
using jbzd.Common.RunnerThing;
using jbzd.NPC;
using jbzd.QuestSystem.QuestStructureElements;
using jbzd.QuestSystem.TestFolder;
using UnityEngine;

namespace jbzd.QuestSystem.Goals
{
    [CreateAssetMenu(menuName = "Quest/Goals/Escort Goal", fileName = " New EscortGoal")]
    public class EscortGoal : GoalSO
    {
        [RunMethod]
        public void ExecuteGoalScenario(List<Actor> actors)
        {
            NpcController npcController = null;
            
            var npcActor = actors.FirstOrDefault(actor => actor.gameObject.TryGetComponent(out npcController));

            if (npcActor is null) Debug.LogError("Something went wrong");
            
            npcController.FollowsPlayer = true;
        }

        public override bool GoalEndCondition(Quest questWithThisGoal, List<Actor> actors)
        {
            NpcController npcController = null;
            
            var npcActor = actors.FirstOrDefault(actor => actor.gameObject.TryGetComponent(out npcController));

            if (npcActor is null) Debug.LogError("Something went wrong");

            if (!npcController.FollowsPlayer) return false;

            npcController.FollowsPlayer = false;
            return true;
        }
    }
}