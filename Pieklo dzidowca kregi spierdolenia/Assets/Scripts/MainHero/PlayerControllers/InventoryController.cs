using System.Collections.Generic;
using System.Linq;
using jbzd.Items;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Zenject;

namespace jbzd.MainHero.PlayerControllers
{
    [RequireComponent(typeof(ItemEquiper))]
    public class InventoryController : MonoBehaviour , IPlayerController
    {
        public List<InventorySlot> slots = new();
        public IReadOnlyDictionary<ItemTypes, ItemSO> EquippedItems => equippedItems;

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
            for(int i = 0; i < 20; i++)
            {
                slots.Add(new InventorySlot());
            }
            _itemEquiper = GetComponent<ItemEquiper>();
            _playerTransform = GetComponent<Transform>();
            AssignItemTypes();
        }

        private void AssignItemTypes()
        {
            equippedItems.Add(ItemTypes.Weapon, weapon);
            equippedItems.Add(ItemTypes.HeadArmor, headArmor);
            equippedItems.Add(ItemTypes.ChestArmor, chestArmor);
            equippedItems.Add(ItemTypes.LegArmor, legArmor);
            equippedItems.Add(ItemTypes.BootsArmor, bootsArmor);
        }

        public bool EquipItem(ItemSO item)
        {
            if (!EquippedItems.ContainsKey(item.ItemType))
            {
                Debug.LogError("You cannot equip that type of item!");
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
            Debug.Log("item podniesiony");

            if (ContainsItem(item, out List<InventorySlot> invSlots))
            {
                foreach (var slot in invSlots)
                {
                    if (slot.RoomLeftInStack(numberOfItems))
                    {
                        slot.AddToStack(numberOfItems);
                        return true;
                    }
                }
                
            }
            
            if (HasFreeSlot(out InventorySlot freeSlot))
            {
                freeSlot.UpdateInventorySlot(item, numberOfItems);
                return true;
            }    
            
            return false;
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
            Debug.Log("item zabrany");
            
            if (!ContainsItem(item, out var invSlots) || invSlots.Sum(x=>x.StackSize) < numberOfItems)
            {
                Debug.LogWarning("Błąd przy usuwaniu itema z inventory - możliwe, że gracz go nie posiada");
                return false;
            }

            foreach (var slot in invSlots)
            {
                var leftNumberOfItems = slot.RemoveFromStack(numberOfItems);
                if (leftNumberOfItems <= 0) break;                
            }

            return true;
        }
    }
}