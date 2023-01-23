namespace jbzd.UI.PlayerMenu
{
    public class QuestUIOpenButton : OpenAndCloseUserInterface
    {
        protected override UserInterfaceController UserInterfaceToOpen { get; set; }
        protected override UserInterfaceController UserInterfaceToClose { get; set; }

        protected override void AssignPropertyUserInterfacesOpenAndClose(UserInterfaceManager userInterfaceManager)
        {
            UserInterfaceToClose = userInterfaceManager.GetUIController<PlayerMenuUIController>();
        }
    }
}