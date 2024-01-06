using jbzd.QuestSystem.QuestStructureElements;
using jbzd.Cutscenes;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using Zenject;
using MyBox;

namespace jbzd.QuestCreationScripts
{ 
    [RequireLayer("Player Triggers")]
    public class CutsceneTriggerOnQuest : MonoBehaviour
    {
        [field: SerializeField] private Quest _quest;

        public PlayableDirector playableDirector;
        private CutscenesManager _cutscenesManager;

        [Inject]
        public void Construct(CutscenesManager cutscenesManager)
        {
            _cutscenesManager = cutscenesManager;
        }

        private void OnTriggerEnter(Collider other)
        {

            if (_quest is null)
            {
                Debug.Log($"There is a trigger without quest on {name}. You need to place a quest or delete component");
                return;
            }

            if (_quest.IsCompleted)
            {
                TimelineAsset timelineAsset = playableDirector.playableAsset as TimelineAsset;
                _cutscenesManager.PlayCutscene(timelineAsset);
            }
        }
    }
}
