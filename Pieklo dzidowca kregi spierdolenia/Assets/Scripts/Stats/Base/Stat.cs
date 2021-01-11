using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Napisane przez Sharashino
/// 
/// Skrypt definiujący czym jest statystyka i umożliwiającym operacje na tej wartości
/// </summary>
namespace jbzdy.CharacterStats
{
    [System.Serializable]
    public class Stat
    {
        public string statName;
        [SerializeField] private int baseValue;

        public List<int> modifiers = new List<int>();

        public int GetBaseValue()
        {
            int finalValue = baseValue;
            modifiers.ForEach(x => finalValue += x);

            return finalValue;
        }

        public int SetBaseValue(int value)
        {
            return baseValue = value;
        }

        public void AddModifier(int modifier)
        {
            if(modifier != 0)
            {
                modifiers.Add(modifier);
            }
        }

        public void RemoveModifier(int modifier)
        {
            if (modifier != 0)
            {
                modifiers.Remove(modifier);
            }
        }
    }
}