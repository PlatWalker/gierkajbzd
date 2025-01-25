using jbzd.MinorSystems.Cutscenes;
using UnityEngine;
using UnityEngine.Timeline;
using Zenject;

namespace jbzd.Common
{
    public class DeactivateDuringCutscene: MonoBehaviour
    {
        private CutscenesManager _cutsceneManager;
        
        [Inject]
        public void Construct(CutscenesManager cutscenesManager)
        {  
            _cutsceneManager = cutscenesManager;
            _cutsceneManager.OnCutsceneStarted += Disappear;
            _cutsceneManager.OnCutsceneEnded += Reappear;
        }
        
        private void Disappear(TimelineAsset timelineAsset){
            gameObject.SetActive(false);
        }

        private void Reappear(TimelineAsset timelineAsset){
            gameObject.SetActive(true);
        }
    }
}