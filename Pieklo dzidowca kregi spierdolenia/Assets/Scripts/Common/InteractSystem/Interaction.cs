using jbzd.Common.InputSystem;
using jbzd.Common.InputSystem.Inputs;
using jbzd.Common.Interfaces;
using jbzdy.Player;
using UnityEngine;
using Zenject;

namespace jbzd.Common.InteractSystem
{
    public class Interaction : MonoBehaviour
    {
        public delegate void InteractionObjectReached(Collider somethingThatReachedInteractionObject);
        public event InteractionObjectReached OnInteractionObjectReach;
        
        private IInteractable _interactable;

        [field:JbzdReadOnly][field:SerializeField] public bool IsInRange { get; private set; }
        [field:SerializeField] public bool IsStandalone { get; private set; }
        
        private PlayerInput _inputController;
        
        [Inject]
        public void Construct(InputManager inputManager)
        {
            _inputController = inputManager.GetInput<PlayerInput>();
        }
        
        public void Start()
        {
            _inputController.OnInteractClick += OnInteract;

            if (GetComponentInParent<IInteractable>() != null)
            {
                _interactable = GetComponentInParent<IInteractable>();
            }
            else if (!IsStandalone)
            {
                Debug.Log("Obiekt interakcji nie posiada rodzica z interfejsem IInteractable" +
                          "lub nie zaznaczyłeś, że jest on wolnostojący!");
            }

            if (GetComponent<Collider>() == null) Debug.Log("Obiekt interakcji musi mieć jakikolwiek collider!");
        }

        public void OnInteract()
        {
            if (IsInRange) _interactable.OnInteract();
        }

        private void OnTriggerEnter(Collider other)
        {
            IsInRange = true;
            OnInteractionObjectReach?.Invoke(other);
        }

        private void OnTriggerStay(Collider other)
        {
            IsInRange = true;
        }

        private void OnTriggerExit(Collider other)
        {
            IsInRange = false;
        }
    }
}
