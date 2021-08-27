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
    public class ArmorEquipmentPanel : EquipmentPanel
    {
        public ArmorTypes allowedArmorType;

        private void Update()
        {
            if (equipedItem != null && LastItem == null)
            {
                LastItem = equipedItem;
                GameManager.Instance.PlayerObject.GetComponent<jbzdy.Items.ItemEquipper>().EquipArmor((ArmorItem)equipedItem);
            }

            if (equipedItem == null && LastItem != null)
            {
                GameManager.Instance.PlayerObject.GetComponent<jbzdy.Items.ItemEquipper>().UnequipArmor((ArmorItem)LastItem);
                LastItem = null;
            }
        }
    }
}