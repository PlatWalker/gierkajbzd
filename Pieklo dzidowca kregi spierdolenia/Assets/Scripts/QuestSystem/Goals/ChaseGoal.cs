using System.Collections.Generic;
using System.Linq;
using jbzd.Common.RunnerThing;
using jbzd.NPC;
using jbzd.QuestSystem.QuestStructureElements;
using jbzd.QuestSystem.TestFolder;
using JetBrains.Annotations;
using UnityEngine;

namespace jbzd.QuestSystem.Goals
{
    [CreateAssetMenu(menuName = "Quest/Goals/Chase Goal", fileName = "New ChaseGoal")]
    public class ChaseGoal : GoalSO
    {
        [field:SerializeField]
        [Tooltip("Npc state that npc should be after catching it")]
        public NpcController.NpcStates ExitNpcState { get; set; } = NpcController.NpcStates.Idle;
        
        [RunMethod]
        [UsedImplicitly]
        public void ExecuteGoalScenario(List<Actor> actors)
        {
            var npcController = GetComponentFromActorsList<NpcController>(actors).First();

            if (npcController.NpcState is NpcController.NpcStates.RunAwayFromPlayer)
            {
                npcController.NpcState = ExitNpcState;
                return;
            }
            
            npcController.NpcState = NpcController.NpcStates.RunAwayFromPlayer;
        }

        public override bool GoalEndCondition(Quest questWithThisGoal, List<Actor> actors)
        {
            var npcController = GetComponentFromActorsList<NpcController>(actors).First();

            if (npcController.NpcState is NpcController.NpcStates.RunAwayFromPlayer) return false;
            
            npcController.NpcAgent.isStopped = true;
            return true;
        }
    }
}
