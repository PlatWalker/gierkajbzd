using jbzd.Common;
using UnityEngine;

namespace jbzd.UI.LoadingScene
{
    public class LoadingUI : UserInterfaceController
    {
        public void ShowLoadingScreen()
        {
            gameObject.SetActive(true);
            FreezeTime.Freeze();
        }

        public void HideLoadingScreen(AsyncOperation asyncOperation)
        {
            Hide();
            asyncOperation.completed -= HideLoadingScreen;
        }
        
        public void HideLoadingScreen()
        {
            Hide();
        }

        private void Hide()
        {
            gameObject.SetActive(false);
            FreezeTime.Unfreeze();
        }

        public override bool InitialActivationState() => false;
    }
}