using jbzd.Items;
using jbzd.MainHero.PlayerControllers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace jbzd.UI.Inventory
{
    public class InventorySlotUI : MonoBehaviour, IDropHandler
    {
        public GameObject _inventoryItemPrefab;
        public InventoryItemUI itemInfo;
        private InventorySlot _inventorySlot;

        public InventoryController _inventoryController;
        public bool _isEquippable;

        public void ShowSlot(InventorySlot inventorySlot)
        {
            _inventorySlot = inventorySlot;
            _isEquippable = false;
            PrepareSlot(_inventorySlot.ItemData, _inventorySlot.StackSize);
        }

        public void ShowSlot(ItemSO item, InventoryController controller)
        {
            _isEquippable = true;
            _inventoryController = controller;
            PrepareSlot(item, -1);
        }

        private void PrepareSlot(ItemSO item, int stackSize)
        {
            if (transform.childCount > 0)
            {
                if (itemInfo != null)
                {
                    itemInfo.onSlotChanged -= RemoveItem;
                }

                Destroy(transform.GetChild(0).gameObject);

            }

            if (item != null)
            {
                GameObject inventoryItemGO = Instantiate(_inventoryItemPrefab, transform);
                itemInfo = inventoryItemGO.GetComponent<InventoryItemUI>();
                itemInfo.ShowItem(item, stackSize);
                itemInfo.onSlotChanged += RemoveItem;
            }
        }

        public void RemoveItem()
        {
            itemInfo.onSlotChanged -= RemoveItem;
            if(_isEquippable)
                _inventoryController.UnequipItem(itemInfo.item);
            else
                _inventorySlot.ClearSlot();
            itemInfo = null;
        }

        public void OnDrop(PointerEventData eventData)
        {
            if(transform.childCount == 0)
            {
                itemInfo = eventData.pointerDrag.GetComponent<InventoryItemUI>();

                if (_isEquippable)
                {
                    if (!_inventoryController.EquipItem(itemInfo.item))
                        return;
                }
                else
                    _inventorySlot.UpdateInventorySlot(itemInfo.item, itemInfo.stackSize);

                itemInfo.ChangeParent(transform);
            }
        }
       
    }
}
