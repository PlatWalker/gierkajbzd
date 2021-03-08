using UnityEngine;
using jbzdy.CharacterStats.Stats;
/// <summary>
/// Napisane przez Sharashino
/// 
/// Skrypt definiujący statystyki postaci 
/// </summary>
namespace jbzdy.CharacterStats
{
    public class CharacterStats : MonoBehaviour
    {
        #region singleton

        public static CharacterStats instance;
        private void Awake()
        {
            instance = this;

        }
        #endregion

        [Header("Experience stats")]
        [SerializeField] private int _ExperiencePoints;
        [SerializeField] private int _Level;

        [Header("Soft stats")]
        [SerializeField] private int _MaxHealth;
        [SerializeField] private Stat _Health;
        [SerializeField] private Stat _Mana;
        [SerializeField] protected Stat _Armor;
        [SerializeField] protected Stat _Damage;

        [Header("Hard stats")]
        [SerializeField] private Stat _Strenght;
        [SerializeField] private Stat _Agility;
        [SerializeField] private Stat _Intelligence;
        [SerializeField] private Stat _Vitality;
        [SerializeField] private Stat _Luck;

        private void Start()
        {
            MaxHealth = Health.BaseValue;
        }

        #region Getters & Setters

        public int ExperiencePoints
        {
            get
            {
                return _ExperiencePoints;
            }
            set
            {
                _ExperiencePoints = value;
            }
        }
        public int Level
        {
            get
            {
                return _Level;
            }
            private set
            {
                _Level = value;
            }
        }
        public int MaxHealth 
        {
            get
            {
                return _MaxHealth;
            }
            set 
            {
                _MaxHealth = value;
            } 
        }
        public Stat Health
        {
            get
            {
                return _Health;
            }
            set
            {
                _Health = value;
            }
        }
        public Stat Mana
        {
            get
            {
                return _Mana;
            }
            set
            {
                _Mana = value;
            }
        }
        public Stat Armor
        {
            get
            {
                return _Armor;
            }
            set
            {
                _Armor = value;
            }
        }
        public Stat Damage
        {
            get
            {
                return _Damage;
            }
            set
            {
                _Damage = value;
            }
        }
        public Stat Strenght
        {
            get
            {
                return _Strenght;
            }
            set
            {
                _Strenght = value;
            }
        }
        public Stat Agility
        {
            get
            {
                return _Agility;
            }
            set
            {
                _Agility = value;
            }
        }
        public Stat Intelligence
        {
            get
            {
                return _Intelligence;
            }
            set
            {
                _Intelligence = value;
            }
        }
        public Stat Vitality
        {
            get
            {
                return _Vitality;
            }
            set
            {
                _Vitality = value;
            }
        }
        public Stat Luck
        {
            get
            {
                return _Luck;
            }
            set
            {
                _Luck = value;
            }
        }
      
        public void AddToLevel(int value)
        {
            _Level += value;
        }
        public void SetExperiencePoints(int value)
        {
            _ExperiencePoints += value;
        }

        #endregion


        public void Heal(int health)
        {
            if(MaxHealth + health > Health.BaseValue)
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
            damage -= Armor.BaseValue;
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



