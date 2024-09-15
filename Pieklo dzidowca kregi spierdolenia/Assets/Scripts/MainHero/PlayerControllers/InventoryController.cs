using System;
using System.Collections.Generic;
using System.Linq;
using jbzd.Items;
using jbzd.SavingSystem;
using jbzd.SavingSystem.SaveData;
using jbzd.UI.Inventory;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using static jbzd.Items.ItemTypes;

namespace jbzd.MainHero.PlayerControllers
{
    [RequireComponent(typeof(ItemEquiper))]
    public class InventoryController : MonoBehaviour, IPlayerController, ISaveable 
    {
        public List<InventorySlot> slots = new();
        public IReadOnlyDictionary<ItemTypes, ItemSO> EquippedItems => equippedItems;
        public delegate void ItemAcquired(ItemSO itemAcquired, int numberOfItems);
        public event ItemAcquired OnItemAcquiring;
        
        [field: SerializeField] private ItemSO headArmor;
        [field: SerializeField] private ItemSO chestArmor;
        [field: SerializeField] private ItemSO legArmor;
        [field: SerializeField] private ItemSO bootsArmor;
        [field: SerializeField] private ItemSO weapon;
        
        private Dictionary<ItemTypes, ItemSO> equippedItems = new();
        private ItemEquiper _itemEquiper;
        private Transform _playerTransform;
        private Item.Factory _itemFactory;

        [Inject]
        public void Construct(Item.Factory factory)
        {
            _itemFactory = factory;
        }
        
        private void Awake()
        {
            for(var i = 0; i < 49; i++)
            {
                slots.Add(new InventorySlot());
            }
            _itemEquiper = GetComponent<ItemEquiper>();
            _playerTransform = GetComponent<Transform>();
            AssignItemTypes();
        }

        private void AssignItemTypes()
        {
            equippedItems.Add(Weapon, weapon);
            equippedItems.Add(HeadArmor, headArmor);
            equippedItems.Add(ChestArmor, chestArmor);
            equippedItems.Add(LegArmor, legArmor);
            equippedItems.Add(BootsArmor, bootsArmor);
        }

        public bool EquipItem(ItemSO item)
        {
            if (!EquippedItems.ContainsKey(item.ItemType))
            {
                Debug.Log("You cannot equip that type of item!");
                return false;
            }
            
            equippedItems[item.ItemType] = item;
            
            _itemEquiper.EquipItem(item.ItemPrefab.GetComponent<Item>());
            return true;
        }

        public void UnequipItem(ItemSO item)
        {
            if (!EquippedItems.ContainsKey(item.ItemType))
            {
                Debug.LogError("You cannot unequip that type of item!");
                return;
            }
            
            _itemEquiper.UnequipItem(item);
            
            equippedItems[item.ItemType] = null;
        }

        public bool ContainsItem(ItemSO itemToAdd, out List<InventorySlot> invSlots)
        {
            invSlots = slots.Where(i => i.ItemData == itemToAdd).ToList();
            return invSlots != null;
        }

        public bool HasFreeSlot(out InventorySlot freeSlot)
        {
            freeSlot = slots.FirstOrDefault(i => i.ItemData == null);
            return freeSlot != null;
        }

        public bool PickUpItem(ItemSO item, int numberOfItems = 1)
        {
            if (ContainsItem(item, out var invSlots))
            {
                foreach (var slot in invSlots.Where(slot => slot.RoomLeftInStack(numberOfItems)))
                {
                    slot.AddToStack(numberOfItems);
                    OnItemAcquiring?.Invoke(item, numberOfItems);
                    Debug.Log($"podniesiono: {item.name} w ilosci {numberOfItems} i dodano do stacka");
                    return true;
                }
            }

            if (!HasFreeSlot(out var freeSlot)) return false;
            
            freeSlot.UpdateInventorySlot(item, numberOfItems);
            OnItemAcquiring?.Invoke(item, numberOfItems);
            Debug.Log($"podniesiono: {item.name} w ilosci {numberOfItems} i dodano do wolnego slota");
            return true;
        }

        public void DropItem(ItemSO item)
        {
            if (!RemoveItem(item)) return;

            var position = _playerTransform.position;
            item.ItemPrefab.transform.position = new Vector3(position.x, item.ItemPrefab.transform.position.y, position.z);
            var newItem = _itemFactory.Create(item.ItemPrefab);
            SceneManager.MoveGameObjectToScene(newItem.gameObject, SceneManager.GetActiveScene());
        }
        
        public bool RemoveItem(ItemSO item, int numberOfItems = 1)
        {
            if (!ContainsItem(item, out var invSlots) || invSlots.Sum(x=>x.StackSize) < numberOfItems)
            {
                Debug.LogWarning("Błąd przy usuwaniu itema z inventory - możliwe, że gracz go nie posiada");
                return false;
            }
            
            Debug.Log($"zabrano: {item.name} w ilosci {numberOfItems}");
            
            foreach (var slot in invSlots)
            {
                var leftNumberOfItems = slot.RemoveFromStack(numberOfItems);
                if (leftNumberOfItems <= 0) break;                
            }

            return true;
        }

