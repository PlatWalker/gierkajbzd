using System;
using System.Linq;
using jbzd.Common;
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

        public void Start()
        {
            Debug.Assert(_task, ErrorTemplate.NotAssignedVariable(nameof(_task), gameObject));
            Debug.Assert(playableDirector, ErrorTemplate.NotAssignedVariable(nameof(playableDirector), gameObject));
        }

        private void OnTriggerEnter(Collider other)
        {
            foreach (var _ in _questManager.ActiveQuests.Where(quest => quest.ActiveTask == _task))
            {
                var timelineAsset = playableDirector.playableAsset as TimelineAsset;
                _cutscenesManager.PlayCutscene(timelineAsset);
            }
        }
    }
}
