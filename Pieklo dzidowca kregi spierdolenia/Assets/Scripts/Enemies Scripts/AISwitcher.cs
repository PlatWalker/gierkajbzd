///<summary>
///Created by Kumdzio
///</summary>
using UnityEngine;

public class AISwitcher : MonoBehaviour
{
    [SerializeField] private EnemyController enemyController=null;
    [SerializeField] private Material material1=null;
    [SerializeField] private Material material2=null;
    private bool material = false;
    private Renderer mesh;

    void Start()
    {
        mesh = GetComponent<Renderer>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform)
                {
                    if (!material)
                    {
                        mesh.material = material1;
                    }
                    else
                    {
                        mesh.material = material2;
                    }
                    material = !material;

                    if(enemyController!= null) enemyController.SwitchAI();
                }
            }
        }
    }
}
