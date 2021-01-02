using System;
using UnityEngine;

/// <summary>
/// Made by Sharashino
/// </summary>
public class Clicker : MonoBehaviour
{
    [SerializeField] private Interactables focus = null;

    void Update()
    {
        if(Input.GetMouseButtonDown(0)) //Left click
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Interactables interactable = hit.collider.GetComponent<Interactables>();

                if (interactable != null)
                {
                    
                    //Focus player on object
                    float intRadius = interactable.GetRadius();

                    SetFocus(interactable);
                }
                else if(interactable == null)
                {
                    RemoveFocus();
                }
            }
        }
    }

    private void SetFocus(Interactables newFocus)
    {
        if(newFocus != focus)
        {
            if(focus != null)
            {
                focus.OnDefocused();
            }

            focus = newFocus;
        }

        newFocus.OnFocused(transform);
    } 

    private void RemoveFocus()
    {
        if (focus != null)
        {
            focus.OnDefocused();
        }

        focus = null;
    }
}
