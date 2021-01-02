using UnityEngine;

namespace jbzdy.CharacterStats
{
    [System.Serializable]
    public class Stat
    {
        [SerializeField] private int baseValue;

        public int GetBaseValue()
        {
            return baseValue;
        }
    }
}