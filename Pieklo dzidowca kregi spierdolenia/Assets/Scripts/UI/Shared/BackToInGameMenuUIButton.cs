using jbzd.UI.InGameMenu;
using UnityEngine;
using UnityEngine.EventSystems;

namespace jbzd.UI.Shared
{
    public class BackToInGameMenuUIButton: MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            GetComponentInParent<UserInterfaceController>().gameObject.SetActive(false);
            GetComponentInParent<UserInterfaceManager>().GetUIController<InGameMenuUIController>().gameObject.SetActive(true);
        }
    }
}