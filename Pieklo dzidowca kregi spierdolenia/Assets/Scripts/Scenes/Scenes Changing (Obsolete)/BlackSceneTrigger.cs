using System;
using jbzd.MainHero;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace jbzd.Scenes.Scenes_Changing__Obsolete_
{
    [Obsolete]
    public class BlackSceneTrigger : MonoBehaviour
    {
        [SerializeField] private string _sceneToUnload;
        [SerializeField] private string _sceneToLoad;
        [SerializeField] private Vector3 _playerPlacementOnLoadedScene;
        
        private PlayerManager _playerController;
        
        [Inject]
        public void Construct(PlayerManager playerController)
        {
            _playerController = playerController;
        }
        
        private void Awake()
        {
            if (_sceneToUnload == default) Debug.Log("You forget add name of scene to unload!");
            if (_sceneToLoad == default) Debug.Log("You forget add name of scene to load!");
        }

        private void OnTriggerEnter(Collider other)
        {
            //GameManager.Instance.UIControllerInstance.ScreenFadeOut();
            var a = SceneManager.UnloadSceneAsync(_sceneToUnload);
            a.completed += LoadScene;
        }

        private void LoadScene(AsyncOperation obj)
        {
            SceneManager.LoadScene(_sceneToLoad, LoadSceneMode.Additive);
            GetComponent<Collider>().enabled = false;
            //GameManager.Instance.UIControllerInstance.ScreenFadeIn();

            if (_playerPlacementOnLoadedScene != default)
            {
                _playerController.PlaceAt(_playerPlacementOnLoadedScene);
            }
        }
    }
}