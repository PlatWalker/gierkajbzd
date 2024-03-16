using System.Collections.Generic;
using jbzd.Common;
using jbzd.Items;
using jbzd.MainHero.PlayerControllers;
using UnityEngine;

namespace jbzd.UI.Inventory
{
    public class InventorySlotUI : MonoBehaviour
    {
        [field:SerializeField]
        public bool IsEquippable { get; set; }
        
        [field:JbzdReadOnly]
        [field:SerializeField]
        public InventoryItemUI itemInfo;
        
        [Tooltip("Works only if inventory slot IsEquippable is marked as true")]
        [SerializeField] 
        private ItemTypes allowedItemType;
        
        private InventorySlot _inventorySlot;
        private KeyValuePair<ItemTypes, ItemSO>? _equippedItem;
        
        private InventoryController _inventoryController;
        private GameObject _inventoryItemPrefab;
        private GameObject _draggedItemHolder;

        public void Init(InventorySlot inventorySlot, GameObject inventoryItemPrefab, InventoryController inventoryController, GameObject draggedItemHolder)
        {
            _equippedItem = null;
            _inventorySlot = inventorySlot;
            
            _draggedItemHolder = draggedItemHolder;
            _inventoryItemPrefab = inventoryItemPrefab;
            _inventoryController = inventoryController;
        }

        public void Init(KeyValuePair<ItemTypes, ItemSO> equippedItem, GameObject inventoryItemPrefab, InventoryController inventoryController, GameObject draggedItemHolder)
        {
            _inventorySlot = null;
            _equippedItem = equippedItem;
            
            _draggedItemHolder = draggedItemHolder;
            _inventoryItemPrefab = inventoryItemPrefab;
            _inventoryController = inventoryController;
        }

        public void UpdateContentOfSlot(InventorySlot newSlot = null)
        {
            if (transform.childCount > 0)
            {
                Destroy(transform.GetChild(0).gameObject);
            }

            if (IsEquippable && _equippedItem?.Value is null)
            {
                return;
            }

            if (newSlot is not null)
            {
                _inventorySlot = newSlot;
            }
            
            if(_inventorySlot?.ItemData is null && _equippedItem is null)
            {
                return;
            }

            var inventoryItemGO = Instantiate(_inventoryItemPrefab, transform);
            itemInfo = inventoryItemGO.GetComponent<InventoryItemUI>();

            if (IsEquippable)
            {
                if (_equippedItem is null)
                {
                    Debug.LogError($"{_equippedItem} value is null, but it shouldn't");
                    return;
                }
                
                itemInfo.Init(_equippedItem.Value.Value, -1, _draggedItemHolder);
            }
            else
            {
                itemInfo.Init(_inventorySlot.ItemData, _inventorySlot.StackSize, _draggedItemHolder);
            }
            
        }

        public void RemoveItem()
        {
            if(IsEquippable)
                _inventoryController.UnequipItem(itemInfo.Item);
            else
                _inventorySlot.ClearSlot();
            itemInfo = null;
        }

        public bool TryAssignToSlot(InventoryItemUI inventoryItemUI)
        {
            if (IsEquippable && inventoryItemUI.Item.ItemType != allowedItemType)
            {
                Debug.Log("Wrong type of item is assigned");
                return false;
            }
            
            itemInfo = inventoryItemUI;
            
            if (IsEquippable)
            {
                if (!_inventoryController.EquipItem(itemInfo.Item))
                {
                    return false;
                }
            }
            else
            {
                _inventorySlot.UpdateInventorySlot(itemInfo.Item, itemInfo.StackSize);
            }

            return true;
        }
    }
}
