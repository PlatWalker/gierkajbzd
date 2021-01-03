using UnityEngine;

/// <summary>
/// Made by sharashino
/// </summary>
namespace jbzdy.CharacterStats
{
    public class PlayerStats : CharacterStats
    {
        public void ConsumeItem(ConsumableItem itemToConsume)
        {
            SetHealth(itemToConsume.healthModifier);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                TakeDamage(5);
            }
        }
    }
}

