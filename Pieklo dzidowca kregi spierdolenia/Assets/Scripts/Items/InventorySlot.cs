using jbzd.Items;
using UnityEngine;

namespace jbzd.Items
{
    [System.Serializable]
    public class InventorySlot
    {
        [SerializeField] private ItemSO itemData;
        [SerializeField] private int stackSize;

        public ItemSO ItemData => itemData;
        public int StackSize => stackSize;

        public InventorySlot(ItemSO source, int amount) 
        {
            itemData = source;
            stackSize = amount;
        }

        public InventorySlot() 
        { 
            ClearSlot();
        }

        public void UpdateInventorySlot(ItemSO item, int amount)
        {
            itemData = item;
            stackSize = amount;
        }

        public void ClearSlot()
        {
            itemData = null;
            stackSize = -1;
        }

        public bool RoomLeftInStack(int amountToAdd, out int amountRemaing)
        {
            amountRemaing = itemData.MaxStackSize - stackSize;
            return RoomLeftInStack(amountToAdd);
        }

        public bool RoomLeftInStack(int amountToAdd)
        {
            if (stackSize + amountToAdd <= ItemData.MaxStackSize) return true;            
            return false;
        }

        public void AddToStack(int amount)
        {
            stackSize += amount;
        }

        public int RemoveFromStack(int amount)
        {
            stackSize -= amount;
            if (stackSize <= 0)
            {
                ClearSlot();
                return -stackSize;
            }
            return 0;
        }

        public void AssignItem(InventorySlot slot)
        {
            if (itemData == slot.ItemData) AddToStack(slot.stackSize);
            itemData = slot.ItemData;
            stackSize = 0;
            AddToStack(slot.stackSize);
        }

        public bool SplitStack(out InventorySlot splitStack)
        {
            if( stackSize <= 1)
            {
                splitStack = null;
                return false;
            }

            int halfStack = Mathf.FloorToInt(stackSize / 2);
            RemoveFromStack(halfStack);

            splitStack = new InventorySlot(ItemData, halfStack);
            return true;
        }
    }
}
