using jbzd.Common.InputSystem;
using jbzd.Common.InputSystem.Inputs;
using jbzd.UI.NewUI;
using UnityEngine;
using Zenject;

namespace jbzd.UI.PhoneMenu
{
    public class OpenPhone : UserInterfaceController
    {
        private GameObject _phone;
        private bool _phoneIsEnabled;
    
        private UserInterfaceInput _inputController;
        
        [Inject]
        public void Construct(InputManager inputManager)
        {
            _inputController = inputManager.GetInput<UserInterfaceInput>();
        }
    
        private void Start()    
        {
            _phone = transform.GetChild(0).gameObject;
            _phone.SetActive(false);
            _inputController.OnPlayerMenuOpened += OpenPhoneView;
        }

        private void OpenPhoneView() => _phone.SetActive(!_phone.activeSelf);
    }
}
