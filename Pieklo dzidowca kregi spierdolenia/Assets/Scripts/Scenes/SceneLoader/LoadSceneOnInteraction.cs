using jbzd.Common.Interfaces;
using UnityEngine;

namespace jbzd.Scenes.SceneLoader
{
    public class LoadSceneOnInteraction: MonoBehaviour, IInteractable
    {
        private SceneLoader _sceneLoader;

        private void Awake()
        {
            _sceneLoader = GetComponent<SceneLoader>();
        }

        public void OnInteract()
        {
            _sceneLoader.LoadScene();
        }
    }
}