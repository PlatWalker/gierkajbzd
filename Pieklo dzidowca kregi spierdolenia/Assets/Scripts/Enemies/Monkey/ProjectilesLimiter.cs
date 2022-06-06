using UnityEngine;
/// <summary>
/// Created by Kumdzio
/// </summary>
namespace jbzdy.Enemies
{
    public class ProjectilesLimiter : MonoBehaviour
    {
        [SerializeField] private float timeToDestroy = 5.0f;
        private float counter = 0f;

        void Update()
        {
            counter += Time.deltaTime;
            if (counter >= timeToDestroy)
            {
                Destroy(this.gameObject);
            }
        }
    }
}