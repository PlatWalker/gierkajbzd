using jbzd.Common;
using jbzd.Common.InputSystem.Inputs;
using jbzd.MainHero;
using jbzd.MainHero.PlayerControllers;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace jbzd.UI.Inventory
{
    public class InventoryUIController : UserInterfaceController
    {
        private PlayerManager _playerManager;
        [SerializeField] private InventoryController _inventoryController;
        [SerializeField] private Transform _slotsContainer;
        [SerializeField] private Transform _equippedContainer;
        [SerializeField] private GameObject _slotPrefab;
        [SerializeField] private GameObject _inventoryItemPrefab;

        private List<InventorySlotUI> _slotsUI = new();

        [Inject]
        public void Construct(PlayerManager playerManager)
        {
            _playerManager = playerManager;
        }

        public void Awake()
        {
            for (int i = 0; i < _inventoryController.Slots.Count; i++) 
            {
                _slotsUI.Add(Instantiate(_slotPrefab, _slotsContainer).GetComponent<InventorySlotUI>());
            }

            for (int i = 0; i < 6; i++)
            {
                _slotsUI.Add(Instantiate(_slotPrefab, _equippedContainer).GetComponent<InventorySlotUI>());
            }
        }

        private void PrepareItemsGrid()
        {
            int i;

            for (i = 0; i < _inventoryController.Slots.Count; i++)
            {
                _slotsUI[i].GetComponent<InventorySlotUI>().ShowSlot(_inventoryController.Slots[i]);
            }
            i--;
            foreach(var equipment in _inventoryController.EquippedItems)
            {
                
                _slotsUI[++i].GetComponent<InventorySlotUI>().ShowSlot(equipment.Value, _inventoryController);
            }
        }

        private void OnEscapeClick()
        {
            _playerManager.CanPlayerMove = true;
            gameObject.SetActive(false);
        }

        private void OnInventoryOpened()
        {
            gameObject.SetActive(!gameObject.activeSelf);

            if (gameObject.activeSelf)
            {
                FreezeTime.Freeze();
                _playerManager.CanPlayerMove = false;
                PrepareItemsGrid();

            }
            else
            {
                FreezeTime.Unfreeze();
                _playerManager.CanPlayerMove = true;
            }

        }

        public override void ConnectInputToHandler(UserInterfaceInput input)
        {
            input.OnEscapeClick += OnEscapeClick;
            input.OnInventoryOpened += OnInventoryOpened;
        }

        public override bool InitialActivationState() => false;
    }
}
