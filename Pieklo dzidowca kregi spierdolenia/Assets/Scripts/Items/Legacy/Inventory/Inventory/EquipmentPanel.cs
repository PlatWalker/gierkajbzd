using jbzdy.Items;
using UnityEngine;
using jbzdy.Items.Enums;

/// <summary>
/// Klasa siedząca na slotach, które służą do zakładania przedmiotów
/// 
/// Napisane przez Sharashino
/// Zmodyfikowane przez Kumdzio
/// </summary>
namespace jbzdy.Inventory 
{
    public class EquipmentPanel : MonoBehaviour
    {
        [HideInInspector] public GridSlot mainSlot;
        [HideInInspector] public int width, height;
        public SharItem equipedItem;
        
        private SharItem lastItem;  
        
        public SharItemTypes allowedItemType;
        public SharItem LastItem { get => lastItem; set => lastItem = value; }

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
        public virtual bool ItemFitsType(SharItem _toCheck)
        {
            if (_toCheck.itemType == allowedItemType) return true;
            return false;
        }
    }
}