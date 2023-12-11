using System;
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
        [SerializeField] private Dictionary<ItemSO, int> itemsInInventory = new();
        [SerializeField] private Dictionary<ItemTypes, Item> equippedItems = new();
        public IReadOnlyDictionary<ItemSO, int> ItemsInInventory => itemsInInventory;
        public IReadOnlyDictionary<ItemTypes, Item> EquippedItems => equippedItems;

        [field: SerializeField] private Item headArmor;
        [field: SerializeField] private Item chestArmor;
        [field: SerializeField] private Item legArmor;
        [field: SerializeField] private Item bootsArmor;
        [field: SerializeField] private Item weapon;
        
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

        public void EquipItem(Item item)
        {
            if (!EquippedItems.ContainsKey(item.ItemSO.ItemType))
            {
                Debug.LogError("You cannot equip that type of item!");
                return;
            }
            
            equippedItems[item.ItemSO.ItemType] = item;
            
            _itemEquiper.EquipItem(item);
        }

        public void UnequipItem(Item item)
        {
            if (!EquippedItems.ContainsKey(item.ItemSO.ItemType))
            {
                Debug.LogError("You cannot unequip that type of item!");
                return;
            }
            
            _itemEquiper.UnequipItem(item);
            
            equippedItems[item.ItemSO.ItemType] = item;
        }

        //TODO obsługa za dużej ilości itemów w eq
        public bool PickUpItem(ItemSO item, int numberOfItems = 1)
        {
            Debug.Log("item podniesiony");

            if (itemsInInventory.ContainsKey(item))
                itemsInInventory[item] += numberOfItems;
            else
                itemsInInventory.Add(item, numberOfItems);

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
            Debug.Log("item zabrany");
            
            if (!itemsInInventory.ContainsKey(item) || itemsInInventory[item] < numberOfItems)
            {
                Debug.LogWarning("Błąd przy usuwaniu itema z inventory - możliwe, że gracz go nie posiada");
                return false;
            }

            itemsInInventory[item] -= numberOfItems;

            if (itemsInInventory[item] == 0) itemsInInventory.Remove(item);

            return true;
        }
    }
}