using System.Collections.Generic;
using jbzd.Items;
using jbzd.MainHero.PlayerControllers;
using MyBox;
using UnityEngine;

namespace jbzd.MainHero.DebugScripts
{
    [RequireComponent(typeof(PlayerManager))]
    public class GetItemsDebug: MonoBehaviour
    {
        [SerializeField]
        private List<ItemSO> listOfItemsToGet = new();

        private InventoryController _inventoryController;
        
        public void Awake()
        {
            _inventoryController = GetComponent<InventoryController>();
        }

        [ButtonMethod]
        public void GetItems()
        {
            foreach (var item in listOfItemsToGet)
            {
                _inventoryController.PickUpItem(item);
            }
        }
    }
}