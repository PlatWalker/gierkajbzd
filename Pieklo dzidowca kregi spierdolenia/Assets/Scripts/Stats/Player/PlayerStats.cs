using UnityEngine;
using System.Collections.Generic;
using jbzdy.StatCreation;
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


        public List<Stat> modifiableStatsList = new List<Stat>();

        private void Start()
        {
            EquipmentManager.instance.onEquipmentChange += OnEquipmentChanged;
        }

        void OnEquipmentChanged (EquipableItem itemToEquip, EquipableItem oldItem)
        {
            if(itemToEquip != null)
            {
                ReturnArmor().AddModifier(itemToEquip.armorModifier);
                ReturnDamage().AddModifier(itemToEquip.damageModifier);
            }

            if (oldItem != null)
            {
                ReturnArmor().RemoveModifier(itemToEquip.armorModifier);
                ReturnDamage().RemoveModifier(itemToEquip.damageModifier);
            }
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

            if (Input.GetKeyDown(KeyCode.X))
            {
                AddXP(10);
            }

            if(Input.GetKeyDown(KeyCode.P))
            {
                AddModifiableStats();
                experienceManager.LevelUp();
            }
        }


        public void AddModifiableStats()
        {
            modifiableStatsList.Add(ReturnStrenght());
            modifiableStatsList.Add(ReturnAgility());
            modifiableStatsList.Add(ReturnIntelligence());
            modifiableStatsList.Add(ReturnVitality());
            modifiableStatsList.Add(ReturnLuck());
        }
    }
}

