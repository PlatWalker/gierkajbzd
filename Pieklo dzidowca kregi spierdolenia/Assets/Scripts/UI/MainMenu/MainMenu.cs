using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using jbzd.MainHero;
namespace jbzd.UI.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        public GameObject eventSystem;
        public GameObject mainCamera;
        private PlayerManager _playerManager;

        public void PlayGame()
        {
            eventSystem.SetActive(false);
            mainCamera.SetActive(false);
            StartCoroutine(LoadScenesAndTeleportPlayer());
        }

        private IEnumerator LoadScenesAndTeleportPlayer()
        {
            yield return SceneManager.LoadSceneAsync("(none) - (GameplayStuff) - (SingleLoad)", LoadSceneMode.Additive);
            yield return SceneManager.LoadSceneAsync("(Krag1) - (AnonFlat) - (Interactive)", LoadSceneMode.Additive);
            yield return SceneManager.LoadSceneAsync("(Krag1) - (AnonFlat) - (Passive)", LoadSceneMode.Additive);

            SceneManager.UnloadSceneAsync("(none) - (Main Menu) - (SingleLoad)");

            _playerManager = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();
            GameObject teleportDestination = GameObject.Find("TeleportDestination");
            _playerManager.PlaceAt(teleportDestination.transform.position);

        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
