using System;
using jbzd.InteractSystem;
using jbzd.QuestSystem;
using jbzd.QuestSystem.QuestStructureElements;
using UnityEngine;
using Zenject;

namespace jbzd.QuestCreationScripts
{
    public class UnlockInteractionOnTask : MonoBehaviour
    {
        [SerializeField] private TaskSO taskToCheck;
        
        [Tooltip("If not added, nothing will be displayed.")]
        [SerializeField] 
        private GameObject interactionDenialText;
        
        [field:SerializeField]
        public bool ShouldShowDenialText { get; set; } = true;
        
        private Quest _questWithTask;
        private QuestManager _questManager;
        private Camera _camera;
        private Interaction _interaction;

        

        [Inject]
        public void Constructor(QuestManager questManager)
        {
            _questManager = questManager;
        }

        private void Awake()
        {
            _camera = Camera.main;
            _interaction = GetComponentInChildren<Interaction>();
            
            Debug.Assert(_interaction, $"Missing interaction script in children on object {gameObject.name}");
        }

        private void Start()
        {
            _camera = Camera.main;
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
            if (interactionDenialText is null) return;
            
            if (_interaction.IsInRange && ShouldShowDenialText)
            {
                interactionDenialText.SetActive(true);
                interactionDenialText.transform.LookAt(_camera?.transform);
            }
            else
            {
                interactionDenialText.SetActive(false);
            }
        }
        
        private void OnDestroy()
        {
            _questWithTask.OnTaskStarted -= OnTaskStarted;
        }
    }
}