        public void SortBy(SortingType sortBy)
        {
            slots.Sort((x, y) =>
            {
                switch (sortBy)
                {
                    case SortingType.Undefined:
                        Debug.LogError("Passed enum is undefined. It can cause strange behaviour in equipment");
                        return 0;
                    case SortingType.All:
                        if (x.ItemData is null && y.ItemData is null)
                        {
                            return 0;
                        }
                        if (x.ItemData is null && y.ItemData is not null)
                        {
                            return 1;
                        }
                        if (x.ItemData is not null && y.ItemData is null)
                        {
                            return -1;
                        }
                        return 0;
                    case SortingType.Weapons:
                        if (x.ItemData is null && y.ItemData is null)
                        {
                            return 0;
                        }
                        if (x.ItemData is null && y.ItemData is not null)
                        {
                            return 1;
                        }
                        if (x.ItemData is not null && y.ItemData is null)
                        {
                            return -1;
                        }
                        if (x.ItemData.ItemType is not Weapon && y.ItemData.ItemType is not Weapon)
                        {
                            return 0;
                        }
                        if (x.ItemData.ItemType is Weapon && y.ItemData.ItemType is Weapon)
                        {
                            return 0;
                        }
                        if (x.ItemData.ItemType is Weapon && y.ItemData.ItemType is not Weapon)
                        {
                            return -1;
                        }
                        if (x.ItemData.ItemType is not Weapon && y.ItemData.ItemType is Weapon)
                        {
                            return 1;
                        }
                        break;
                    case SortingType.Armors:
                        if (x.ItemData is null && y.ItemData is null)
                        {
                            return 0;
                        }
                        if (x.ItemData is null && y.ItemData is not null)
                        {
                            return 1;
                        }
                        if (x.ItemData is not null && y.ItemData is null)
                        {
                            return -1;
                        }
                        if (x.ItemData.ItemType is not (HeadArmor or ChestArmor or LegArmor or BootsArmor)
                            && y.ItemData.ItemType is not (HeadArmor or ChestArmor or LegArmor or BootsArmor))
                        {
                            return 0;
                        }
                        if (x.ItemData.ItemType is (HeadArmor or ChestArmor or LegArmor or BootsArmor) 
                            && y.ItemData.ItemType is (HeadArmor or ChestArmor or LegArmor or BootsArmor))
                        {
                            return 0;
                        }
                        if (x.ItemData.ItemType is (HeadArmor or ChestArmor or LegArmor or BootsArmor) 
                            && y.ItemData.ItemType is not (HeadArmor or ChestArmor or LegArmor or BootsArmor))
                        {
                            return -1;
                        }
                        if (x.ItemData.ItemType is not (HeadArmor or ChestArmor or LegArmor or BootsArmor) 
                            && y.ItemData.ItemType is (HeadArmor or ChestArmor or LegArmor or BootsArmor))
                        {
                            return 1;
                        }
                        break;
                    case SortingType.QuestItems:
                        if (x.ItemData is null && y.ItemData is null)
                        {
                            return 0;
                        }
                        if (x.ItemData is null && y.ItemData is not null)
                        {
                            return 1;
                        }
                        if (x.ItemData is not null && y.ItemData is null)
                        {
                            return -1;
                        }
                        if (x.ItemData.ItemType is not ItemTypes.QuestItem && y.ItemData.ItemType is not ItemTypes.QuestItem)
                        {
                            return 0;
                        }
                        if (x.ItemData.ItemType is ItemTypes.QuestItem && y.ItemData.ItemType is ItemTypes.QuestItem)
                        {
                            return 0;
                        }
                        if (x.ItemData.ItemType is ItemTypes.QuestItem && y.ItemData.ItemType is not ItemTypes.QuestItem)
                        {
                            return -1;
                        }
                        if (x.ItemData.ItemType is not ItemTypes.QuestItem && y.ItemData.ItemType is ItemTypes.QuestItem)
                        {
                            return 1;
                        }
                        break;
                    case SortingType.Others: // for now it's only consumables, but maybe later it will be something else?
                        if (x.ItemData is null && y.ItemData is null)
                        {
                            return 0;
                        }
                        if (x.ItemData is null && y.ItemData is not null)
                        {
                            return 1;
                        }
                        if (x.ItemData is not null && y.ItemData is null)
                        {
                            return -1;
                        }
                        if (x.ItemData.ItemType is not Consumable && y.ItemData.ItemType is not Consumable)
                        {
                            return 0;
                        }
                        if (x.ItemData.ItemType is Consumable && y.ItemData.ItemType is Consumable)
                        {
                            return 0;
                        }
                        if (x.ItemData.ItemType is Consumable && y.ItemData.ItemType is not Consumable)
                        {
                            return -1;
                        }
                        if (x.ItemData.ItemType is not Consumable && y.ItemData.ItemType is Consumable)
                        {
                            return 1;
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(sortBy), sortBy, null);
                }

                return 0;
            });
        }

        public void LoadData(GameData gameData)
        {
            slots = gameData.InventorySaveData.InventorySlots;
            headArmor = gameData.InventorySaveData.headArmor;
            chestArmor = gameData.InventorySaveData.chestArmor;
            legArmor = gameData.InventorySaveData.legArmor;
            bootsArmor = gameData.InventorySaveData.bootsArmor;
            weapon = gameData.InventorySaveData.weapon;
        }

        public void SaveData(ref GameData gameData)
        {
            gameData.InventorySaveData = new InventoryControllerSaveData
            {
                InventorySlots = slots.ToList(),
                headArmor = headArmor,
                chestArmor = chestArmor,
                legArmor = legArmor,
                bootsArmor = bootsArmor,
                weapon = weapon
            };
        }
    }
}