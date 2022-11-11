using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Główny manager ekwipunku gracza, odpowiadający za on/off inventory, rozpisane wszystko stylem Kumdzia żeby nikt nie pierdolił że nie wie co od czego
/// 
/// Napisane przez Sharashino
/// </summary>
namespace jbzdy.Inventory
{
    [System.Serializable]
    public class OnInventoryOpen : UnityEvent { }
    [System.Serializable]
    public class OnInventoryClose : UnityEvent { }
    
    [Obsolete("system sharashino...")]
    public class InventoryManager : MonoBehaviour
    {
        /// InventoryManager aviliable modes
        public enum ActiveMode { loot, inventory }

        /// Current InventoryManager mode
        public ActiveMode mode = ActiveMode.inventory;

        /// Inventory canvas
        public Canvas canvas;

        /// Use this variable to show or close inventory! Not requires invmanager reference. Static variable
        public static bool showInventory = false;

        /// Toggle to prevent update execution each frame
        private bool isOpen = false;

        /// KeyCode to open inventory
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
    }
}