using System.Collections;
using jbzd.QuestSystem;
using jbzd.QuestSystem.QuestStructureElements;
using UnityEngine;
using Zenject;

namespace jbzd.MinorSystems.Barks
{
    public class PlaysBarkOnOutOfAreaOnTask: MonoBehaviour
    {
        private BarkController _controller;
        private Quest _questWithTask;
        private bool _isInArea;
        private bool _isActive;
        
        [SerializeField]
        private TaskSO taskTriggeringBarks;
        
        [SerializeField]
        private Collider area;

        [SerializeField]
        [Tooltip("Time until barks starts displaying")]
        private int timeUntilBarksStarts;
        
        private QuestManager _questManager;
        
        [Inject]
        public void Construct(QuestManager questManager)
        {
            _questManager = questManager;
        }
        
        public void Awake()
        {
            _controller = GetComponentInChildren<BarkController>();
            
            Debug.Assert(_controller is not null, $"There should be {nameof(BarkController)} on one of this children objects");
        }

        public void Start()
        {
            Debug.Assert(area.gameObject.layer == LayerMask.NameToLayer("Area"),"Given area should have layer \"Area\" ");
            
            _questWithTask = _questManager.GetQuestWithThisTaskSo(taskTriggeringBarks);

            _questWithTask.OnTaskStarted += OnTaskStarted;
            _questWithTask.OnTaskEnded += OnTaskEnded;
        }
        
        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Area")) return;
            if (other != area) return;
            
            _isInArea = true;
            _controller.StopRandomBarks();
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Area")) return;
            if (other != area) return;
            
            _isInArea = false;
            StartBarks();
        }

        private void StartBarks()
        {
            if (_isActive)
            {
                _controller.StartRandomBarks();
            }
        }

        private void OnTaskEnded(Quest questInvoked, TaskSO taskUpdated)
        {
            if (taskUpdated != taskTriggeringBarks) return;
            
            _controller.StopRandomBarks();
            _isActive = false;
        }

        private void OnTaskStarted(Quest questInvoked, TaskSO taskUpdated)
        {
            if (taskUpdated != taskTriggeringBarks) return;
            
            if (_isInArea == false)
            {
                StartCoroutine(StartBarksFromBeginning());
            }
            
            _isActive = true;
        }

        private IEnumerator StartBarksFromBeginning()
        {
            yield return new WaitForSeconds(timeUntilBarksStarts);
            StartBarks();
        }
        
        public void OnDestroy()
        {
            _questWithTask.OnTaskStarted -= OnTaskStarted;
            _questWithTask.OnTaskEnded -= OnTaskEnded;
        }
    }
}