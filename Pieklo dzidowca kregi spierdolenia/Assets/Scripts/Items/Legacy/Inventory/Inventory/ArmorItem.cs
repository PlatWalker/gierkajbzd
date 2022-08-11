using jbzdy.Items.Enums;
using UnityEngine;

/// <summary>
/// Klasa pomocnicza do tworzenia przedmiotów typu armor
/// 
/// Napisane przez Sharashino
/// </summary>
namespace jbzdy.Items
{
    public class ArmorItem : SharItem
    {
        public ArmorTypes armorType;
        public Sprite armorInventorySprite;
        public int armorValue;
        public int armorLevel;
    }
}
