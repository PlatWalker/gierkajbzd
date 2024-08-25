using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Timeline;

namespace jbzd.MinorSystems.Cutscenes
{
    [UsedImplicitly] // in installer
    public class CutscenesManager
    {
        public delegate void CutsceneNeededToBePlayed(TimelineAsset timelineAsset);
        public delegate void CutsceneEventOccured(TimelineAsset timelineAsset);

        public event CutsceneNeededToBePlayed OnCutscenePlayDemand;
        public event CutsceneEventOccured OnCutsceneStarted;
        public event CutsceneEventOccured OnCutsceneEnded;

        public void PlayCutscene(TimelineAsset timelineAsset)
        {
            Debug.Log($"Cutscene {timelineAsset.name} CUTSCENE MANAGER");
            
            OnCutscenePlayDemand?.Invoke(timelineAsset);
            StartCutscene(timelineAsset);

        }
        private void StartCutscene(TimelineAsset timelineAsset)
        {
            Debug.Log($"Cutscene started CUTSCENE MANAGER");
            OnCutsceneStarted?.Invoke(timelineAsset);
        }
        /// <summary>
        /// Only CutsceneDirector script can use this, I know, it's stupid xD
        /// </summary>
        /// <param name="timelineAsset"></param>
        public void EndCutscene(TimelineAsset timelineAsset)
        {
            Debug.Log($"Cutscene ended CUTSCENE MANAGER");
            OnCutsceneEnded?.Invoke(timelineAsset);
        }
    }
}