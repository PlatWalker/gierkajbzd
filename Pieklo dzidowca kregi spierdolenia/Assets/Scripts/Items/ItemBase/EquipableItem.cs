using UnityEngine;

/// <summary>
/// Napisane przez sharashino 
/// </summary>
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

public enum EquipmentSlot
{
    Head,
    Chest,
    Legs,
    Feet,
    Weapon,
    Shield,
}
