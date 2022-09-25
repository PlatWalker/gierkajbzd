using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace jbzd.UI
{
    public abstract class OpenAndCloseUserInterface : MonoBehaviour
    {
        protected abstract UserInterfaceController UserInterfaceToOpen { get; set; }
        protected abstract UserInterfaceController UserInterfaceToClose { get; set; }

        private UserInterfaceManager _userInterfaceManager;
        
        [Inject]
        public void Construct(UserInterfaceManager userInterfaceManager)
        {
            _userInterfaceManager = userInterfaceManager;
        }

        protected abstract void AssignPropertyUserInterfacesOpenAndClose(UserInterfaceManager userInterfaceManager);
        
        private void Start()
        {
            if (!TryGetComponent(out Button component)) 
                Debug.LogError($"gameobject z skryptem {typeof(OpenAndCloseUserInterface)} musi miec component z buttonem!"); 

            component.onClick.AddListener(OpenCloseUserInterfaces);
            AssignPropertyUserInterfacesOpenAndClose(_userInterfaceManager);
        }

        private void OpenCloseUserInterfaces()
        {
            UserInterfaceToClose.gameObject.SetActive(false);
            UserInterfaceToOpen.gameObject.SetActive(true);
        }
    }
}
