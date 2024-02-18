using jbzd.Items;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace jbzd.UI.Inventory
{
    public class InventoryItemUI : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        public ItemSO item;
        public int stackSize;
        public Image image;

        private Transform _parentAfterDrag;

        public delegate void OnSlotChanged();
        public event  OnSlotChanged onSlotChanged;

        public void Awake() 
        {
            image = GetComponent<Image>();    
        }

        public void ShowItem(ItemSO itemm, int stackSizee)
        {
            item = itemm;
            stackSize = stackSizee;
            image.sprite = item.ItemIcon;
            TMP_Text text = transform.GetChild(0).GetComponent<TMP_Text>();
            text.text = stackSize == -1 ? "" : stackSize.ToString();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            image.raycastTarget = false;
            _parentAfterDrag = transform.parent;
            transform.SetParent(transform.parent.parent);
            transform.SetAsLastSibling();
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            image.raycastTarget = true;
            transform.SetParent(_parentAfterDrag);
        }

        public void ChangeParent(Transform newParent)
        {
            _parentAfterDrag = newParent;
            onSlotChanged?.Invoke();
        }
    }
}
