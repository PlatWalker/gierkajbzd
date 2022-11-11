using System;
using jbzd.Items;
using jbzd.Items.Legacy.Inventory.Inventory;
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

        //public Item test;

        private PlayerManager _playerController;
        
        [Inject]
        public void Construct(PlayerManager playerController)
        {
            _playerController = playerController;
        }
        
        public new void Awake()
        {
            item = this.GetComponent<SharItem>();
            //test = GetComponentInParent<Item>();
            inventory = InventoryClass.Instance;            
        }

        public void Start()
        {
            //test = GetComponentInParent<Item>();
        }

        public override void Interact()
        {
            base.Interact();
            //TODO wywalic
            //PickUp();
            //GameManager.Instance.PlayerObject.GetComponent<jbzdy.Items.ItemEquipper>().EquipWeaponOrTrinket(item);
            //_playerController.GetComponent<jbzdy.Items.ItemEquipper>().EquipItem(test.ItemSO);
        }

        private void PickUp()
        {
            InventoryClass.Instance.AddItem(item);
        }
    }
}

