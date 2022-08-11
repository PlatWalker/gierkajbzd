using jbzdy.Items;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using jbzdy.Inventory.SaveLoad;
using System.Collections.Generic;
using jbzd;

// <summary>
// Główna klasa Inventory odpowiadająca za całą funkcjonalność, spawnowanie itemów i grida
// 
// Napisane przez Sharashino
// </summary>
namespace jbzdy.Inventory
{
    [System.Serializable] public class OnInventoryItemAdd : UnityEvent { }
    [System.Serializable] public class OnInventoryItemDrop : UnityEvent { }
    [System.Serializable] public class OnInventoryItemRemove : UnityEvent { }
    
    public class InventoryClass : MonoBehaviour
    {
        public static InventoryClass Instance;

        public OnInventoryItemAdd OnInventoryItemAdd;
        public OnInventoryItemDrop OnInventoryItemDrop;
        public OnInventoryItemRemove OnInventoryItemRemove;

        public List<InventoryItem> inventoryItems = new List<InventoryItem>();
        public List<EquipmentPanel> equipmentPanels;
        public bool dragItemOutsideToDrop = false;
        public Transform player;
        
        [HideInInspector] public Transform levelPoint;
        public float distanceAwayFromPlayerToDrop = 1f;
        public bool autoEquipItems = true;

        #region utility
        public List<GridSlot> slots;
        [HideInInspector] public Image cell;
        [HideInInspector] public int cellSize = 70;
        [HideInInspector] public int padding;
        [HideInInspector] public int column = 5;
        [HideInInspector] public int row = 4;
        [HideInInspector] public int lootColumn = 4;
        [HideInInspector] public int lootRow = 4;
        [HideInInspector] public Color normalCellColor;
        [HideInInspector] public Color hoveredCellColor;
        [HideInInspector] public Color blockedCellColor;
        [HideInInspector] public RectTransform lootPanel;
        [HideInInspector] InventoryManager inventoryManager;

        public void DrawPreview()
        {
            ClearPreview();

            //Inventory grid preview initialization
            for (int i = 0; i < column; i++)
            {
                for (int j = 0; j < row; j++)
                {
                    var _cell = Instantiate(cell);
                    _cell.rectTransform.SetParent(transform);
                    _cell.rectTransform.sizeDelta = new Vector2(cellSize - padding, cellSize - padding);
                    _cell.rectTransform.anchoredPosition = new Vector2(((cellSize * i) + padding), ((-cellSize * j) + padding));
                    _cell.rectTransform.localScale = new Vector2(1, 1);

                    _cell.name = i + "," + j;

                    _cell.color = normalCellColor;
                }
            }

            //Loot inventory grid preview initialization
            if (lootPanel != null)
            {
                for (int i = 0; i < lootRow; i++)
                {
                    for (int j = 0; j < lootColumn; j++)
                    {
                        var _cell = Instantiate(cell);
                        _cell.rectTransform.SetParent(lootPanel);
                        _cell.rectTransform.sizeDelta = new Vector2(cellSize - padding, cellSize - padding);
                        _cell.rectTransform.anchoredPosition = new Vector2(((cellSize * i) + padding), ((-cellSize * j) + padding));
                        _cell.rectTransform.localScale = new Vector2(1, 1);

                        _cell.name = i + "," + j;

                        _cell.color = normalCellColor;
                    }
                }
            }

            //Equipment grid preview initialization
            if (equipmentPanels != null)
            {
                for (int k = 0; k < equipmentPanels.Count; k++)
                {
                    for (int i = 0; i < equipmentPanels[k].width; i++)
                    {
                        for (int j = 0; j < equipmentPanels[k].height; j++)
                        {

                            var _cell = Instantiate(cell);
                            _cell.rectTransform.SetParent(equipmentPanels[k].transform);
                            _cell.rectTransform.sizeDelta = new Vector2(cellSize - padding, cellSize - padding);
                            _cell.rectTransform.anchoredPosition = new Vector2(((cellSize * i) + padding), ((-cellSize * j) + padding));
                            _cell.rectTransform.localScale = new Vector2(1, 1);

                            _cell.name = i + (k + 1) * 100 + "," + j + (k + 1) * 100;

                            _cell.color = normalCellColor;
                        }
                    }
                }
            }

            transform.SetAsLastSibling();
        }

        #endregion

        void Awake()
        {
            GetComponentInParent<Canvas>().enabled = true;

            Initialize();

            if (Instance == null)
            {
                Instance = this;
            }
        }

