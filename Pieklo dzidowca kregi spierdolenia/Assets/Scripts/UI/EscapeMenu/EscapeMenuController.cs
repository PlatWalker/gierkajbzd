using jbzd.Common;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using jbzd.SavingSystem;
using System.Collections.Generic;
using jbzd.UI.LoadingScene;

namespace jbzd.UI.EscapeMenu
{
    public class EscapeMenuController : UserInterfaceController
    {
        private SaveManager _saveManager;
        private LoadingUI _loadingUI;
        
        [SerializeField]
        private List<GameObject> _buttonParents = new();
        [SerializeField]
        private GameObject _buttonParent;

        public override bool InitialActivationState() => false;

        [Inject]
        public void Constructor(SaveManager saveManager, UserInterfaceManager userInterfaceManager)
        {
            _saveManager = saveManager;
            _loadingUI = userInterfaceManager.GetUIController<LoadingUI>();
        }

        private void OnEnable()
        {
            _buttonParents.ForEach(p => { p.SetActive(false); });
            _buttonParent.SetActive(true);
        }

        public void QuitToMainMenu()
        {
            FreezeTime.Unfreeze();
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
        
        public void Resume()
        {
            gameObject.SetActive(false);
            FreezeTime.Unfreeze();
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void SaveGame()
        {
            _saveManager.SaveGame();
        }

        public void LoadGame()
        {
            gameObject.SetActive(false);
            StartCoroutine(_saveManager.LoadGame());
        }
    } 
}
