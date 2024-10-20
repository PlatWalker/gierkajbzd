using jbzd.Common;
using jbzd.MinorSystems.Cutscenes;
using jbzd.QuestSystem.QuestStructureElements;
using jbzd.QuestSystem;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Zenject;
using MyBox;
using System.Linq;

namespace jbzd.QuestCreationScripts
{

    [RequireLayer("Player Triggers")]
    public class CutsceneTriggerEndGoalOnTask : MonoBehaviour
    {
        [field: SerializeField] private TaskSO _task;
        [field: SerializeField] private GoalSO _goal;
        [JbzdReadOnly][field: SerializeField] private bool _wasTriggered = false;

        public PlayableDirector playableDirector;
        private CutscenesManager _cutscenesManager;
        private QuestManager _questManager;

        [Inject]
        public void Construct(CutscenesManager cutscenesManager, QuestManager questManager)
        {
            _cutscenesManager = cutscenesManager;
            _questManager = questManager;
        }

        public void Start()
        {
            Debug.Assert(_task, ErrorTemplate.NotAssignedVariable(nameof(_task), gameObject));
            Debug.Assert(playableDirector, ErrorTemplate.NotAssignedVariable(nameof(playableDirector), gameObject));
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_wasTriggered) return;

            foreach (var quest in _questManager.ActiveQuests.Where(quest => quest.ActiveTask == _task))
            {
                var timelineAsset = playableDirector.playableAsset as TimelineAsset;
                _cutscenesManager.PlayCutscene(timelineAsset);
                if (_goal != null)
                {
                    quest.MakeActorPlayInThisQuest(_goal);
                }
                _wasTriggered = true;
            }
        }
    }
}
