using UnityEngine;
using jbzdy.Inventory.SaveLoad;
using UnityEngine.SceneManagement;

namespace jbzdy.Inventory
{
    public class LevelTransition : MonoBehaviour
    {
        public int sceneId;

        private void OnTriggerEnter(Collider other)
        {
            FindObjectOfType<SaveData>().SaveLevelPeristence();
            SceneManager.LoadScene(sceneId);
        }
    }
}