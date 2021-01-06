using UnityEngine;

/// <summary>
/// Napisane przez Sharashino
/// 
/// Skrypt definiujący statystyki postaci 
/// </summary>
namespace jbzdy.CharacterStats
{
    public class CharacterStats : MonoBehaviour
    {
        public static CharacterStats instance;
        private void Awake()
        {
            instance = this;
        }

        public int MaxHealth { get; private set; }
        [SerializeField] private Stat Health;
        [SerializeField] private Stat Mana;
        [SerializeField] private Stat Armor;
        [SerializeField] private Stat Strenght;
        [SerializeField] private Stat Agility;
        [SerializeField] private Stat Intelligence;
        [SerializeField] private Stat Vitality;
        [SerializeField] private Stat Luck;

        private void Start()
        {
            MaxHealth = Health.GetBaseValue();
        }

        public void Heal(int health)
        {
            if(MaxHealth + health > Health.GetBaseValue())
            {
                Health.AddModifier(5);
            }
            else
            {
                MaxHealth += health;
            }
        }

        public void TakeDamage(int damage)
        {
            //logic for damage reduction goes here
            damage -= Armor.GetBaseValue();
            damage = Mathf.Clamp(damage, 0, int.MaxValue);

            MaxHealth -= damage;

            Debug.Log(transform.name + " takes " + damage + " damage");

            if (MaxHealth <= 0)
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



