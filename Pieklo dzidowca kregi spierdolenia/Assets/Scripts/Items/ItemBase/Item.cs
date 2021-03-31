using UnityEngine;

/// <summary>
/// Napisane przez Sharashino
/// </summary>
namespace jbzdy.Items
{
    [CreateAssetMenu(fileName = "NewItem", menuName = "Items/New Item")]
    public class Item1 : ScriptableObject
    {
        [SerializeField] private string itemName = "New Item";
        [SerializeField] private string itemDesc = "It must be awesome";
        [SerializeField] private Sprite itemIcon = null;
        [SerializeField] private GameObject itemObject;
        [SerializeField] private bool canPickUp;

        #region properties

        public string ItemName
        {
            get
            {
                return itemName;
            }
            set
            {
                itemName = value;
            }
        }
        public string ItemDesc
        {
            get
            {
                return itemDesc;
            }
            set
            {
                itemDesc = value;
            }
        }
        public Sprite ItemIcon
        {
            get
            {
                return itemIcon;
            }
            set
            {
                itemIcon = value;
            }
        }
        public GameObject ItemObject
        {
            get
            {
                return itemObject;
            }
            set
            {
                itemObject = value;
            }
        }
        public bool CanPickUp
        {
            get
            {
                return canPickUp;
            }
            set
            {
                canPickUp = value;
            }
        }

        #endregion

        public virtual void Use()
        {
            //Using the default Item
            //
            //ja tak jak kot siedem żyć i baluje całą noc

            Debug.Log("Using " + itemName);
        }

        public void RemoveFromInventory()
        {
           // Inventory.instance.RemoveItem(this);
        }
    }
}