        // Destroy each slot before making of new end configuration
        public void ClearPreview()
        {
            if (GetComponents<GridSlot>() != null)
            {
                foreach (var previewSlot in FindObjectsOfType<GridSlot>())
                {
                    if(previewSlot.gameObject.name != "Utility object")
                        DestroyImmediate(previewSlot.gameObject);
                }
            }
        }

        // Method that initializes inventory cells and referenced components such as loot inventory and equipment panels
        public void Initialize()
        {
            if(SaveData.instance == null)
                ClearPreview();

            inventoryManager = FindObjectOfType<InventoryManager>();
            inventoryManager.inventory = this;

            //Inventory grid initialization
            for (int i = 0; i < column; i++)
            {
                for (int j = 0; j < row; j++)
                {
                    var _cell = Instantiate(cell, transform, true);
                    _cell.rectTransform.sizeDelta = new Vector2(cellSize - padding, cellSize - padding);
                    _cell.rectTransform.anchoredPosition = new Vector2(((cellSize * i) + padding), ((-cellSize * j) + padding));
                    _cell.rectTransform.localScale = new Vector2(1, 1);

                    _cell.name = i + "," + j;

                    var slot = _cell.GetComponent<GridSlot>();
                    slot.free = true;
                    slot.image.color = normalCellColor;
                    slot.x = i; slot.y = j;
                    slots.Add(slot);
                }
            }

            //Equipment grid initialization
            if (equipmentPanels != null)
            {
                for (int k = 0; k < equipmentPanels.Count; k++)
                {
                    for (int i = 0; i < equipmentPanels[k].width; i++)
                    {
                        for (int j = 0; j < equipmentPanels[k].height; j++)
                        {

                            var _cell = Instantiate(cell, equipmentPanels[k].transform, true);
                            _cell.rectTransform.sizeDelta = new Vector2(cellSize - padding, cellSize - padding);
                            _cell.rectTransform.anchoredPosition = new Vector2(((cellSize * i) + padding), ((-cellSize * j) + padding));
                            _cell.rectTransform.localScale = new Vector2(1, 1);

                            _cell.name = i + (k + 1) * 100 + "," + j + (k + 1) * 100;

                            var slot = _cell.GetComponent<GridSlot>();
                            slot.free = true;
                            slot.image.color = normalCellColor;
                            slot.equipmentPanel = equipmentPanels[k];
                            slot.x = i + (k + 1) * 100; slot.y = j + (k + 1) * 100;
                            slots.Add(slot);

                            if (i == 0 && j == 0)
                            {
                                equipmentPanels[k].mainSlot = slot;
                            }
                        }
                    }
                }
            }

            transform.SetAsLastSibling();
        }

        // Use this method to add item to inventory
        public bool AddItem(SharItem item)
        {
            if (CheckFreeSpaceForAllSlots(item.itemWidth, item.itemHeight))
            {
                var _slot = CheckFreeSpaceForAllSlots(item.itemWidth, item.itemHeight);
                var _InventoryItem = Instantiate(cell).gameObject.AddComponent<InventoryItem>();
                var _InventoryItemImage = _InventoryItem.GetComponent<Image>();

                _InventoryItemImage.rectTransform.SetParent(transform);
                _InventoryItemImage.rectTransform.sizeDelta = new Vector2(cellSize * item.itemWidth - padding, cellSize * item.itemHeight - padding);
                _InventoryItemImage.rectTransform.anchoredPosition = _slot.GetComponent<RectTransform>().anchoredPosition;
                _InventoryItemImage.color = Color.white;
                _InventoryItemImage.sprite = item.itemIcon;

                _InventoryItem.item = item;

                _InventoryItem.finalPosition = _slot.GetComponent<RectTransform>().anchoredPosition;

                _InventoryItem.x = _slot.x;
                _InventoryItem.y = _slot.y;
                _InventoryItem.width = item.itemWidth;
                _InventoryItem.height = item.itemHeight;
                _InventoryItem.inventory = this;

                MarkSlots(_slot.x, _slot.y, item.itemWidth, item.itemHeight, false);

                Destroy(_InventoryItem.GetComponent<GridSlot>());

                inventoryItems.Add(_InventoryItem);

                _InventoryItem.transform.parent = transform;

                if(item!=null) item.gameObject.SetActive(false);
 
                

                
                if (autoEquipItems)
                {
                    foreach (var equipmentPanel in equipmentPanels)
                    {
                        if (equipmentPanel.allowedItemType == _InventoryItem.item.itemType && equipmentPanel.equipedItem == null)
                        {
                            EquipItem(equipmentPanel, _InventoryItem);
                            MarkSlots(_slot.x, _slot.y, item.itemWidth, item.itemHeight, true);
                        }
                    }
                }

                OnInventoryItemAdd.Invoke();

                return true;
            }
            return false;
        }

