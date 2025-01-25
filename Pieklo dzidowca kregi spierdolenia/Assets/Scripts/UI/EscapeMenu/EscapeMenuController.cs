using System;
using System.Collections;
using jbzd.Common;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using jbzd.SavingSystem;
using System.Collections.Generic;
using jbzd.UI.LoadingScene;
using jbzd.Dialogues.RuntimeData;

namespace jbzd.UI.EscapeMenu
{
    public class EscapeMenuController : UserInterfaceController
    {
        private SaveManager _saveManager;
        private UserInterfaceManager _userInterfaceManager;
        
        [SerializeField]
        private List<GameObject> _buttonParents = new();
        [SerializeField]
        private GameObject _buttonParent;

        public override bool InitialActivationState() => false;

        [Inject]
        public void Constructor(SaveManager saveManager, UserInterfaceManager userInterfaceManager)
        {
            _saveManager = saveManager;
            _userInterfaceManager = userInterfaceManager;
        }

        private void OnEnable()
        {
            _buttonParents.ForEach(p => { p.SetActive(false); });
            _buttonParent.SetActive(true);
        }
        public void Resume()
        {
            FreezeTime.Unfreeze();
            gameObject.SetActive(false);
        }

        public void QuitToMainMenu()
        {
            FreezeTime.Unfreeze();
            ContainerSO.ResetDataGlobal();
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
        
        public void QuitGame()
        {
            FreezeTime.Unfreeze();
            ContainerSO.ResetDataGlobal();
            Application.Quit();
        }

        public void SaveGame()
        {
            _saveManager.SaveGame();
        }

        public void LoadGame()
        {
            StartCoroutine(Loading());
        }

        private IEnumerator Loading()
        {
            var loadingUI = _userInterfaceManager.GetUIController<LoadingUI>();
            loadingUI.ShowLoadingScreen();
            yield return _saveManager.LoadGame();
            loadingUI.HideLoadingScreen();
            gameObject.SetActive(false);
        }
    } 
}
