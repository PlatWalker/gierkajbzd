using jbzd.Common;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using jbzd.Dialogues;
using jbzd.MinorSystems.Cutscenes;
using Zenject;
using MyBox;
using UnityEngine.Serialization;

namespace jbzd.QuestCreationScripts
{ 
    [RequireComponent(typeof(Collider))]
    [RequireLayer("Player Triggers")]
    public class CutsceneTrigger : MonoBehaviour
    {
        public PlayableDirector playableDirector;
        private CutscenesManager _cutscenesManager;
        
        [SerializeField]
        [JbzdReadOnly]
        private bool wasTriggered;

        [Inject]
        public void Construct(CutscenesManager cutscenesManager)
        {
            _cutscenesManager = cutscenesManager;
        }


        public void OnTriggerEnter(Collider other)
        {
            if (wasTriggered) return;
            
            var timelineAsset = playableDirector.playableAsset as TimelineAsset;
            _cutscenesManager.PlayCutscene(timelineAsset);
            wasTriggered = true;
        }
    }
}
