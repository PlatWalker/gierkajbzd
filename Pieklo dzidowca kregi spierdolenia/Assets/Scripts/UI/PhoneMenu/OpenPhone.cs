using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPhone : MonoBehaviour
{
    private GameObject phone;
    private bool phoneIsEnabled;

    private void Start()    
    {
        phone = transform.GetChild(0).gameObject;
        phone.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            phone.SetActive(!phone.activeSelf);
        }
    }
}
