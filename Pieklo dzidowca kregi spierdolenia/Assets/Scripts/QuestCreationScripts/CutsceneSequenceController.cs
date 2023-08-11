using jbzd.Cutscenes;
using jbzd.QuestSystem.QuestStructureElements;
using UnityEngine;
using UnityEngine.Timeline;
using Zenject;

namespace jbzd.QuestCreationScripts
{
    public class CutsceneSequenceController : MonoBehaviour
    {
        [field: SerializeField] private Quest quest;
        [field: SerializeField] private TaskSO task;
        [field: SerializeField] private TimelineAsset asset;

        private CutscenesManager _cutscenesManager;
        
        [Inject]
        public void Constructor(CutscenesManager cutscenesManager)
        {
            _cutscenesManager = cutscenesManager;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (quest.IsCompleted || quest.ActiveTask != task) return;
            
            _cutscenesManager.PlayCutscene(asset);
        }
    }
}
