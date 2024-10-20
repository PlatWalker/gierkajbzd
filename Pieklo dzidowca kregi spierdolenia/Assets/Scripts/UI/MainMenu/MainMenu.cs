using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using jbzd.MainHero;
using UnityEngine.UI;
using jbzd.SavingSystem;
using Zenject;

namespace jbzd.UI.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        public GameObject eventSystem;
        public GameObject Canvas;
        private PlayerManager _playerManager;
        [SerializeField] private GameObject _fadeIn;
        [SerializeField] private GameObject _intro;
        [SerializeField] private GameObject _loadText;

        private Animation _animation;
        private Animator _introAnimation;

        [Inject] private SaveManager _saveManager;
        [SerializeField] private GameObject wczytajGreGameObject;
        public void Start()
        {
            var directoryPath = Path.Combine(Application.persistentDataPath, SaveManager.SAVE_DIRECTORY_NAME);
            var filePath = Path.Combine(directoryPath, SaveManager.SAVE_FILE_NAME);
            wczytajGreGameObject.SetActive(File.Exists(filePath));

            _animation = _fadeIn.GetComponent<Animation>();
            _introAnimation = _intro.GetComponent<Animator>();
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
            _animation.Play("FadeIn");
            yield return new WaitForSeconds(1.5f);
            Canvas.SetActive(false);
            yield return SceneManager.LoadSceneAsync("(none) - (GameplayStuff) - (SingleLoad)", LoadSceneMode.Additive);
            yield return _saveManager.LoadGame();
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

            AsyncOperation loadScene1 = SceneManager.LoadSceneAsync("(none) - (GameplayStuff) - (SingleLoad)", LoadSceneMode.Additive);
            AsyncOperation loadScene2 = SceneManager.LoadSceneAsync("(Krag1) - (AnonFlat) - (Interactive)", LoadSceneMode.Additive);
            AsyncOperation loadScene3 = SceneManager.LoadSceneAsync("(Krag1) - (AnonFlat) - (Passive)", LoadSceneMode.Additive);

            bool scenesLoaded = false;

            while (!scenesLoaded)
            {
                if (loadScene1.isDone && loadScene2.isDone && loadScene3.isDone) { 
                    _loadText.SetActive(true);

                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        _intro.GetComponent<PostWwiseEvent>().StopEvent();
                        scenesLoaded = true;
                        break;
                    }

                    if (!_introAnimation.GetCurrentAnimatorStateInfo(0).IsName("Intro_anim"))
                    {
                        _intro.GetComponent<Image>().color = Color.black;
                        scenesLoaded = true;
                        break;
                    }
                }

                yield return null;
            }

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
