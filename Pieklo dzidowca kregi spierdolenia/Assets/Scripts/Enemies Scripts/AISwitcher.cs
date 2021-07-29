///<summary>
///Created by Kumdzio
///</summary>
using UnityEngine;
namespace jbzdy.Enemies
{
    public class AISwitcher : MonoBehaviour
    {
        [SerializeField] private EnemyController enemyController = null;
        [SerializeField] private Material material1 = null;
        [SerializeField] private Material material2 = null;
        [SerializeField] private bool AIPausedAtStart = false;
        private bool shouldChange = false;
        private bool material = false;
        private Renderer mesh;

        void Start()
        {
            mesh = GetComponent<Renderer>();
            shouldChange = AIPausedAtStart;
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
                        ChangeState();
                    }
                }
            }
        }
        private void LateUpdate()
        {
            if (shouldChange)
            {
                ChangeState();
                shouldChange = false;
            }

        }

        private void ChangeState()
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

            if (enemyController != null) enemyController.SwitchAI();
        }
    }
}