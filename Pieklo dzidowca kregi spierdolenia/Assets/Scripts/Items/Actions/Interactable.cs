using UnityEngine;

// <summary>
// Napisane przez Sharashino
// 
// Główna klasa rzeczy z którymi możemy wchodzić w interakcję, obecnie przyciskiem E (Sklepy, skrzynie, lootowanie, interakcja z postaciami)
// </summary>
namespace jbzdy.Actions.Interaction
{
    public class Interactable : MonoBehaviour
    {
        [SerializeField] private InteractionZone interactionZone;
        [SerializeField] private bool isInteracting;

        public InteractionZone InteractionZone { get => interactionZone; set => interactionZone = value; }
        public bool IsInteracting { get => isInteracting; set => isInteracting = value; }

        public delegate void Interacted(GameObject gameObject);
        public static event Interacted OnInteraction;

        public void Awake()
        {
            interactionZone = GetComponentInChildren<InteractionZone>();
        }

        public void Update()
        {
            if (interactionZone.IsInRange)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    isInteracting = true;
                    Interact();
                }
            }
        }

        public virtual void Interact()
        {
            OnInteraction?.Invoke(this.gameObject);
        }

        public virtual void StopInteract()
        {

        }
    }
}