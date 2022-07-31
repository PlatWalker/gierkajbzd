using jbzdy.DialogueSystem.NodeDatas;
using jbzdy.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace jbzdy.Dialogues
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Dialogue/New Scene Transition Event", fileName = "New Scene Transition Event")]
    public class SceneChangeEvent : DialogueEventSO
    {
        [SerializeField] private string _sceneToUnload;
        [SerializeField] private string _sceneToLoad;
        [SerializeField] private Vector3 _playerPlacementOnLoadedScene;
        
        public override void RunEvent()
        {
            base.RunEvent();
            ChangeSceneEvent();
        }

        public override void RunEvent(Object someObject)
        {
            RunEvent();
        }

        private void ChangeSceneEvent()
        {
            GameManager.Instance.UIControllerInstance.ScreenFadeOut();
            var a = SceneManager.UnloadSceneAsync(_sceneToUnload);
            a.completed += LoadScene;
        }
        
        private void LoadScene(AsyncOperation obj)
        {
            SceneManager.LoadScene(_sceneToLoad, LoadSceneMode.Additive);
            GameManager.Instance.UIControllerInstance.ScreenFadeIn();

            if (_playerPlacementOnLoadedScene != default)
            {
                var playerController = GameManager.Instance.PlayerObject.GetComponent<PlayerController>();
                playerController.PlaceAt(_playerPlacementOnLoadedScene);
            }
        }
    }
}
