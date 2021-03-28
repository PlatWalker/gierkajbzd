using jbzdy.Inventory.Equipment;
using jbzdy.Inventory.Items;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace jbzdy.Inventory
{
    [System.Serializable] public class OnInventoryItemAdd1 : UnityEvent { }
    [System.Serializable] public class OnInventoryItemDrop1 : UnityEvent { }
    [System.Serializable] public class OnInventoryItemRemove1 : UnityEvent { }


    public class InventoryClass1 : MonoBehaviour
    {
        public static InventoryClass1 Instance;

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
        }


        public OnInventoryItemAdd1 OnInventoryItemAdd;
        public OnInventoryItemDrop1 OnInventoryItemDrop;

        public List<InventoryItem1> inventoryItems = new List<InventoryItem1>();
        public List<EquipmentPanel1> equipmentPanels = new List<EquipmentPanel1>();

        public Transform player;

        public Image cellImage;

        public int cellSize = 75;

        public int padding;

        public int column;

        public int row;

        public int lootColumn;

        public int lootRow;

        public Color defaultColor;

        public Color hoveredColor;

        public Color blockedColor;

        public RectTransform lootPanel;

        internal void DrawPreview()
        {
            throw new NotImplementedException();
        }
    }

}
