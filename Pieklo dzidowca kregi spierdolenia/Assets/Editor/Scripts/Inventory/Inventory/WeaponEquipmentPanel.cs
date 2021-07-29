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
                LastItem = equipedItem;
            }

            if (equipedItem == null && LastItem != null)
            {
                LastItem = null;
            }
        }
    }
}