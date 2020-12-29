using UnityEngine;

/// <summary>
/// Made by Sharashino
/// </summary>
public class Clicker : MonoBehaviour
{
    [SerializeField] private ItemInteraction playerInteraction;
    [SerializeField] private PlayerStats playerStats;


    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Interactable interactable = hit.collider.GetComponent<Interactable>();

                if (interactable != null)
                {

                }
            }
        }
    }
}
