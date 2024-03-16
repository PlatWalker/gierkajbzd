using System;
using jbzd.Common;
using jbzd.MainHero;
using jbzd.Scenes.SceneLoader;
using UnityEngine.SceneManagement;
using Zenject;

namespace jbzd.UI.EscapeMenu
{
    public class EscapeMenuController : UserInterfaceController
    {
        public override bool InitialActivationState() => false;
        
        public void QuitToMainMenu()
        {
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
        
        public void Resume()
        {
            gameObject.SetActive(false);
            FreezeTime.Unfreeze();
        }
    } 
}
