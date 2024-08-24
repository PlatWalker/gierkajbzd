using jbzd.MinorSystems.Barks;
using jbzd.MinorSystems.Interact;
using jbzd.QuestSystem;
using jbzd.QuestSystem.QuestStructureElements;
using UnityEngine;
using Zenject;

namespace jbzd.QuestCreationScripts
{
    public class UnlockInteractionOnTask : MonoBehaviour
    {
        [SerializeField] private TaskSO taskToCheck;
        
        [field:SerializeField]
        public bool ShouldShowDenialText { get; set; } = true;
        
        private Quest _questWithTask;
        private QuestManager _questManager;
        private Interaction _interaction;
        private PlayerThoughtBarkController _playerThoughtBarkController;


        [Inject]
        public void Constructor(QuestManager questManager)
        {
            _questManager = questManager;
        }

        private void Awake()
        {
            _interaction = GetComponentInChildren<Interaction>();
            
            Debug.Assert(taskToCheck, $"Missing configuration field {nameof(taskToCheck)} on object {gameObject.name}");
            Debug.Assert(_interaction, $"Missing interaction script in children on object {gameObject.name}");

            if (ShouldShowDenialText)
            {
                _playerThoughtBarkController = GetComponentInChildren<PlayerThoughtBarkController>();
                Debug.Assert(_playerThoughtBarkController, $"Missing PlayerThoughtBark in children on object {gameObject.name}");
            }
        }

        private void Start()
        {
            if (_interaction is null) return;
            
            _interaction.IsInteractable = false;
                
            _questWithTask = _questManager.GetQuestWithThisTaskSo(taskToCheck);
            _questWithTask.OnTaskStarted += OnTaskStarted;
        }

        private void OnTaskStarted(Quest questInvoked, TaskSO taskUpdated)
        {
            if (taskUpdated != taskToCheck) return;

            _interaction.IsInteractable = true;
            ShouldShowDenialText = false;
        }
        
        private void Update()
        {
            if (_interaction.IsInRange && ShouldShowDenialText && _interaction.IsInteractable is false)
            {
                _playerThoughtBarkController?.ShowPlayerThoughtBark();
            }
            else
            {
                _playerThoughtBarkController?.HidePlayerThoughtBark();
            }
        }
        
        private void OnDestroy()
        {
            if (_questWithTask) _questWithTask.OnTaskStarted -= OnTaskStarted;
        }
    }
}
