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
        /// <summary>
        /// An item that panel stores
        /// </summary>
        public Item equipedItem;
        
        /// <summary>
        /// We call update only if item in not the same as before. Made for optimization
        /// </summary>
        private Item lastItem;

        [HideInInspector]
        public GridSlot mainSlot;
        
        [HideInInspector]
        public int width, height;

        /// <summary>
        /// Item type allowed for this slot
        /// </summary>
        public ItemTypes allowedItemType;
        public ArmorTypes allowedArmorType;

        [Header("Using ids ignore allowedItemType. Only specified id items will be equiped")]
        public int[] allowedIds;

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