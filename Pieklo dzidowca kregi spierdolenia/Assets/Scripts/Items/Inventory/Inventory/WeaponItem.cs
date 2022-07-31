using jbzdy.Items.Enums;
using UnityEngine;

/// <summary>
/// Klasa pomocnicza do tworzenia przedmiotów typu weapon
/// 
/// Napisane przez Sharashino
/// </summary>
namespace jbzdy.Items
{
    public class WeaponItem : Item
    {
        public WeaponTypes weaponType;
        public Sprite weaponInventorySprite;
        public int weaponDamage;
        public int weaponLevel;
    }
}
