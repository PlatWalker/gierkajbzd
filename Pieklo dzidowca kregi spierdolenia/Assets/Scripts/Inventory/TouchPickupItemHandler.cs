using UnityEngine;
using jbzdy.Items;
using jbzdy.Inventory;
using UnityEngine.EventSystems;

namespace jbzdy.Inventory
{
    public class TouchPickupItemHandler : MonoBehaviour, IPointerClickHandler
    {
        PickupItem pickupItem;
        InventoryClass inventory;

        private void OnEnable()
        {
            pickupItem = FindObjectOfType<PickupItem>();
            inventory = FindObjectOfType<InventoryClass>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (pickupItem.interactionType != InteractionType.clickToPickup)
                return;

            if (eventData.hovered[0].gameObject.GetComponent<Item>() != null)
            {
                inventory.AddItem(eventData.hovered[0].gameObject.GetComponent<Item>());
            }
        }
    }
}