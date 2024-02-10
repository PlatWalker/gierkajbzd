using jbzd.Common.InputSystem;
using jbzd.Common.InputSystem.Inputs;
using jbzd.MainHero;
using UnityEngine.SceneManagement;
using UnityEngine;
using Zenject;

namespace jbzd.UI.InGameMenu
{
    public class EscapeMenuController : UserInterfaceController
    {
        public override void ConnectInputToHandler(UserInterfaceInput input)
        {
            input.OnEscapeClick += ShowOrHideMenu;
            
        }
        public override bool InitialActivationState() => true;
        
        [SerializeField]
        private GameObject panel;

        private float _savedTimeScale;

        private UserInterfaceInput _inputController;

        [Inject]
        public void Construct(
            PlayerManager playerController,
            InputManager inputManager)
        {
            _inputController = inputManager.GetInput<UserInterfaceInput>();
        }

        public void QuitToMainMenu()
        {
            Time.timeScale = _savedTimeScale;
            SceneManager.LoadScene(0);
        }
        public void ShowOrHideMenu()
        {
            if (panel.activeSelf)
            {
                Time.timeScale = _savedTimeScale;
                panel.SetActive(false);
            }
            else
            {
                _savedTimeScale = Time.timeScale;
                Time.timeScale = 0f;
                panel.SetActive(true);
            }
        }
        
    } 
}
