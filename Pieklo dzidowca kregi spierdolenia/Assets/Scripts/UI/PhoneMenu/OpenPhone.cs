using System.Collections;
using System.Collections.Generic;
using jbzd.Common.InputSystem;
using jbzd.Common.InputSystem.Inputs;
using UnityEngine;
using Zenject;

public class OpenPhone : MonoBehaviour
{
    private GameObject phone;
    private bool phoneIsEnabled;
    
    private UserInterfaceInput _inputController;
        
    [Inject]
    public void Construct(InputController inputController)
    {
        _inputController = inputController.GetInput<UserInterfaceInput>();
    }
    
    private void Start()    
    {
        phone = transform.GetChild(0).gameObject;
        phone.SetActive(false);
        _inputController.OnPlayerMenuOpened += OpenPhoneView;
    }

    private void OpenPhoneView() => phone.SetActive(!phone.activeSelf);
}
