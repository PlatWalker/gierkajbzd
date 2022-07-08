using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace jbzdy.UI
{
    public class MainMenu : MonoBehaviour
    {
        public void PlayGame()
        {
            SceneManager.LoadScene("Gameplay_stuff", LoadSceneMode.Additive);
            SceneManager.LoadScene("Level1Triggers", LoadSceneMode.Additive);
            SceneManager.LoadScene("mapa lvl 1", LoadSceneMode.Additive);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (arg0.name != "mapa lvl 1") return;
            
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("mapa lvl 1"));
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
