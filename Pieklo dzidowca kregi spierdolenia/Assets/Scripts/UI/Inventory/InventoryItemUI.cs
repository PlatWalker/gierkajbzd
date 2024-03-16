using jbzd.Items;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace jbzd.UI.Inventory
{
    public class InventoryItemUI : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
    {
        [SerializeField]
        private GameObject hoverTextContainer;

        [SerializeField]
        private TMP_Text stackCountText;

        [SerializeField]
        private TMP_Text itemNameText;
        
        [SerializeField]
        private TMP_Text itemDescriptionText;
        
        public ItemSO Item { get; private set; }
        public int StackSize { get; private set; }
        public Image Image { get; private set; }
        
        private InventorySlotUI _draggedFrom;
        private GameObject _objectHolderWhenDragged;
        
        public void Awake() 
        {
            Image = GetComponent<Image>();
        }

        public void Init(ItemSO item, int stackSize, GameObject draggedItemHolder)
        {
            Item = item;
            StackSize = stackSize;
            _objectHolderWhenDragged = draggedItemHolder;
            
            Image.sprite = Item.ItemIcon;
            stackCountText.text = StackSize == -1 ? "" : StackSize.ToString();

            itemNameText.text = Item.ItemName;
            itemDescriptionText.text = item.ItemDescription;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            hoverTextContainer.gameObject.SetActive(false);
            _draggedFrom = transform.parent.GetComponent<InventorySlotUI>();
            Image.raycastTarget = false;
            
            //gameObject.transform.parent.parent.parent.parent
            transform.SetParent(_objectHolderWhenDragged.transform);
            transform.SetAsLastSibling();
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!IsTargetValidInventorySlot(out var targetInventorySlot) ||
                !targetInventorySlot.TryAssignToSlot(this))
            {
                var parentRectTransform = _draggedFrom.GetComponent<RectTransform>();
                var childRectTransform = GetComponent<RectTransform>();
                SnapMiddlePointOfChildToMiddlePointOfParent(parentRectTransform, childRectTransform);
                
                transform.SetParent(_draggedFrom.transform);
                Image.raycastTarget = true;
                
                return;
            }
            
            var parentRectTransform1 = targetInventorySlot.GetComponent<RectTransform>();
            var childRectTransform1 = GetComponent<RectTransform>();
            SnapMiddlePointOfChildToMiddlePointOfParent(parentRectTransform1, childRectTransform1);
            
            transform.SetParent(targetInventorySlot.transform);
            Image.raycastTarget = true;
            _draggedFrom.RemoveItem();
            
            return;

            bool IsTargetValidInventorySlot(out InventorySlotUI inventorySlotOut) => 
                !(inventorySlotOut = null) && eventData.pointerEnter is not null && eventData.pointerEnter.TryGetComponent(out inventorySlotOut);

            void SnapMiddlePointOfChildToMiddlePointOfParent(RectTransform parentRectTransform, RectTransform childRectTransform)
            {
                var parentCenterPosition = new Vector2(parentRectTransform.rect.width * 0.5f, parentRectTransform.rect.height * 0.5f);
                var childCenterPosition = new Vector2(childRectTransform.rect.width * 0.5f, childRectTransform.rect.height * 0.5f);
                var offset = parentCenterPosition - childCenterPosition;
                childRectTransform.position = parentRectTransform.TransformPoint(offset);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (eventData.pointerDrag is not null) return;
            
            hoverTextContainer.gameObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (eventData.pointerDrag is not null) return;
            
            hoverTextContainer.gameObject.SetActive(false);
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            if (eventData.pointerDrag is not null) return;
            
            hoverTextContainer.transform.position = Input.mousePosition;
        }
    }
}
