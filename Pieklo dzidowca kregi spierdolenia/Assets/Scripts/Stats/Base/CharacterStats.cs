using UnityEngine;

/// <summary>
/// Made by Sharashino
/// </summary>
namespace jbzdy.CharacterStats
{
    public class CharacterStats : MonoBehaviour
    {
        public int currentHealth { get; private set; }

        [SerializeField] private Stat Health;
        [SerializeField] private Stat Mana;
        [SerializeField] private Stat Armor;
        [SerializeField] private Stat Strenght;
        [SerializeField] private Stat Agility;
        [SerializeField] private Stat Intelligence;
        [SerializeField] private Stat Vitality;
        [SerializeField] private Stat Luck;

        public void TakeDamage(int damage)
        {
            damage -= Armor.GetBaseValue();
            currentHealth -= damage;

            Debug.Log(transform.name + " takes " + damage + " damage");

            if (currentHealth <= 0)
            {
                CharacterDie();
            }
        }

        private void CharacterDie()
        {
            //Die lol
            //Overwritten
            Debug.Log(transform.name + " died");
        }
    }
}



