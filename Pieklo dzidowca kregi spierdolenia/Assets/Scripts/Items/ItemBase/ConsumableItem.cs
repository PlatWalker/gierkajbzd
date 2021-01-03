using UnityEngine;

/// <summary>
/// Made by sharashino 
/// </summary>
[CreateAssetMenu(fileName = "NewConsumableItem", menuName = "Items/New Consumable Item")]
public class ConsumableItem : Item
{
    public int healthModifier;
    public int manaModifier;
}
