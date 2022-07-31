using jbzd;
using jbzdy.Items;
using UnityEngine;
using jbzdy.Items.Enums;

/// <summary>
/// Klasa siedząca na slotach, które służą do zakładania przedmiotów
/// 
/// Napisane przez Sharashino
/// Zmodyfikowana przez Kumdzio
/// </summary>
namespace jbzdy.Inventory
{
    public class ArmorEquipmentPanel : EquipmentPanel
    {
        public ArmorTypes allowedArmorType;

        private void Update()
        {
            if (equipedItem != null && LastItem == null)
            {
                if (equipedItem.itemType == ItemTypes.Armor)
                {
                    if(((ArmorItem)equipedItem).armorType != allowedArmorType)
                    {
                        equipedItem = null;
                        return;
                    }
                }
                LastItem = equipedItem;
                GameManager.Instance.PlayerObject.GetComponent<jbzdy.Items.ItemEquipper>().EquipArmor((ArmorItem)equipedItem);
            }

            if (equipedItem == null && LastItem != null)
            {
                GameManager.Instance.PlayerObject.GetComponent<jbzdy.Items.ItemEquipper>().UnequipArmor((ArmorItem)LastItem);
                LastItem = null;
            }
        }

        override public bool ItemFitsType(Item _toCheck)
        {
            if (_toCheck.itemType == ItemTypes.Armor)
            {
                if (((ArmorItem)_toCheck).armorType == allowedArmorType)
                    return true;
            }
            return false;
        }
    }
}