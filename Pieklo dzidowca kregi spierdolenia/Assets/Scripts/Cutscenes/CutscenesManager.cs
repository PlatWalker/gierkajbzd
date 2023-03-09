using JetBrains.Annotations;
using UnityEngine.Timeline;

namespace jbzd.Cutscenes
{
    [UsedImplicitly] // in installer
    public class CutscenesManager
    {
        public delegate void CutsceneNeededToBePlayed(TimelineAsset timelineAsset);
        
        public event CutsceneNeededToBePlayed OnCutscenePlayDemand;

        public void PlayCutscene(TimelineAsset timelineAsset)
        {
            OnCutscenePlayDemand?.Invoke(timelineAsset);
        }
    }
}