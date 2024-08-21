using System;
using System.Collections.Generic;
using UnityEngine;

namespace jbzd.Common.Random
{
    [Serializable]
    public class NewDict<T1, T2, T3>
    {
        [SerializeField] NewDictItem<T1, T2, T3>[] thisDictItems;

        public Dictionary<T1, T2> ToDictionary()
        {
            Dictionary<T1, T2> newDict = new();
            foreach (var item in thisDictItems)
            {
                newDict.Add(item.Key, item.Value);
            }
            return newDict;
        }

        public Dictionary<T1, (T2, T3)> ToBiggerDictionary()
        {
            Dictionary<T1, (T2, T3)> newDict = new();
            foreach (var item in thisDictItems)
            {
                newDict.Add(item.Key, (item.Value, item.Value2));
            }
            return newDict;
        }
    }

    [Serializable]
    public class NewDictItem<T1, T2, T3>
    {
        [SerializeField] public T1 Key;
        [SerializeField] public T2 Value;
        [SerializeField] public T3 Value2;
    }
}
