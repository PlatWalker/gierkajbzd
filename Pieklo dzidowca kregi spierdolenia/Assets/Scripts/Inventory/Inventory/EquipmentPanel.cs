using jbzdy.Items;
using UnityEngine;
using jbzdy.Items.Enums;

/// <summary>
/// Klasa siedząca na slotach, które służą do zakładania przedmiotów
/// 
/// Napisane przez Sharashino
/// </summary>
namespace jbzdy.Inventory 
{
    public class EquipmentPanel : MonoBehaviour
    {
        [HideInInspector]
        public GridSlot mainSlot;
        [HideInInspector]
        public int width, height;
        public Item equipedItem;
        private Item lastItem;  
        
        public ItemTypes allowedItemType;

        [Header("Using ids ignore allowedItemType. Only specified id items will be equiped")]
        public int[] allowedIds;

        public Item LastItem { get => lastItem; set => lastItem = value; }

        private void Update()
        {
            if(equipedItem != null && lastItem == null)
            {
                lastItem = equipedItem;
            }

            if(equipedItem == null && lastItem != null)
            {
                    lastItem = null;
            }
        }
    }
}