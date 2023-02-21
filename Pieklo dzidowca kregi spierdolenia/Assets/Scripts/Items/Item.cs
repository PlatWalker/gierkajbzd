using jbzd.Common.Interfaces;
using jbzd.MainHero;
using jbzd.MainHero.PlayerControllers;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace jbzd.Items
{
    public class Item : MonoBehaviour , IInteractable
    {
        [field: SerializeField]
        public ItemSO ItemSO { get; set; }

        private InventoryController _inventoryController;

        [Inject]
        public void Construct(PlayerManager playerManager)
        {
            _inventoryController = playerManager.GetPlayerController<InventoryController>();
        }
        
        public virtual void OnInteract()
        {
            _inventoryController.PickUpItem(ItemSO);
            Destroy(gameObject);
        }
        
        [UsedImplicitly] // Factory pattern, nie inicjuje sie
        public class Factory : PlaceholderFactory<Object, Item>
        {
        }
    }
}