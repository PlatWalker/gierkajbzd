using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField]
    private Transform interactionTransform;

    public float radius = 2f;

    private bool isFocus = false;
    private Transform player;
    private bool hasInteracted = false;

    public virtual void Interact()
    {
        Debug.Log("interacted with " + transform.name);

    }

    private void Update()
    {
        if (isFocus && !hasInteracted) //to interact with an object you have to rpm it then press e
        {
            float distance = Vector3.Distance(player.position, interactionTransform.position);

            if (distance <= radius)
            {
                if (Input.GetKey(KeyCode.E))
                {
                    Interact();
                    hasInteracted = true;
                    OnDefocused();
                }
            }
        }
        else if (!hasInteracted) //or you can just press e if you are in the object radius
        {
            float distance = Vector3.Distance(player.position, interactionTransform.position);

            if (distance <= radius)
            {
                if (Input.GetKey(KeyCode.E))
                {
                    Interact();
                    hasInteracted = true;
                    OnDefocused();
                }
            }
        }
    }

    public void OnFocused(Transform playerTransform)    //when rpm'ed an object
    {
        isFocus = true;
        player = playerTransform;
        hasInteracted = false;
    }

    public void OnDefocused()   //when interacted with object you are losing its focus
    {
        isFocus = false;
        player = null;
        hasInteracted = false;
    }

    private void OnDrawGizmosSelected() //drawing gizmos (radius) of an object
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionTransform.position, radius);
    }


}
