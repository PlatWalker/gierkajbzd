using UnityEngine;
using UnityEngine.SceneManagement;

namespace jbzd.UI.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        public void PlayGame()
        {
            SceneManager.LoadScene("Gameplay_stuff", LoadSceneMode.Additive);
            SceneManager.LoadScene("Level1Triggers", LoadSceneMode.Additive);
            SceneManager.LoadScene("Level_1_Smaller", LoadSceneMode.Additive);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (arg0.name != "Level_1_Smaller") return;
            
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("Level_1_Smaller"));
            SceneManager.UnloadSceneAsync("Main Menu", UnloadSceneOptions.None);

            if (FindObjectOfType<GameManager>() != null)
            {
                GameManager.Instance.Init();
            }
        }

        public void QuitGame()
        {
            Application.Quit();
        }

    } 
}
