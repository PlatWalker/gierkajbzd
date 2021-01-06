using System;
using UnityEngine;

/// <summary>
/// Napisane przez Sharashino
/// 
/// Główna klasa rzeczy z którymi możemy wchodzić w interakcję, obecnie przyciskiem E (Sklepy, skrzynie, lootowanie, interakcja z postaciami)
/// </summary>
public class Interactables : MonoBehaviour
{
    [SerializeField] private float radius = 3f;
    private bool isFocused = false;
    private Transform playerTransform;
    private bool hasInteracted;
    private float distance;

    private void Update()
    {
        if(playerTransform != null)
        {
            distance = Vector3.Distance(playerTransform.position, transform.position);

            if (distance <= radius)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Interact();
                }
            }
        }
    }

    public virtual void Interact()
    {
        
    }

    public void OnFocused()
    {
        isFocused = true;
        hasInteracted = false;
    }

    public void OnDefocused()
    {
        isFocused = false;
        playerTransform = null;
        hasInteracted = false;
    }

    private void OnTriggerEnter(Collider collider)
    {
        Transform interactorTransform = collider.gameObject.GetComponent<Clicker>().transform;

        if (interactorTransform != null)
        {
            playerTransform = interactorTransform.transform;
        }
        else
        {
            return;
        }
    }
}
