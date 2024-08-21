namespace jbzd.UI.InGameMenu
{
    public class InGameMenuUIController : UserInterfaceController
    {
        public override bool InitialActivationState() => false;

        public void OpenUI(UserInterfaceController uiToOpen)
        {
            uiToOpen.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }

    }
}
