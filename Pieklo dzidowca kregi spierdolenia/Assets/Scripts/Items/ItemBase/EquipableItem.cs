using UnityEngine;
using jbzdy.Managers;
using jbzdy.Items.Enums;

/// <summary>
/// Napisane przez sharashino 
/// 
/// Przedmioty które możemy ubrać
/// </summary>
namespace jbzdy.Items
{
    [CreateAssetMenu(fileName = "NewEquipableItem", menuName = "Items/New Equipable Item")]
    public class EquipableItem : Item
    {
        public EquipmentSlot equipmentSlot;
        public int armorModifier;
        public int damageModifier;

        public override void Use()
        {
            base.Use();

            //Equip item
            //
            //
            //Remove from inventory
            EquipmentManager.instance.EquipItem(this);
            RemoveFromInventory();
        }
    }
}

