using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace jbzd.Common
{
    
    public class SetThisAsActiveScene : MonoBehaviour
    {
        public void Awake()
        {
            SceneManager.sceneLoaded += SetActiveThisScene;
        }

        private void SetActiveThisScene(Scene arg0, LoadSceneMode arg1)
        {
            if (arg0.Equals(gameObject.scene)) SceneManager.SetActiveScene(gameObject.scene);
        }
    }
}
