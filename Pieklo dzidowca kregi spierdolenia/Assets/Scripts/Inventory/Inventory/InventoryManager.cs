using UnityEngine;
using UnityEngine.Events;

namespace jbzdy.Inventory
{
    [System.Serializable]
    public class OnInventoryOpen : UnityEvent { }
    [System.Serializable]
    public class OnInventoryClose : UnityEvent { }

    public class InventoryManager : MonoBehaviour
    {
        /// <summary>
        /// InventoryManager aviliable modes
        /// </summary>
        public enum ActiveMode { loot, inventory }

        /// <summary>
        /// Current InventoryManager mode
        /// </summary>
        public ActiveMode mode = ActiveMode.inventory;

        /// <summary>
        /// Inventory canvas
        /// </summary>
        Canvas canvas;

        /// <summary>
        /// Use this variable to show or close inventory! Not requires invmanager reference. Static variable
        /// </summary>
        public static bool showInventory = false;

        /// <summary>
        /// Toggle to prevent update execution each frame
        /// </summary>
        private bool isOpen = false;

        /// <summary>
        /// KeyCode to open inventory
        /// </summary>
        public KeyCode inventoryOpenKey = KeyCode.I;
        
        public bool lookCursorWhenInventoryOff = true;

        public OnInventoryOpen OnOpen;
        public OnInventoryClose OnClose;

        public InventoryClass inventory;


        private void Start()
        {
            showInventory = false;
            isOpen = false;
            InventoryClose();
            canvas.enabled = false;
        }

        private void OnEnable()
        {
            if(canvas == null)
                canvas = GetComponent<Canvas>();

            if (inventory == null)
                inventory = InventoryClass.Instance;

            InventoryClose();
        }

        private void Update()
        {
            if (Input.GetKeyDown(inventoryOpenKey))
            {
                showInventory = !showInventory;
            }

            if (showInventory)
            {
                InventoryOpen();
            }
            else
            {
                mode = ActiveMode.inventory;
                InventoryClose();
            }

            if (mode == ActiveMode.inventory)
            {
                if (inventory.lootPanel != null)
                    inventory.lootPanel.gameObject.SetActive(false);
            }
            else if (mode == ActiveMode.loot)
            {
                if (inventory.lootPanel != null)
                    inventory.lootPanel.gameObject.SetActive(true);
            }
        }
    
        /// <summary>
        /// Method called to open inventory. Has event callback
        /// </summary>
        private void InventoryOpen()
        {
            if (isOpen)
                return;
            else
            {
                canvas.enabled = true;
                OnOpen.Invoke();
                isOpen = true;
            }
        }

        /// <summary>
        /// Method called to close inventory. Has event callback
        /// </summary>
        private void InventoryClose()
        {
            if (!isOpen)
                return;
            else
            {
                canvas.enabled = false;
                OnClose.Invoke();
                isOpen = false;
            }
        }

        /// <summary>
        /// Use this method to open - close inventory with button on mobile
        /// </summary>
        public void MobileToggle()
        {
            showInventory = !showInventory;
        }
    }
}