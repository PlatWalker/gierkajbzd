using System.Collections.Generic;
using System.Linq;
using jbzd.MainHero;
using jbzd.MainHero.PlayerControllers;
using UnityEngine;
using Zenject;

namespace jbzd.UI.Inventory
{
    public class InventoryUIController : UserInterfaceController
    {
        [SerializeField] 
        private GameObject inventoryItemPrefab;
        
        private PlayerManager _playerManager;
        private InventoryController _inventoryController;
        private List<InventorySlotUI> _slotsUI = new();

        private bool _isFirstTimeAwoken = true;
        
        [Inject]
        public void Construct(PlayerManager playerManager)
        {
            _playerManager = playerManager;
        }

        public void OnEnable()
        {
            if (_isFirstTimeAwoken) Init();
                
            foreach (var slotUI in _slotsUI)
            {
                slotUI.UpdateContentOfSlot();
            }
        }

        private void Init()
        {
            _isFirstTimeAwoken = false;
            
            _inventoryController = _playerManager.GetPlayerController<InventoryController>();
            _slotsUI = GetComponentsInChildren<InventorySlotUI>().ToList();
            
            int i;

            for (i = 0; i < _inventoryController.slots.Count; i++)
            {
                _slotsUI[i].Init(_inventoryController.slots[i], inventoryItemPrefab, _inventoryController, gameObject);
            }
            i--;
            foreach(var equipment in _inventoryController.EquippedItems)
            {
                _slotsUI[++i].Init(equipment, inventoryItemPrefab, _inventoryController, gameObject);
            }
        }
        
        /// <summary>
        /// Use this method only when inventory is displayed
        /// </summary>
        public void SortBy(SortingType sortBy)
        {
            _inventoryController.SortBy(sortBy);
            
            int i;

            for (i = 0; i < _inventoryController.slots.Count; i++)
            {
                _slotsUI[i].UpdateContentOfSlot(_inventoryController.slots[i]);
            }
        }

        public override bool InitialActivationState() => false;
    }
}