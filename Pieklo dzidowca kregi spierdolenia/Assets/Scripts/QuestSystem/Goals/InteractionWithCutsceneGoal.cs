using System.Collections.Generic;
using jbzd.Common.RunnerThing;
using jbzd.MinorSystems.Cutscenes;
using jbzd.QuestSystem.QuestStructureElements;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Timeline;

namespace jbzd.QuestSystem.Goals
{
    [CreateAssetMenu(menuName = "Quest/Goals/Interaction With Cutscene Goal", fileName = "New InteractionWithCutsceneGoal")]
    public class InteractionWithCutsceneGoal : GoalSO
    {
        [field:SerializeField]
        public TimelineAsset CutsceneToPlayAfterInteraction { get; set; }
        
        [RunMethod]
        [UsedImplicitly]
        public void ExecuteGoalScenario(CutscenesManager cutscenesManager)
        {
            cutscenesManager.PlayCutscene(CutsceneToPlayAfterInteraction);
        }

        public override bool GoalEndCondition(Quest questWithThisGoal, List<Actor> actors) => true;
    }
}