using UnityEngine;
using jbzdy.Items.Enums;
using UnityEngine.Events;

/// <summary>
/// Klasa główna do tworzenia przedmiotów
/// 
/// Napisane przez Sharashino
/// </summary>
namespace jbzdy.Items
{
    public class Item : MonoBehaviour
    {
        [System.Serializable]
        public class OnUseEvent : UnityEvent { }
        [System.Serializable]
        public class OnPickupEvent : UnityEvent { }

        public int itemID;
        public string itemName;
        public string itemDescription;
        public Sprite itemIcon;
        public ItemTypes itemType;

        [Range(1, 10)]
        public int itemWidth = 1, itemHeight = 1;

        [Header("Stack options")]
        public bool isStackable;

        [Range(1, 100)]
        public int itemMaxStackSize = 1;

        [Range(1, 100)]
        public int itemStackSize = 1;
        
        [SerializeField]
        public OnUseEvent onUseEvent;
        [SerializeField]
        public OnPickupEvent onPickupEvent;
    }
}
