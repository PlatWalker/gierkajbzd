using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using jbzd.MainHero;
using jbzd.SavingSystem;
using Zenject;

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
        private Animation _animation;
        [Inject] private SaveManager _saveManager;
        public void Start()
        {
            _animation = _fadeIn.GetComponent<Animation>();
        }

        public void PlayGame()
        {
            var directoryPath = Path.Combine(Application.persistentDataPath, SaveManager.SAVE_DIRECTORY_NAME);
            var filePath = Path.Combine(directoryPath, SaveManager.SAVE_FILE_NAME);
            
            if (File.Exists(filePath))
            {
                Debug.Log($"Deleted previous save file from path: {filePath}");
                File.Delete(filePath);
            }
            
            _fadeIn.SetActive(true);
            eventSystem.SetActive(false);
            mainCamera.SetActive(false);
            StartCoroutine(LoadScenesAndTeleportPlayer());
        }

        public void LoadGame()
        {
            StartCoroutine(StartLoadingGame());
        }

        private IEnumerator StartLoadingGame()
        {
            _fadeIn.SetActive(true);
            eventSystem.SetActive(false);
            mainCamera.SetActive(false);
            _animation.Play("FadeIn");
            yield return new WaitForSeconds(1.5f);
            Canvas.SetActive(false);
            yield return SceneManager.LoadSceneAsync("(none) - (GameplayStuff) - (SingleLoad)", LoadSceneMode.Additive);
            _saveManager.LoadGame();
            yield return new WaitForSeconds(2f);
            yield return SceneManager.UnloadSceneAsync("(none) - (Main Menu) - (SingleLoad)");
            _animation.Play("FadeIn");
        }

        private IEnumerator LoadScenesAndTeleportPlayer()
        {
            _fadeIn.SetActive(true);
            _animation.Play("FadeIn");
            yield return new WaitForSeconds(1);
            _intro.SetActive(true);
            _fadeIn.SetActive(true);
            yield return new WaitForSeconds(5);
            
            yield return SceneManager.LoadSceneAsync("(none) - (GameplayStuff) - (SingleLoad)", LoadSceneMode.Additive);
            yield return SceneManager.LoadSceneAsync("(Krag1) - (AnonFlat) - (Interactive)", LoadSceneMode.Additive);
            yield return SceneManager.LoadSceneAsync("(Krag1) - (AnonFlat) - (Passive)", LoadSceneMode.Additive);
            
            _playerManager = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();
            var teleportDestination = GameObject.Find("PlayStartPosition");
            _playerManager.PlaceAt(teleportDestination.transform.position);
            

            _animation.Play("FadeIn");
            yield return new WaitForSeconds(1);
            Canvas.SetActive(false);
            _intro.SetActive(false);
            yield return new WaitForSeconds(1.5f);
            yield return SceneManager.UnloadSceneAsync("(none) - (Main Menu) - (SingleLoad)");
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
