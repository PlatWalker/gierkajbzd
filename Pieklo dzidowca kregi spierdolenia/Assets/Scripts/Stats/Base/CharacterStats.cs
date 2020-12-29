using System.Collections.ObjectModel;
using System.Collections.Generic;
using System;

/// <summary>
/// Made by Sharashino
/// </summary>
namespace jbzdy.CharacterStats
{
    [Serializable]
    public class CharacterStats
    {
        public float baseValue;

        public virtual float Value
        {
            get
            {
                if (isDirty || baseValue != lastBaseValue)
                {
                    lastBaseValue = baseValue;
                    value = CalculateFinalValue();
                    isDirty = false;
                }
                return value;
            }
        }

        protected bool isDirty = true;
        protected float value;
        protected float lastBaseValue = float.MinValue;
        protected readonly List<Stats> statModifiers;
        public readonly ReadOnlyCollection<Stats> StatModifiers;

        public CharacterStats()
        {
            statModifiers = new List<Stats>();
            StatModifiers = statModifiers.AsReadOnly();
        }

        public CharacterStats(float baseValue) : this()
        {
            this.baseValue = baseValue;
        }

        public virtual void AddModifier(Stats stat)
        {
            isDirty = true;
            statModifiers.Add(stat);
            statModifiers.Sort(CompareModifierOrders);
        }

        public virtual bool RemoveModifier(Stats stat)
        {
            if (statModifiers.Remove(stat))
            {
                isDirty = true;
                return true;
            }

            return false;
        }

        public virtual bool RemoveAllModifiersFromSource(object source)
        {
            bool didRemove = false;

            for (int i = statModifiers.Count - 1; i >= 0; i--)
            {
                if (statModifiers[i].Source == source)
                {
                    isDirty = true;
                    didRemove = true;
                    statModifiers.RemoveAt(i);
                }
            }
            return didRemove;
        }

        protected virtual int CompareModifierOrders(Stats a, Stats b)
        {
            if (a.ReadOrder < b.ReadOrder)
            {
                return -1;
            }
            else if (a.ReadOrder > b.ReadOrder)
            {
                return 1;
            }
            return 0; //if(a.ReadOrder == b.ReadOrder)
        }

        protected virtual float CalculateFinalValue()
        {
            float finalValue = baseValue;
            float sumPercentAdd = 0;

            for (int i = 0; i < statModifiers.Count; i++)
            {
                Stats modifier = statModifiers[i];
                if (modifier.Type == StatModType.Flat)
                {
                    finalValue += modifier.Value;
                }
                else if (modifier.Type == StatModType.PercentAdd)
                {
                    sumPercentAdd += modifier.Value;

                    if (i + 1 >= statModifiers.Count || statModifiers[i + 1].Type != StatModType.PercentAdd)
                    {
                        finalValue *= 1 + sumPercentAdd;
                        sumPercentAdd = 0;
                    }
                }
                else if (modifier.Type == StatModType.PercentMult)
                {
                    finalValue *= 1 + modifier.Value;
                }
            }

            return (float)Math.Round(finalValue, 4);
        }
    }
}



