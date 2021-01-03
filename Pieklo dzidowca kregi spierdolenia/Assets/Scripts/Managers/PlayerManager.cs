using UnityEngine;
using jbzdy.CharacterStats;

/// <summary>
/// Made by sharashino
/// </summary>
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    private void Awake()
    {
        instance = this;
    }

    [SerializeField] private PlayerStats playerStats;

    public void UseItem()
    {
        //for using items
    }

    public void ConsumeItem(ConsumableItem itemToConsume)
    {
        //for consuming items
        playerStats.SetHealth(itemToConsume.healthModifier);
    }

    public void EquipItem()
    {
        //for equiping items
    }
}
