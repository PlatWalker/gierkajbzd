using UnityEngine;

// <summary>
// Napisane przez Sharashino
//
// Klasa tworząca grupę dropów jako ScriptableObject
// <summary>
namespace jbzdy.Items.Drop
{
    public class ItemDrop : ScriptableObject
    {
        [SerializeField] public ItemDropBase[] itemDropBases;
    }
}

