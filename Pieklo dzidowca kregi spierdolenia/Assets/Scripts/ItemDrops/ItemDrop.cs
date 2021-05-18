using System.Collections;
using System.Collections.Generic;
using jbzdy.Items;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Drop", fileName = "New Drop")]
public class ItemDrop : ScriptableObject
{
    public ItemDropBase[] itemDropBases;
}

