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
        [SerializeField] private string _statName;
        [SerializeField] private int _baseValue;

        public List<int> modifiers = new List<int>();

        public string StatName
        {
            get
            {
                return _statName;
            }
        }

        public int BaseValue
        {
            get
            {
                int finalValue = _baseValue;
                modifiers.ForEach(x => finalValue += x);

                return finalValue;
            }
            set
            {
                _baseValue = value;
            }
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