using System;
using UnityEngine;
using jbzdy.Inventory.SaveLoad;
using UnityEngine.SceneManagement;

namespace jbzdy.Inventory
{
    [Obsolete("Sharashino skrypt")]
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