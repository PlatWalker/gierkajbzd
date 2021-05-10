using UnityEngine;
using jbzdy.Items;
using jbzdy.Managers;
using System.Collections.Generic;
using jbzdy.CharacterStats.Stats;

// <summary>
// Napisane przez Sharashino
// 
// Skrypt ze statystykami dla gracza
// </summary>
namespace jbzdy.CharacterStats
{
    public class PlayerStats : CharacterStats
    {
        [SerializeField] private ExperienceManager experienceManager = default;

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
        }

        private void AddModifiableStats()
        {
            modifiableStatsList.Add(Strength);
            modifiableStatsList.Add(Agility);
            modifiableStatsList.Add(Intelligence);
            modifiableStatsList.Add(Vitality);
            modifiableStatsList.Add(Luck);
        }
    }
}

