using UnityEngine;

/// <summary>
/// Made by Sharashino
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

        public int CurrentHealth { get; private set; }
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
            CurrentHealth = Health.GetBaseValue();
        }

        public void SetHealth(int health)
        {
            if(CurrentHealth + health > Health.GetBaseValue())
            {
                return;
            }
            else
            {
                CurrentHealth += health;
            }
        }

        public void TakeDamage(int damage)
        {
            //logic for damage reduction goes here
            damage -= Armor.GetBaseValue();
            damage = Mathf.Clamp(damage, 0, int.MaxValue);

            CurrentHealth -= damage;

            Debug.Log(transform.name + " takes " + damage + " damage");

            if (CurrentHealth <= 0)
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



