using jbzd.Common.RunnerThing;
using jbzd.MinorSystems.Cutscenes;
using UnityEngine;
using UnityEngine.Timeline;

namespace jbzd.Dialogues.RuntimeData
{
    public class CutsceneRuntimeData : NodeRuntimeData
    {
        [field:SerializeField] public TimelineAsset TimelineAsset { get; set; }
        
        [RunMethod]
        public void Run(DialogueManager manager, CutscenesManager cutscenesManager)
        {
            cutscenesManager.PlayCutscene(TimelineAsset);
            
            manager.NextNode();
        }
        
    }
}
