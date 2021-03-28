using jbzdy.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;


namespace jbzdy.Inventory.Items
{
    public class InventoryItem1 : MonoBehaviour
    {
        public int x, y, width, height;
        private Image itemmImage;
        private Item item;
        private RectTransform rect;
        internal InventoryClass1 inventory;
        private PointerEventData dragEventData;
        private bool isDragging;
        private Vector2 lastPosition;
        public TMP_Text stackText;




    }

}
