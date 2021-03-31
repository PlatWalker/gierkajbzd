using UnityEngine;

/// <summary>
/// Napisane przez Sharashino
/// 
/// Główna klasa rzeczy z którymi możemy wchodzić w interakcję, obecnie przyciskiem E (Sklepy, skrzynie, lootowanie, interakcja z postaciami)
/// </summary>
namespace jbzdy.Actions.Interaction
{
    public class Interactables : MonoBehaviour
    {
        [SerializeField] private InteractionZone interactionZone;

        public InteractionZone InteractionZone { get => interactionZone; set => interactionZone = value; }

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
                    Interact();
                }
            }
        }

        public virtual void Interact()
        {

        }
    }
}