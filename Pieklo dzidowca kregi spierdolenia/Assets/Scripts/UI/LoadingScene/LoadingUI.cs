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
            gameObject.SetActive(false);
            FreezeTime.Unfreeze();
            asyncOperation.completed -= HideLoadingScreen;
        }

        public override bool InitialActivationState() => false;
    }
}