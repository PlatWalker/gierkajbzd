using UnityEngine;
using jbzdy.CharacterStats;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private CharacterStats Strenght;
    [SerializeField] private CharacterStats Agility;
    [SerializeField] private CharacterStats Intelligence;
    [SerializeField] private CharacterStats Vitality;
    [SerializeField] private CharacterStats Luck;
}

public class ItemInteraction
{
    public void EquipItem(PlayerStats playerStats, ConsumableItem itemToEquip)
    {

    }

    public void UseItem(PlayerStats playerStats, ConsumableItem itemToUse)
    {
        itemToUse.ConsumeItem();
    }
}