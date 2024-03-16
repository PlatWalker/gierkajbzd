using UnityEngine;
using UnityEngine.EventSystems;

namespace jbzd.UI.Inventory
{
    public class SortInventoryUIButton: MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private SortingType sortingType;
        
        private InventoryUIController _inventoryUIController;

        public void Awake()
        {
            _inventoryUIController = GetComponentInParent<InventoryUIController>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _inventoryUIController.SortBy(sortingType);
        }
    }
}