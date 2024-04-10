using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using jbzd.MainHero;
using TheKiwiCoder;

namespace jbzd.UI.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        public GameObject eventSystem;
        public GameObject mainCamera;
        public GameObject Canvas;
        private PlayerManager _playerManager;
        [SerializeField] private GameObject _fadeIn;
        [SerializeField] private GameObject _intro;
        private Animation animation;

        public void Start()
        {
            animation = _fadeIn.GetComponent<Animation>();
        }

        public void PlayGame()
        {
            _fadeIn.SetActive(true);
            eventSystem.SetActive(false);
            mainCamera.SetActive(false);
            StartCoroutine(LoadScenesAndTeleportPlayer());
        }

        private IEnumerator LoadScenesAndTeleportPlayer()
        {
            _fadeIn.SetActive(true);
            animation.Play("FadeIn");
            yield return new WaitForSeconds(1);
            _intro.SetActive(true);
            _fadeIn.SetActive(true);
            yield return new WaitForSeconds(5);
            
            yield return SceneManager.LoadSceneAsync("(none) - (GameplayStuff) - (SingleLoad)", LoadSceneMode.Additive);
            yield return SceneManager.LoadSceneAsync("(Krag1) - (AnonFlat) - (Interactive)", LoadSceneMode.Additive);
            yield return SceneManager.LoadSceneAsync("(Krag1) - (AnonFlat) - (Passive)", LoadSceneMode.Additive);
            
            _playerManager = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();
            GameObject teleportDestination = GameObject.Find("TeleportDestination");
            _playerManager.PlaceAt(teleportDestination.transform.position);
            

            animation.Play("FadeIn");
            yield return new WaitForSeconds(1);
            Canvas.SetActive(false);
            _intro.SetActive(false);
            yield return new WaitForSeconds(1.5f);
            yield return SceneManager.UnloadScene("(none) - (Main Menu) - (SingleLoad)");
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
