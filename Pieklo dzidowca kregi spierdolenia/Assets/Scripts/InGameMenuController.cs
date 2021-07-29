using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using jbzdy.Player;

namespace jbzdy.UI
{
    public class InGameMenuController : MonoBehaviour
    {
        [SerializeField]
        private GameObject panel;

        float savedTimeScale;

        public void QuitToMainMenu()
        {
            Time.timeScale = savedTimeScale;
            SceneManager.LoadScene(0);
        }

        public void PauseGame()
        {;
            savedTimeScale = Time.timeScale;
            Time.timeScale = 0f;
            GameManager.Instance.PlayerObject.GetComponent<PlayerController>().CanPlayerMove = false;
            panel.SetActive(true);
        }

        public void ResumeGame()
        {
            Time.timeScale = savedTimeScale;
            GameManager.Instance.PlayerObject.GetComponent<PlayerController>().CanPlayerMove = true;
            panel.SetActive(false);
        }

        private void Update()
        {
            // zrobić eventa, moze z zastosowaniem wzorca "observer"?
            if (GameManager.Instance.GameInputController.uIinputStatus.inGameMenu == true && panel.activeSelf == false)
            {
                PauseGame();

            }
            else if (GameManager.Instance.GameInputController.uIinputStatus.inGameMenu == true && panel.activeSelf == true)
            {
                ResumeGame();
            }
        }
    } 
}
