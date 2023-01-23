using System.Collections.Generic;
using System.Linq;
using jbzd.QuestSystem.QuestStructureElements;
using jbzd.QuestSystem.TestFolder;
using UnityEngine;

namespace jbzd.QuestSystem.Goals
{
    [CreateAssetMenu(menuName = "Quest/Goals/Escort Goal", fileName = " New EscortGoal")]
    public class EscortGoal : GoalSO
    {
        public override void ExecuteGoalScenario(List<Actor> actors)
        {
            NpcControllerTest npcController = null;
            
            var npcActor = actors.FirstOrDefault(actor => actor.gameObject.TryGetComponent(out npcController));

            if (npcActor is null) Debug.LogError("Something went wrong");
            
            npcController.StopFollow();
        }

        public override bool GoalEndCondition() => true;
    }
}