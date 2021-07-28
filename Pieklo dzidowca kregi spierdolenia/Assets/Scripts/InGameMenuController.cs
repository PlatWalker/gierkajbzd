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

        public void QuitToMainMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(0);
        }

        public void PauseGame()
        {
            Time.timeScale = 0f;
            GameManager.Instance.PlayerObject.GetComponent<PlayerController>().CanPlayerMove = false;
            panel.SetActive(true);
        }

        public void ResumeGame()
        {
            Time.timeScale = 1f;
            GameManager.Instance.PlayerObject.GetComponent<PlayerController>().CanPlayerMove = true;
            panel.SetActive(false);
        }

        private void Update()
        {
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
