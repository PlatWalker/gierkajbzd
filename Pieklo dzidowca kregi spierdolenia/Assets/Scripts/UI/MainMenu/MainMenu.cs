using UnityEngine;
using UnityEngine.SceneManagement;

namespace jbzd.UI.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        public GameObject eventSystem;
        public GameObject mainCamera;

        public void PlayGame()
        {
            eventSystem.SetActive(false);
            mainCamera.SetActive(false);
            
            SceneManager.LoadSceneAsync("(none) - (GameplayStuff) - (SingleLoad)", LoadSceneMode.Additive);
            SceneManager.sceneLoaded += SceneLoaded;
        }

        private void SceneLoaded(Scene arg0, LoadSceneMode loadSceneMode)
        {
            SceneManager.UnloadSceneAsync("(none) - (Main Menu) - (SingleLoad)");
            SceneManager.LoadSceneAsync("(Krag1) - (MainScene) - (Passive)", LoadSceneMode.Additive);
            SceneManager.LoadSceneAsync("(Krag1) - (MainScene) - (Interactive)", LoadSceneMode.Additive);

            SceneManager.sceneLoaded -= SceneLoaded;
        }

        public void QuitGame()
        {
            Application.Quit();
        }

    } 
}
