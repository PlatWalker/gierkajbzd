using UnityEngine;

/// <summary>
/// Napisane przez sharashino 
/// </summary>
[CreateAssetMenu(fileName = "NewConsumableItem", menuName = "Items/New Consumable Item")]
public class ConsumableItem : Item
{
    public int healthModifier;
    public int manaModifier;

    public override void Use()
    {
        base.Use();

        //Using the consumable item
        //
        //Mocht ik onder het hakkuh bezwijken
        
        Debug.Log("Consuming " + itemName);
    }
}



