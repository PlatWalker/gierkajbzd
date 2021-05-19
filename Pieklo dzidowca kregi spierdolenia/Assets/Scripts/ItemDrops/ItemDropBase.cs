using System.Collections;
using System.Collections.Generic;
using jbzdy.Items;
using UnityEngine;

[System.Serializable]
public class ItemDropBase
{
    [SerializeField] private GameObject itemToDrop = default;
    [SerializeField] private float dropChance = default;

    public GameObject ItemToDrop { get => itemToDrop; set => itemToDrop = value; }
    public float ItemDropChance { get => dropChance; set => dropChance = value; }
}
