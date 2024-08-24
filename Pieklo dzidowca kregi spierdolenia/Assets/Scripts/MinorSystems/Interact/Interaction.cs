using jbzd.Common;
using jbzd.Common.Interfaces;
using jbzd.MinorSystems.InputSystem;
using jbzd.MinorSystems.InputSystem.Inputs;
using TMPro;
using UnityEngine;
using Zenject;

namespace jbzd.MinorSystems.Interact
{
    [RequireComponent(typeof(Collider))]
    public class Interaction : MonoBehaviour
    {
        [field: SerializeField] public bool IsInteractable { get; set; } = true;

        [field:TextArea]
        [field:SerializeField]
        public string Warning { get; set; } = "Ten skrypt może być tylko i wylacznie na przygotowanym prefabie znajdź go w assetach. Opisane w dokumentacji.";
        
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
            Debug.Assert(gameObject.name is "InteractionSpace" or "IteractionSpace", 
                $"Ten skrypt może być tylko i wylacznie na przygotowanym prefabie, a jest na {gameObject.name}. Opisane w dokumentacji.");
            
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
                if (IsInRange && IsInteractable) interactable.OnInteract();
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
