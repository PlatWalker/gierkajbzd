using UnityEngine;
using UnityEngine.EventSystems;

namespace jbzd.UI.InGameMenu
{
    public class OpenUIButton: MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private UserInterfaceController uiToOpen; 
        
        public void OnPointerClick(PointerEventData eventData)
        {
            uiToOpen.gameObject.SetActive(true);
            GetComponentInParent<UserInterfaceController>().gameObject.SetActive(false);
        }
    }
}