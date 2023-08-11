using jbzd.Common;
using jbzd.Common.InputSystem;
using jbzd.Common.InputSystem.Inputs;
using jbzd.Common.Interfaces;
using UnityEngine;
using Zenject;

namespace jbzd.InteractSystem
{
    [RequireComponent(typeof(Collider))]
    public class Interaction : MonoBehaviour
    {
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
                Debug.LogError("Rodzic obiektu interakcji nie posiada interfejsu IInteractable.");
                return;
            }

            _interactable = interactable;
            _inputController.OnInteractClick += OnInteract;
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
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;

            IsInRange = true;
        }

        private void OnTriggerExit(Collider other)
        {
            IsInRange = false;
        }

        private void OnDestroy()
        {
            _inputController.OnInteractClick -= OnInteract;
        }

    }
}
