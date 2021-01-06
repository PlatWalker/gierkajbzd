///<summary>
///Created by Kumdzio
///</summary>
using UnityEngine;

public class AISwitcher : MonoBehaviour
{
    [SerializeField] EnemyController enemyController;
    [SerializeField] Material material1;
    [SerializeField] Material material2;

    bool material = false;
    Renderer mesh;
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
                if (hit.transform.name == gameObject.transform.name)
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
