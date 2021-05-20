using System;
using UnityEngine;

// <summary>
// Napisane przez Sharashino
//
// Bazowa klasa przechowująca dane o pojedynczym dropie
// <summary>
namespace jbzdy.Items.Drop
{
    [Serializable]
    public class ItemDropBase
    {
        [SerializeField] private GameObject itemToDrop = default;
        [SerializeField] private float dropChance = default;

        public GameObject ItemToDrop
        {
            get => itemToDrop;
            set => itemToDrop = value;
        }

        public float ItemDropChance
        {
            get => dropChance;
            set => dropChance = value;
        }
    }
}
