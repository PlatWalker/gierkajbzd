using System;
using UnityEngine;
using UnityEngine.Playables;
using Zenject;

namespace jbzd.Cutscenes
{
    [RequireComponent(typeof(PlayableDirector))]
    public class CutsceneDirector : MonoBehaviour
    {
        private CutscenesManager _cutsceneManager;
        private PlayableDirector _playableDirector;
        
        [Inject]
        public void Constructor(CutscenesManager cutscenesManager)
        {
            _cutsceneManager = cutscenesManager;
        }

        public void Awake()
        {
            _playableDirector = GetComponent<PlayableDirector>();
            _cutsceneManager.OnCutscenePlayDemand += timelineAsset =>
            {
                if (timelineAsset == _playableDirector.playableAsset)
                {
                    Debug.Log($"Cutscene{name} starts playing");
                    _playableDirector.Play();
                }
            };
            
        }
        
        
    }
}