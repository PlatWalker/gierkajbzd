using jbzd.MinorSystems.Cutscenes;
using jbzd.QuestSystem.QuestStructureElements;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using Zenject;
using MyBox;
using jbzd.QuestSystem;

namespace jbzd.QuestCreationScripts
{ 
    [RequireLayer("Player Triggers")]
    public class CutsceneTriggerOnTask : MonoBehaviour
    {
        [field: SerializeField] private TaskSO _task;

        public PlayableDirector playableDirector;
        private CutscenesManager _cutscenesManager;
        private QuestManager _questManager;

        [Inject]
        public void Construct(CutscenesManager cutscenesManager, QuestManager questManager)
        {
            _cutscenesManager = cutscenesManager;
            _questManager = questManager;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_task is null)
            {
                Debug.Log($"There is a trigger without task on {name}. You need to place a quest or delete component");
                return;
            }

            foreach (var quest in _questManager.ActiveQuests)
            {
                if (quest.ActiveTask == _task)
                {
                    TimelineAsset timelineAsset = playableDirector.playableAsset as TimelineAsset;
                    _cutscenesManager.PlayCutscene(timelineAsset);
                }
            }               
        }
    }
}
