using jbzd.UI.QuestMenu;

namespace jbzd.UI.PlayerMenu
{
    public class QuestUIOpenButton : OpenAndCloseUserInterface
    {
        protected override UserInterfaceController UserInterfaceToOpen { get; set; }
        protected override UserInterfaceController UserInterfaceToClose { get; set; }

        protected override void AssignPropertyUserInterfacesOpenAndClose(UserInterfaceManager userInterfaceManager)
        {
            UserInterfaceToOpen = userInterfaceManager.GetUIController<QuestUIController>();
            UserInterfaceToClose = userInterfaceManager.GetUIController<PlayerMenuUIController>();
        }
    }
}