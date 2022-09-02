using System.Collections;
using System.Collections.Generic;
using jbzd.Common.InputSystem;
using UnityEngine;

public class OpenPhone : MonoBehaviour
{
    private GameObject phone;
    private bool phoneIsEnabled;

    private void Start()    
    {
        phone = transform.GetChild(0).gameObject;
        phone.SetActive(false);
        GameManager.Instance.GameInputController.GetInput<UserInterfaceInput>().OnPlayerMenuOpened += OpenPhoneView;
    }

    private void OpenPhoneView() => phone.SetActive(!phone.activeSelf);
}
