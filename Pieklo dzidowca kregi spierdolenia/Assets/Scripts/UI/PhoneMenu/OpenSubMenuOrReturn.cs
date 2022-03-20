using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenSubMenuOrReturn : MonoBehaviour
{
    [SerializeField]
    public GameObject canvasToOpen;
    private GameObject canvasToClose;

    private void Start()
    {
        canvasToClose = transform.parent.gameObject;
        gameObject.GetComponent<Button>().onClick.AddListener(OpenCloseCanvas);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && canvasToClose.activeSelf)
        {
            canvasToClose.SetActive(!canvasToClose.activeSelf);
        }
    }
    private void OpenCloseCanvas()
    {
        canvasToClose.SetActive(!canvasToClose.activeSelf);
        canvasToOpen.SetActive(!canvasToOpen.activeSelf);
    }
}
