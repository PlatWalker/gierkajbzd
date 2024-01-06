using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using jbzd.Cutscenes;
using jbzd.Dialogues;
using Zenject;
using MyBox;

namespace jbzd.QuestCreationScripts
{ 
    [RequireComponent(typeof(Collider))]
    [RequireLayer("Player Triggers")]
    public class CutsceneTrigger : MonoBehaviour
    {
        public PlayableDirector playableDirector;
        private CutscenesManager _cutscenesManager;
        private bool wasTriggered = false;

        [Inject]
        public void Construct(CutscenesManager cutscenesManager)
        {
            _cutscenesManager = cutscenesManager;
        }


        public void OnTriggerEnter(Collider other)
        {
            if (!wasTriggered)
            {
                TimelineAsset timelineAsset = playableDirector.playableAsset as TimelineAsset;
                _cutscenesManager.PlayCutscene(timelineAsset);
                wasTriggered = true;
            }
        }
    }
}
