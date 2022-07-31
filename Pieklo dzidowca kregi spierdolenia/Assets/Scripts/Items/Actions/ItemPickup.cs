using jbzdy.Items;
using jbzdy.Inventory;
using UnityEngine;

// <summary>
// Napisane przez Sharashino
// 
// Skrypt umożliwiającymi obiektom bycie podniesionym
// 
// Posiada nadpisywalną metode z Interactables dla zmienienia logiki podczas interakcji z obiektem (tutaj podnoszenie przedmiotu)
// </summary>
namespace jbzdy.Actions.Interaction
{
    public class ItemPickup : Interactable
    {
        [SerializeField] private Item item = default;

        public InventoryClass inventory;

        public Item Item { get => item; set => item = value; }
        
        public new void Awake()
        {
            item = this.GetComponent<Item>();
            inventory = InventoryClass.Instance;            
        }
        
        
        public override void Interact()
        {
            base.Interact();
            PickUp();
        }

        private void PickUp()
        {
            InventoryClass.Instance.AddItem(item);
        }
    }
}

