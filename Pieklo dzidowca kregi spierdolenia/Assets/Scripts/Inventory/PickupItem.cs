using jbzdy.Items;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Testowa klasa do podnoszenia itemów na klik myszy i ogólnie do wyjebania jest
/// 
/// Napisane przez Sharashino
/// </summary>
namespace jbzdy.Inventory
{
    public enum InteractionType { raycastFromCamera, clickToPickup, triggerPickup }

    public class PickupItem : MonoBehaviour
    {
        public InteractionType interactionType;
        public KeyCode pickupKey = KeyCode.F;
        public Transform playerCamera;

        public float raycastPickupDistance = 3f;

        [HideInInspector]
        public InventoryClass    inventory;

        public Text itemNameTooltip;

        private void Start()
        {
            if(itemNameTooltip!=null)
                itemNameTooltip.text = string.Empty;

            if (playerCamera == null && Camera.main != null)
                playerCamera = Camera.main.transform;

            if (inventory == null)
                inventory = FindObjectOfType<InventoryClass>();
        }

        private void Update()
        {
            switch (interactionType)
            {
                case InteractionType.clickToPickup:
                    //Do nothing. Behavior presented by Unity OnPonterClick interface. No need for realization special method
                    break;
                case InteractionType.raycastFromCamera:
                    PickupWithRaycast();
                    break;
                case InteractionType.triggerPickup:
                    //Do nothing. Behavior presented by OnTriggerEnter method
                    break;
            }
        }
        
        public void PickupWithRaycast()
        {
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, raycastPickupDistance))
            {
                if(itemNameTooltip)
                    itemNameTooltip.text = string.Empty;

                if (hit.collider.CompareTag("Item") && hit.collider.GetComponent<Item>() != null)
                {
                    var item = hit.collider.GetComponent<Item>();

                    if (itemNameTooltip)
                    {
                        if(item.isStackable)
                            itemNameTooltip.text = string.Format("{0}x{1}", item.itemName, item.itemStackSize);
                        else
                            itemNameTooltip.text = string.Format("{0}", item.itemName);
                    }

                    if (Input.GetKeyDown(pickupKey))
                        inventory.AddItem(hit.collider.GetComponent<Item>());
                }

                if (hit.collider.CompareTag("LootBox") && hit.collider.GetComponent<LootBox>() != null && !InventoryManager.showInventory)
                {
                    if (itemNameTooltip)
                        itemNameTooltip.text = "Search";

                    if (Input.GetKeyDown(pickupKey))
                        inventory.SearchLootBox(hit.collider.GetComponent<LootBox>());
                }
                
            }
            else
            {
                if (itemNameTooltip != null && itemNameTooltip.text != string.Empty)
                {
                    itemNameTooltip.text = string.Empty;
                }
            }

        }

        public void InspectLootBoxWithRaycast()
        {
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, raycastPickupDistance))
            {
                if (itemNameTooltip)
                    itemNameTooltip.text = string.Empty;
                
                if (hit.collider.CompareTag("LootBox") && hit.collider.GetComponent<LootBox>() != null && !InventoryManager.showInventory)
                {
                    if (itemNameTooltip)
                        itemNameTooltip.text = "Search";

                    if (Input.GetKeyDown(pickupKey))
                        inventory.SearchLootBox(hit.collider.GetComponent<LootBox>());
                }

            }
            else
            {
                if (itemNameTooltip != null && itemNameTooltip.text != string.Empty)
                {
                    itemNameTooltip.text = string.Empty;
                }
            }
        }

        public void InspectLootBox(LootBox lootBox)
        {
            inventory.SearchLootBox(lootBox);
        }

        private void OnTriggerStay(Collider other)
        {
            print(other.name);

            if (interactionType != InteractionType.triggerPickup)
            {
                print("Return");
                return;
            }

            if (Input.GetKeyDown(pickupKey))
            {
                if (!other.GetComponent<Item>()) print("Other item equal null");
                if (other.GetComponent<Item>()) print("Other item has item component");

                if (other.CompareTag("Item") && other.GetComponent<Item>() != null)
                {
                    inventory.AddItem(other.GetComponent<Item>());
                }
            }
        }

    }
}