        // Method to add item to special grid slot.
        public bool AddItem(SharItem item, int x, int y)
        {
            if (CheckFreeSpaceForAllSlots(item.itemWidth, item.itemHeight))
            {
                var _InventoryItem = Instantiate(cell).gameObject.AddComponent<InventoryItem>();
                var _InventoryItemImage = _InventoryItem.GetComponent<Image>();

                _InventoryItemImage.rectTransform.SetParent(FindSlotByIndex(x, y).GetComponent<RectTransform>().transform.parent);
                _InventoryItemImage.rectTransform.sizeDelta = new Vector2(cellSize * item.itemWidth - padding, cellSize * item.itemHeight - padding);
                _InventoryItemImage.rectTransform.anchoredPosition = FindSlotByIndex(x, y).GetComponent<RectTransform>().anchoredPosition;

                _InventoryItem.item = item;

                _InventoryItemImage.sprite = item.itemIcon;
                _InventoryItem.x = x;
                _InventoryItem.y = y;
                _InventoryItem.width = item.itemWidth;
                _InventoryItem.height = item.itemHeight;
                _InventoryItem.inventory = this;

                _InventoryItem.finalPosition = FindSlotByIndex(x, y).GetComponent<RectTransform>().anchoredPosition;

                MarkSlots(x, y, item.itemWidth, item.itemHeight, false);

                Destroy(_InventoryItem.GetComponent<GridSlot>());

                inventoryItems.Add(_InventoryItem);

                item.transform.parent = this.transform;

                item.gameObject.SetActive(false);

                if (autoEquipItems)
                {
                    foreach (var equipmentPanel in equipmentPanels)
                    {
                        if (equipmentPanel.allowedItemType == _InventoryItem.item.itemType && equipmentPanel.equipedItem == null)
                        {
                            EquipItem(equipmentPanel, _InventoryItem);
                            MarkSlots(x, y, item.itemWidth, item.itemHeight, true);
                        }
                    }
                }
                
                OnInventoryItemAdd.Invoke();

                return true;
            }
            return false;
        }

        // Method returns true if item with specified title exists in the inventory and remove it if needed
        public bool TakeItemFromPlayer(SharItem itemToCheck, int itemAmount, bool removeItem)
        {
            List<InventoryItem> itemsToRemove = new List<InventoryItem>();

            foreach (var item in inventoryItems)
            {
                if (item.item.itemID == itemToCheck.itemID)
                {
                    if (removeItem)
                    {
                        
                        foreach (EquipmentPanel panel in equipmentPanels)
                        {
                            if (panel.equipedItem == null) continue;

                            if (!panel.equipedItem.Equals(item.item))
                            {
                                continue;
                            }

                            if (panel.GetType() == typeof(ArmorEquipmentPanel))
                            {
                                //GameManager.Instance.PlayerObject.GetComponent<jbzdy.Items.ItemEquipper>().UnequipArmor((ArmorItem)itemToCheck);
                            }
                            else
                            {
                                //GameManager.Instance.PlayerObject.GetComponent<jbzdy.Items.ItemEquipper>().UnequipWeaponOrTrinket(itemToCheck);
                            }

                        }
                        if (itemAmount >= item.item.itemStackSize)
                        {
                            itemsToRemove.Add(item);
                        }
                        else
                        {
                            item.item.itemStackSize -= itemAmount;
                        }
                        
                    }
                    
                    if (item.item.itemStackSize < itemAmount)
                    {
                        itemAmount -= item.item.itemStackSize;
                    }
                    else
                    {
                        foreach (var itemToRemove in itemsToRemove)
                        {
                            RemoveItem(itemToRemove);
                        }
                        return true;
                    }
      
                }
            }

            return false;
        }

        public int CheckItemAmount(SharItem itemToCheck)
        {
            int itemAmount = 0;

            foreach (var item in inventoryItems)
            {
                if (item.item.itemID == itemToCheck.itemID)
                {
                    itemAmount += item.item.itemStackSize;
                }
            }

            return itemAmount;
        }


