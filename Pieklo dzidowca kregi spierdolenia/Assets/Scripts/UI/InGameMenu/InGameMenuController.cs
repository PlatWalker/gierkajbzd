using System;
using System.Collections;
using System.Collections.Generic;
using jbzd;
using jbzd.Common.InputSystem;
using jbzd.Common.InputSystem.Inputs;
using UnityEngine.SceneManagement;
using UnityEngine;
using jbzdy.Player;
using Zenject;

namespace jbzdy.UI
{
    public class InGameMenuController : MonoBehaviour
    {
        [SerializeField]
        private GameObject panel;

        private float _savedTimeScale;

        private PlayerController _playerController;
        private UserInterfaceInput _inputController;

        [Inject]
        public void Construct(
            PlayerController playerController,
            InputController inputController)
        {
            _inputController = inputController.GetInput<UserInterfaceInput>();
            _playerController = playerController;
        }
        
        public void Start()
        {
            _inputController.OnInGameMenuOpened += ShowOrHideMenu;
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
