using System;
using System.Collections.Generic;
using jbzd.Common.Interfaces;
using jbzdy.CharacterStats;
using jbzdy.CharacterStats.Stats;
using UnityEngine;

namespace jbzd.MainHero.LegacyHeroThings.Stats
{
    [Obsolete("Skrypt sharashino ...")]
    public class PlayerStats : CharacterStats , IDamageable
    {
        //[SerializeField] private ExperienceManager experienceManager;

        [HideInInspector] public List<Stat> modifiableStatsList = new List<Stat>();

        private void Start()
        {
            AddModifiableStats();
        }

        private void Update()
        {
            TestingCheats();
        }

        private void TestingCheats()
        {
            /*
            if (Input.GetKeyDown(KeyCode.T))
            {
                TakeDamage(5);
            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                Heal(5);
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                experienceManager.AddXP(10);
            }

            if (Input.GetKeyDown(KeyCode.P))
            {

                experienceManager.LevelUp();
            }
            */
        }

        private void AddModifiableStats()
        {
            modifiableStatsList.Add(Strength);
            modifiableStatsList.Add(Agility);
            modifiableStatsList.Add(Intelligence);
            modifiableStatsList.Add(Vitality);
            modifiableStatsList.Add(Luck);
        }

        [field:SerializeField]
        public int MaximumHealth { get; private set; }
        [field:SerializeField]
        public int CurrentHealth { get; private set; }
        public void SetDamage(int damageAmount, DamageType damageType)
        {
            Health.BaseValue -= damageAmount;
        }

        public void SetDamage(int damageAmount, DamageType damageType, float criticalMultiplier, float criticalChance)
        {
            if (UnityEngine.Random.Range(0.0f, 1.0f) <= criticalChance)
            {
                damageAmount = (int)(damageAmount * criticalMultiplier);
            }

            SetDamage(damageAmount, damageType);
        }
    }
}

