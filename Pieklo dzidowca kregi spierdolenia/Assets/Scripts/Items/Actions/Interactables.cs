using UnityEngine;

/// <summary>
/// Napisane przez Sharashino
/// </summary>
public class Interactables : MonoBehaviour
{
    [SerializeField] private float radius = 3f;
    private bool isFocused = false;
    private Transform playerTransform;
    private bool hasInteracted;

    private void Update()
    {
        if (isFocused && !hasInteracted)
        {
            float distance = Vector3.Distance(playerTransform.position, transform.position);
            
            if(distance <= radius)
            {
                Interact();
                hasInteracted = true;
            }
        }
    }

    public virtual void Interact()
    {

    }

    public void OnFocused(Transform playerTransform)
    {
        isFocused = true;
        hasInteracted = false;
        this.playerTransform = playerTransform;
    }

    public void OnDefocused()
    {
        isFocused = false;
        playerTransform = null;
        hasInteracted = false;
    }
    public float GetRadius()
    {
        return radius;
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
