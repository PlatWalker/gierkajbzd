using UnityEngine;

/// <summary>
/// Made by sharashino
/// 
/// Każdy obiekt z którym możemy wejść w interakcję musi mieć tą klase
/// </summary>
namespace jbzdy.Actions.Interaction
{
    public class ItemInteract : Interactables
    {
        [SerializeField] private Item item;

        public override void Interact()
        {
            if (item.CanPickUp)
            {
                base.Interact();
                PickUpItem();
            }
        }

        private void PickUpItem()
        {
            Debug.Log("picking up " + item.name);

            Destroy(gameObject);
        }
    }
}
