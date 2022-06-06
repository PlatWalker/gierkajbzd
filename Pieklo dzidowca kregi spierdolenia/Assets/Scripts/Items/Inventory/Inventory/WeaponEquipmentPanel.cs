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
    public class WeaponEquipmentPanel : EquipmentPanel
    {
        public WeaponTypes allowedWeaponType;

        private void Update()
        {
            if (equipedItem != null && LastItem == null)
            {
                GameManager.Instance.PlayerObject.GetComponent<jbzdy.Items.ItemEquipper>().EquipWeaponOrTrinket(equipedItem);
                LastItem = equipedItem;
            }

            if (equipedItem == null && LastItem != null)
            {
                GameManager.Instance.PlayerObject.GetComponent<jbzdy.Items.ItemEquipper>().UnequipWeaponOrTrinket(LastItem);
                LastItem = null;
            }
        }
    }
}