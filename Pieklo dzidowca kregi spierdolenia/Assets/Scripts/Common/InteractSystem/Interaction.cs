using System;
using jbzd.Common.InputSystem;
using jbzd.Common.InputSystem.Inputs;
using jbzd.Common.Interfaces;
using UnityEngine;
using Zenject;

namespace jbzd.Common.InteractSystem
{
    [RequireComponent(typeof(Collider))]
    public class Interaction : MonoBehaviour
    {
        public delegate void InteractionObjectReached(Collider somethingThatReachedInteractionObject);
        public event InteractionObjectReached OnInteractionObjectReach;
        
        private IInteractable _interactable;

        [field:JbzdReadOnly][field:SerializeField] public bool IsInRange { get; private set; }

        private PlayerInput _inputController;
        
        [Inject]
        public void Construct(InputManager inputManager)
        {
            _inputController = inputManager.GetInput<PlayerInput>();
        }
        
        public void Awake()
        {
            if (TryGetComponent(out _interactable))
            {
                _inputController.OnInteractClick += OnInteract;
            }
            else
            {
                Debug.LogError("Obiekt interakcji nie posiada rodzica z interfejsem IInteractable" +
                          "lub nie zaznaczyłeś, że jest on wolnostojący!");
            }
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

        public void OnDestroy()
        {
            _inputController.OnInteractClick -= OnInteract;
        }

    }
}
