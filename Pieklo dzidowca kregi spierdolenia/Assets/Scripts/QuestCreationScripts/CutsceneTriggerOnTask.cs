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
    public class CutsceneTriggerOnTask : MonoBehaviour
    {
        [field: SerializeField] private Quest _quest;
        [field: SerializeField] private TaskSO _task;

        public PlayableDirector playableDirector;
        private CutscenesManager _cutscenesManager;

        [Inject]
        public void Construct(CutscenesManager cutscenesManager)
        {
            _cutscenesManager = cutscenesManager;
        }

        private void OnTriggerEnter(Collider other)
        {

            if (_quest is null || _task is null)
            {
                Debug.Log($"There is a trigger without quest or task on {name}. You need to place a quest or delete component");
                return;
            }

            if (_quest.ActiveTask == _task)
            {
                TimelineAsset timelineAsset = playableDirector.playableAsset as TimelineAsset;
                _cutscenesManager.PlayCutscene(timelineAsset);
            }
        }
    }
}
