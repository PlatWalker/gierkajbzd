using UnityEngine;

/// <summary>
/// napisane przez Sharashino
/// 
/// Skrypt ze statystykami dla gracza
/// </summary>
namespace jbzdy.CharacterStats
{
    public class PlayerStats : CharacterStats
    {
        public void ConsumeItem(ConsumableItem itemToConsume)
        {
            Heal(itemToConsume.healthModifier);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                TakeDamage(5);
            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                Heal(5);
            }
        }
    }
}

