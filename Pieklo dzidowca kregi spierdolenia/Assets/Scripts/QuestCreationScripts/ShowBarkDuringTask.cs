using jbzd.MinorSystems.Barks;
using jbzd.QuestSystem;
using jbzd.QuestSystem.QuestStructureElements;
using UnityEngine;
using Zenject;

namespace jbzd.QuestCreationScripts
{
    public class ShowBarkDuringTask : MonoBehaviour
    {
        private BarkController _barkController;
        private QuestManager _questManager;
        [SerializeField] private TaskSO taskToCheck;
        
        [Inject]
        public void Constructor(QuestManager questManager)
        {
            _questManager = questManager;
        }
        
        void Start()
        {
            _barkController = GetComponentInChildren<BarkController>();
            Debug.Assert(_barkController, $"Missing {nameof(BarkController)} on {name}");

            foreach (var quest in _questManager.AllQuestsFromLoadedMaps)
            {
                quest.OnTaskStarted += (_, _) =>
                {
                    if (taskToCheck == null) return;
                    if (quest.ActiveTask != taskToCheck) return;
                    _barkController.StartRandomBarks();
                };
                quest.OnTaskEnded += (_, _) =>
                {
                    if (taskToCheck == null) return;
                    if (quest.ActiveTask != taskToCheck) return;
                    _barkController.StopRandomBarks();
                };
            }
        }

        
    }
}
