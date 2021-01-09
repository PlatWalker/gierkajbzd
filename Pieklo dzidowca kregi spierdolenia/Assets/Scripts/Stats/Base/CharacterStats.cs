using UnityEngine;
using System.Collections.Generic;
using System;

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

        [SerializeField] private int ExperiencePoints;
        [SerializeField] private int Level;
        [SerializeField] private int MaxHealth;
        [SerializeField] private Stat Health;
        [SerializeField] private Stat Mana;
        [SerializeField] private Stat Armor;
        [SerializeField] private Stat Damage;
        [SerializeField] private Stat Strenght;
        [SerializeField] private Stat Agility;
        [SerializeField] private Stat Intelligence;
        [SerializeField] private Stat Vitality;
        [SerializeField] private Stat Luck;

        public int toNextLevel;

        private void Start()
        {
            MaxHealth = Health.GetBaseValue();
        }


        #region Getters & Setters

        public int GetMaxHealth()
        {
            return MaxHealth;
        }
        public Stat ReturnHealth()
        {
            return Health;
        }
        public Stat ReturnMana()
        {
            return Mana;
        }
        public Stat ReturnArmor()
        {
            return Armor;
        }
        public Stat ReturnDamage()
        {
            return Damage;
        }
        public Stat ReturnStrenght()
        {
            return Strenght;
        }
        public Stat ReturnAgility()
        {
            return Agility;
        }
        public Stat ReturnIntelligence()
        {
            return Intelligence;
        }
        public Stat ReturnVitality()
        {
            return Vitality;
        }

        public Stat ReturnLuck()
        {
            return Luck;
        }

        #endregion


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

        public void AddXP(int xp)
        {
            ExperiencePoints += xp;

            if(ExperiencePoints >= toNextLevel)
            {
                Level++;
                LevelUp();
            }
        }

        public void LevelUp()
        {
            toNextLevel *= 2;
            Debug.Log("Level up!");
            Debug.Log("To next level: " + toNextLevel);
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



