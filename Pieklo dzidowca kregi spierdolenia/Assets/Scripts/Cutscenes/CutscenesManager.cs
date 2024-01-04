using JetBrains.Annotations;
using UnityEngine.Timeline;
using UnityEngine;


namespace jbzd.Cutscenes
{
    [UsedImplicitly] // in installer
    public class CutscenesManager
    {
        public delegate void CutsceneNeededToBePlayed(TimelineAsset timelineAsset);
        public delegate void CutsceneEventOccured();

        public event CutsceneNeededToBePlayed OnCutscenePlayDemand;
        public event CutsceneEventOccured OnCutsceneStarted;
        public event CutsceneEventOccured OnCutsceneEnded;

        public void PlayCutscene(TimelineAsset timelineAsset)
        {
            Debug.Log($"Cutscene {timelineAsset.name} CUTSCENE MANAGER");
            
            StartCutscene();
            OnCutscenePlayDemand?.Invoke(timelineAsset);
        }
        public void StartCutscene()
        {
            Debug.Log($"Cutscene started CUTSCENE MANAGER");
            OnCutsceneStarted?.Invoke();
        }
        public void EndCutscene()
        {
            Debug.Log($"Cutscene ended CUTSCENE MANAGER");
            OnCutsceneEnded?.Invoke();
        }
    }
}