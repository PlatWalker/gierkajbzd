using UnityEngine;
using jbzdy.Items;
using jbzdy.StatCreation;
using System.Collections.Generic;
using jbzdy.CharacterStats.Stats;

/// <summary>
/// Napisane przez Sharashino
/// 
/// Skrypt ze statystykami dla gracza
/// </summary>
namespace jbzdy.CharacterStats
{
    public class PlayerStats : CharacterStats
    {
        [SerializeField] private ExperienceManager experienceManager;

        [HideInInspector] public List<Stat> modifiableStatsList = new List<Stat>();

        private void Start()
        {
            EquipmentManager.instance.onEquipmentChange += OnEquipmentChanged;
            AddModifiableStats();
        }

        void OnEquipmentChanged (EquipableItem itemToEquip, EquipableItem oldItem)
        {
            if(itemToEquip != null)
            {
                Armor.AddModifier(itemToEquip.armorModifier);
                Damage.AddModifier(itemToEquip.damageModifier);
            }

            if (oldItem != null)
            {
                Armor.RemoveModifier(itemToEquip.armorModifier);
                Damage.RemoveModifier(itemToEquip.damageModifier);
            }
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

        public void AddModifiableStats()
        {
            modifiableStatsList.Add(Strenght);
            modifiableStatsList.Add(Agility);
            modifiableStatsList.Add(Intelligence);
            modifiableStatsList.Add(Vitality);
            modifiableStatsList.Add(Luck);
        }
    }
}

