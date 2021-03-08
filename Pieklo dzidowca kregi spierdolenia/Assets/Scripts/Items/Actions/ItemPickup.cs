using jbzdy.Items;
using UnityEngine;

/// <summary>
/// Napisane przez Sharashino
/// 
/// Skrypt umożliwiającymi obiektom bycie podniesionym
/// 
/// Posiada nadpisywalną metode z Interactables dla zmienienia logiki podczas interakcji z obiektem (tutaj podnoszenie przedmiotu)
/// </summary>
namespace jbzdy.Actions.Interaction
{
    public class ItemPickup : Interactables
    {
        [SerializeField] private Item item = default;

        public override void Interact()
        {
            base.Interact();
            PickUp();
        }

        private void PickUp()
        {
            Debug.Log("Picking up item: " + item.name);

            bool wasPickedUp = Inventory.instance.AddItem(item);

            if (wasPickedUp)
            {
                //Remove object from scene
                //Destroy(gameObject);
            }
        }
    }
}

