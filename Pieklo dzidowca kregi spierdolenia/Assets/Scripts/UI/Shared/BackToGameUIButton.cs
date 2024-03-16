using jbzd.Common;
using UnityEngine;
using UnityEngine.EventSystems;

namespace jbzd.UI.Shared
{
    public class BackToGameUIButton: MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            GetComponentInParent<UserInterfaceController>().gameObject.SetActive(false);
            FreezeTime.Unfreeze();
        }
    }
}