        // Use this method to drop items from inventory. This method is not destroys items
        public void DropItem(InventoryItem InventoryItem)
        {
            if (InventoryItem == null)
                return;

            int _temp_x, _temp_y, _temp_width, _temp_height;

            _temp_x = InventoryItem.x;
            _temp_y = InventoryItem.y;
            _temp_width = InventoryItem.width;
            _temp_height = InventoryItem.height;

            InventoryItem.item.gameObject.SetActive(true);
            InventoryItem.item.transform.position = player.transform.position + player.transform.forward * distanceAwayFromPlayerToDrop;

            if (levelPoint != null)
            {
                InventoryItem.item.transform.parent = levelPoint.parent;
            }

            InventoryItem.item.gameObject.transform.parent = null;

            inventoryItems.Remove(InventoryItem);

            Destroy(InventoryItem.gameObject);

            OnInventoryItemDrop.Invoke();

            MarkSlots(_temp_x, _temp_y, _temp_width, _temp_height, true);
        }

        // This method is for auto-equip
        public void EquipItem(EquipmentPanel panel, InventoryItem item)
        {
            item.transform.SetParent(panel.mainSlot.transform.parent);

            item.finalPosition = panel.mainSlot.GetComponent<RectTransform>().anchoredPosition;
            item.x = panel.mainSlot.x;
            item.y = panel.mainSlot.y;
            
            //item.item.
            MarkSlots(panel.mainSlot.x, panel.mainSlot.y, item.width, item.height, false);
            panel.equipedItem = item.item;
        }

        // Method to remove and destroy an item from a scene
        public void RemoveItem(InventoryItem InventoryItem)
        {
            int temp_x, temp_y, temp_width, temp_height;

            temp_x = InventoryItem.x;
            temp_y = InventoryItem.y;
            temp_width = InventoryItem.width;
            temp_height = InventoryItem.height;

            inventoryItems.Remove(InventoryItem);

            Destroy(InventoryItem.gameObject);

            if(InventoryItem.item!=null)Destroy(InventoryItem.item.gameObject);

            OnInventoryItemRemove.Invoke();

            MarkSlots(temp_x, temp_y, temp_width, temp_height, true);
        }

        // Use this method to mark inventory slots free or used by some item.
        // Used for visual marking of slots with their current state (used, free, blocked)
        public void MarkSlots(int startSlot_x, int startSlot_y, int width, int height, bool isFree)
        {
            for (int i = startSlot_x; i < startSlot_x + width; i++)
            {
                for (int j = startSlot_y; j < startSlot_y + height; j++)
                {
                    if (FindSlotByIndex(i, j))
                    {
                        var slot = FindSlotByIndex(i, j);
                        slot.free = isFree;

                        if (slot.free) 
                            slot.image.color = normalCellColor; 
                        else 
                            slot.image.color = hoveredCellColor;
                    }
                }
            }
        }

        // Method to check if we can put an item with some width & height to a specified slot 
        public GridSlot CheckFreeSpaceAtSlot(int cell_x, int cell_y, int width, int height)
        {
            for (int i = cell_x; i < cell_x + width; i++)
            {
                for (int j = cell_y; j < cell_y + height; j++)
                {
                    if (FindSlotByIndex(i, j) == null || FindSlotByIndex(i, j).free == false)
                    {
                        return null;
                    }
                }
            }

            return FindSlotByIndex(cell_x, cell_y);
        }

        // Method for marking slots as free or not. Sometimes we need to redraw inventory state in some cases
        public void DrawRegularSlotsColors()
        {
            foreach (var slot in slots)
            {
                if (slot.free)
                {
                    slot.image.color = normalCellColor;
                }
                else
                {
                    slot.image.color = hoveredCellColor;
                }
            }
        }

        // When dragging item in inventory grid we call this method to draw current slots state at realtime
        public void DrawColorsForHoveredSlots(int startSlot_x, int startSlot_y, int width, int height)
        {
            for (int i = startSlot_x; i < startSlot_x + width; i++)
            {
                for (int j = startSlot_y; j < startSlot_y + height; j++)
                {
                    var slot = FindSlotByIndex(i, j);

                    if (slot != null)
                    {
                        if (slot.free)
                            slot.image.color = hoveredCellColor;
                        else 
                            slot.image.color = blockedCellColor;
                    }
                }
            }
        }

        // Same as DrawColorsForHoveredSlots but for stacking items. With default draw method blocked color will be apeared on attempt to stack items
        public void DrawColorForStackableHoveredSlots(int startSlot_x, int startSlot_y, int width, int height)
        {
            for (int i = startSlot_x; i < startSlot_x + width; i++)
            {
                for (int j = startSlot_y; j < startSlot_y + height; j++)
                {
                    var slot = FindSlotByIndex(i, j);

                    if (slot != null)
                    {
                         slot.image.color = hoveredCellColor;
                    }
                }
            }
        }
        
