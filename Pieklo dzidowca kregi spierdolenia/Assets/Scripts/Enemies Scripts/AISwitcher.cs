///<summary>
///Created by Kumdzio
///</summary>
using UnityEngine;

public class AISwitcher : MonoBehaviour
{
    [SerializeField] private EnemyController enemyController;
    [SerializeField] private Material material1;
    [SerializeField] private Material material2;

    private bool material = false;
    private Renderer mesh;
    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<Renderer>();
    }

    // Update is called once per frame
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
