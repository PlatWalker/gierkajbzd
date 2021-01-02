using UnityEngine;


[CreateAssetMenu(fileName = "NewConsumableItem", menuName = "Items/New Consumable Item")]
public class ConsumableItem : Item
{
    public bool canConsume;

    public int healthModifier;
    public int manaModifier;
}