        // Method to check if we have enough space to pickup something
        public GridSlot CheckFreeSpaceForAllSlots(int width, int height)
        {
            foreach (var slot in slots)
            {
                if (CheckFreeSpaceAtSlot(slot.x, slot.y, width, height) && slot.equipmentPanel == null)
                    return CheckFreeSpaceAtSlot(slot.x, slot.y, width, height);
            }

            return null;
        }

        // Method to find slot with coordinates. Return slot if found or null when is none
        public GridSlot FindSlotByIndex(int x, int y)
        {
            foreach (var slot in slots)
            {
                if (slot.x == x && slot.y == y)
                    return slot;
            }

            return null;
        }

        // Remove inventory item
        public void RemoveInventoryItem(InventoryItem InventoryItem)
        {
            int temp_x, temp_y, temp_width, temp_height;

            temp_x = InventoryItem.x;
            temp_y = InventoryItem.y;
            temp_width = InventoryItem.width;
            temp_height = InventoryItem.height;

            inventoryItems.Remove(InventoryItem);
            
            Destroy(InventoryItem.gameObject);

            MarkSlots(temp_x, temp_y, temp_width, temp_height, true);
        }

        // Method which allows us to substract stack on a need
        public void SubstractStack(InventoryItem InventoryItem)
        {
            // If check free space == false -> exit
            if (InventoryItem.item.itemStackSize < 2)
                return;

            if (InventoryItem.item.isStackable)
            {
                if (InventoryItem.item.itemStackSize % 2 == 0)
                {
                    if (CheckFreeSpaceForAllSlots(InventoryItem.width, InventoryItem.height) == null)
                    {
                        return;
                    }

                    InventoryItem.item.gameObject.SetActive(true);

                    var second_item_go = Instantiate(InventoryItem.item.gameObject);
                    var second_item = second_item_go.GetComponent<SharItem>();

                    InventoryItem.item.itemStackSize /= 2;
                    second_item.itemStackSize = InventoryItem.item.itemStackSize;

                    AddItem(second_item);

                    second_item.name = InventoryItem.item.name;

                    InventoryItem.item.gameObject.SetActive(false);
                }
                else if (InventoryItem.item.itemStackSize % 2 == 1)
                {
                    if (CheckFreeSpaceForAllSlots(InventoryItem.width, InventoryItem.height) == null)
                    {
                        return;
                    }

                    InventoryItem.item.gameObject.SetActive(true);

                    var second_item_go = Instantiate(InventoryItem.item.gameObject);
                    var second_item = second_item_go.GetComponent<SharItem>();

                    InventoryItem.item.itemStackSize = (InventoryItem.item.itemStackSize - 1) / 2;
                    second_item.itemStackSize = InventoryItem.item.itemStackSize + 1;

                    AddItem(second_item);

                    second_item.name = InventoryItem.item.name;

                    InventoryItem.item.gameObject.SetActive(false);
                }
            }
            else
            {
                return;
            }

        }

        // Method for items auto stacking
        public void AutoStack(InventoryItem InventoryItem)
        {
            List<InventoryItem> items = new List<InventoryItem>();

            foreach (var i in inventoryItems)
            {
                if (i.item.itemID == InventoryItem.item.itemID)
                {
                    items.Add(i);
                }
            }

            foreach (var i in items)
            {
                if (InventoryItem.item.itemStackSize + i.item.itemStackSize <= InventoryItem.item.itemMaxStackSize)
                {
                    InventoryItem.item.itemStackSize += InventoryItem.item.itemStackSize;
                    RemoveItem(i);
                }

                if (InventoryItem.item.itemStackSize == InventoryItem.item.itemMaxStackSize)
                    break;
            }
        }

        // We can use consumable items with this method. Stacked items will be decreased by one. Single item will be removed after use
        public void UseItem(InventoryItem InventoryItem, bool closeInventory)
        {
            // If not stackable
            if (!InventoryItem.item.isStackable || InventoryItem.item.itemStackSize <= 1)
            {
                InventoryItem.item.onUseEvent.Invoke();
                RemoveItem(InventoryItem);
            }
            // If stackable
            else
            {
                InventoryItem.item.onUseEvent.Invoke();
                InventoryItem.item.itemStackSize -= 1;
            }

            if (closeInventory)
                InventoryManager.showInventory = false;
        }
    }
}

