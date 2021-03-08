using UnityEngine;

/// <summary>
/// Napisane przez sharashino 
/// 
/// Przedmioty które możemy skonsumować
/// </summary>
namespace jbzdy.Items
{
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

            Debug.Log("Consuming " + ItemName);
        }
    }
}