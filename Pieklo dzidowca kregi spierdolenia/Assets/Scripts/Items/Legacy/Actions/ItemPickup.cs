using System;
using jbzd.Items;
using jbzd.MainHero;
using jbzdy.Items;
using jbzdy.Inventory;
using UnityEditor;
using UnityEngine;
using Zenject;

// <summary>
// Napisane przez Sharashino
// 
// Skrypt umożliwiającymi obiektom bycie podniesionym
// 
// Posiada nadpisywalną metode z Interactables dla zmienienia logiki podczas interakcji z obiektem (tutaj podnoszenie przedmiotu)
// </summary>
namespace jbzdy.Actions.Interaction
{
    public class ItemPickup : Interactable
    {
        [SerializeField] private SharItem item = default;

        public InventoryClass inventory;

        public SharItem Item { get => item; set => item = value; }

        public PlaceHolderForItemSO test;

        private PlayerController _playerController;
        
        [Inject]
        public void Construct(PlayerController playerController)
        {
            _playerController = playerController;
        }
        
        public new void Awake()
        {
            item = this.GetComponent<SharItem>();
            test = GetComponentInParent<PlaceHolderForItemSO>();
            inventory = InventoryClass.Instance;            
        }

        public void Start()
        {
            test = GetComponentInParent<PlaceHolderForItemSO>();
        }

        public override void Interact()
        {
            base.Interact();
            //TODO wywalic
            //PickUp();
            //GameManager.Instance.PlayerObject.GetComponent<jbzdy.Items.ItemEquipper>().EquipWeaponOrTrinket(item);
            _playerController.GetComponent<jbzdy.Items.ItemEquipper>().EquipItem(test.ItemSO);
        }

        private void PickUp()
        {
            InventoryClass.Instance.AddItem(item);
        }
    }
}

