using System.Collections.Generic;
using System.Linq;
using jbzd.Common.RunnerThing;
using jbzd.NPC;
using jbzd.QuestSystem.QuestStructureElements;
using JetBrains.Annotations;
using UnityEngine;

namespace jbzd.QuestSystem.Goals
{
    [CreateAssetMenu(menuName = "Quest/Goals/Escort Goal", fileName = " New EscortGoal")]
    public class EscortGoal : GoalSO
    {
        [field:SerializeField]
        [Tooltip("Npc state that npc should be after stopped following player")]
        public NpcController.NpcStates ExitNpcState { get; set; } = NpcController.NpcStates.Idle;
        
        [RunMethod]
        [UsedImplicitly]
        public void ExecuteGoalScenario(List<Actor> actors)
        {
            var npcControllers=GetComponentFromActorsList<NpcController>(actors);

            foreach (var npcController in npcControllers.Where(npcController => npcController != null))
            {
                if (npcController.NpcState is NpcController.NpcStates.FollowPlayer)
                {
                    npcController.NpcState = ExitNpcState;
                    continue;
                }

                npcController.NpcState = NpcController.NpcStates.FollowPlayer;
            }
        }

        public override bool GoalEndCondition(Quest questWithThisGoal, List<Actor> actors)
        {
            var npcController = GetComponentFromActorsList<NpcController>(actors).First();

            if (npcController.NpcState is NpcController.NpcStates.FollowPlayer) return false;
            
            npcController.NpcAgent.isStopped = true;
            return true;
        }
    }
}