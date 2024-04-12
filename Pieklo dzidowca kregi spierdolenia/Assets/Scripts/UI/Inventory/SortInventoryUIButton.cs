using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace jbzd.UI.Inventory
{
    public class SortInventoryUIButton: MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private SortingType sortingType;

        [SerializeField]
        private Sprite SortingLabelActivatedSprite;
        
        private InventoryUIController _inventoryUIController;

        public void Awake()
        {
            _inventoryUIController = GetComponentInParent<InventoryUIController>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _inventoryUIController.SortBy(sortingType);
            gameObject.transform.parent.GetComponent<Image>().sprite = SortingLabelActivatedSprite;
        }
    }
}