using System.Collections;
using System.Collections.Generic;
using jbzdy.Items;
using UnityEngine;

[System.Serializable]
public class ItemDropBase
{
    [SerializeField] private Item itemToDrop = default;
    [SerializeField] private float itemDropChance = default;
    
    public Item ItemToDrop { get => itemToDrop; set => itemToDrop = value; }
    public float ItemDropChance { get => itemDropChance; set => itemDropChance = value; }
}
