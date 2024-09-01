using jbzd.Common;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using jbzd.SavingSystem;
using System.Collections.Generic;
using jbzd.Dialogues.RuntimeData;

namespace jbzd.UI.EscapeMenu
{
    public class EscapeMenuController : UserInterfaceController
    {
        private SaveManager _saveManager;
        [SerializeField]
        private List<GameObject> _buttonParents = new();
        [SerializeField]
        private GameObject _buttonParent;

        public override bool InitialActivationState() => false;

        [Inject]
        public void Constructor(SaveManager saveManager)
        {
            _saveManager = saveManager;
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
            _saveManager.LoadGame();
        }
    } 
}
