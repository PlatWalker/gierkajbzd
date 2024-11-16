using jbzd.Common.Interfaces;
using jbzd.MinorSystems.Cutscenes;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Zenject;

namespace jbzd.QuestCreationScripts
{
    public class CutsceneTriggerOnInteract : MonoBehaviour, IInteractable
    {
        public PlayableDirector playableDirector;
        private CutscenesManager _cutscenesManager;
        private bool _wasTriggered = false;

        [Inject]
        public void Construct(CutscenesManager cutscenesManager)
        {
            _cutscenesManager = cutscenesManager;
        }

        public void OnInteract()
        {
            if (_wasTriggered) return;
            _wasTriggered = true;
            TimelineAsset timelineAsset = playableDirector.playableAsset as TimelineAsset;
            _cutscenesManager.PlayCutscene(timelineAsset);
        }
    }
}
