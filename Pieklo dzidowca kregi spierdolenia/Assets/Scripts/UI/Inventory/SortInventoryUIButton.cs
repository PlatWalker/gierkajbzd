using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using jbzd.Scenes.Prefabs.UI.Inventory.Elements.Scripts;

namespace jbzd.UI.Inventory
{
    public class SortInventoryUIButton: MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private SortingType sortingType;

        [SerializeField]
        private Sprite SortingLabelActivatedSprite;
        
        [SerializeField]
        private ScrollbarSlider _scrollbarSlider;
        private InventoryUIController _inventoryUIController;


        public void Awake()
        {
            _inventoryUIController = GetComponentInParent<InventoryUIController>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _inventoryUIController.SortBy(sortingType);
            gameObject.transform.parent.GetComponent<Image>().sprite = SortingLabelActivatedSprite;
            _scrollbarSlider.RestartValue();
        }
    }
}