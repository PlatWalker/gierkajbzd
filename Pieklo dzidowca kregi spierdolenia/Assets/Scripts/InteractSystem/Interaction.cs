using jbzd.Common;
using jbzd.Common.InputSystem;
using jbzd.Common.InputSystem.Inputs;
using jbzd.Common.Interfaces;
using TMPro;
using UnityEngine;
using Zenject;

namespace jbzd.InteractSystem
{
    [RequireComponent(typeof(Collider))]
    public class Interaction : MonoBehaviour
    {
        private GameObject _pressE;
        public float infoHeight = 5.0f;
        public string infoText = string.Empty;
        public delegate void InteractionObjectReached(Collider somethingThatReachedInteractionObject);
        /// <summary>
        /// Event is called when object collider that is on layers collider matrix (most likely player)
        /// collide with this object.
        /// </summary>
        public event InteractionObjectReached OnInteractionObjectReach;
        
        private IInteractable[] _interactable;

        [field:JbzdReadOnly][field:SerializeField] public bool IsInRange { get; private set; }

        private PlayerInput _inputController;
        
        [Inject]
        public void Construct(InputManager inputManager)
        {
            _inputController = inputManager.GetInput<PlayerInput>();          
        }
        
        private void Awake()
        {
            if (transform.parent is null)
            {
                Debug.LogError("Obiekt interakcji musi mieć rodzica");
                return;
            }
            
            var interactable = GetComponentsInParent<IInteractable>();

            
            if (interactable.Length == 0)
            {
                Debug.LogError($"Rodzic ({transform.parent.name}) obiektu interakcji nie posiada interfejsu IInteractable.");
                return;
            }

            _interactable = interactable;
            _inputController.OnInteractClick += OnInteract;

            if (_pressE == null)
            {
                _pressE = Instantiate(Resources.Load<GameObject>("Press_e"));
                _pressE.transform.parent = transform;
            }

            _pressE.SetActive(false);
            _pressE.transform.localPosition = new Vector3(0, infoHeight, 0);
            _pressE.transform.localScale = new Vector3(-1, 1, -1);
            _pressE.GetComponentInChildren<TextMeshPro>().text = infoText;
        }

        private void OnInteract()
        {
            foreach (var interactable in _interactable)
            {
                if (IsInRange) interactable.OnInteract();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;
            
            IsInRange = true;
            OnInteractionObjectReach?.Invoke(other);
            _pressE.SetActive(IsInRange);
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;

            IsInRange = true;
            _pressE.SetActive(IsInRange);
        }

        private void OnTriggerExit(Collider other)
        {
            IsInRange = false;
            _pressE.SetActive(IsInRange);
        }

        private void OnDestroy()
        {
            _inputController.OnInteractClick -= OnInteract;
        }
    }
}
