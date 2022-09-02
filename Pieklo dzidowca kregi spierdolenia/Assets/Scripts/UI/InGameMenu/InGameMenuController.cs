using System;
using System.Collections;
using System.Collections.Generic;
using jbzd;
using jbzd.Common.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine;
using jbzdy.Player;

namespace jbzdy.UI
{
    public class InGameMenuController : MonoBehaviour
    {
        [SerializeField]
        private GameObject panel;

        private float _savedTimeScale;

        private UserInterfaceInput _uiInput;
        private PlayerController _playerController;
        
        public void Start()
        {
            _uiInput = GameManager.Instance.GameInputController.GetInput<UserInterfaceInput>();
            _playerController = GameManager.Instance.PlayerObject.GetComponent<PlayerController>();
            
            _uiInput.OnInGameMenuOpened += ShowOrHideMenu;
        }

        public void QuitToMainMenu()
        {
            Time.timeScale = _savedTimeScale;
            SceneManager.LoadScene(0);
        }

        private void ShowOrHideMenu()
        {
            if (panel.activeSelf)
            {
                Time.timeScale = _savedTimeScale;
                _playerController.CanPlayerMove = true;
                panel.SetActive(false);
            }
            else
            {
                _savedTimeScale = Time.timeScale;
                Time.timeScale = 0f;
                _playerController.CanPlayerMove = false;
                panel.SetActive(true);
            }
        }
    } 
}
