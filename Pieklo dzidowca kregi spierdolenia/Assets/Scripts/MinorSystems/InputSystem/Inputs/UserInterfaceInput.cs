using System.Collections.Generic;

namespace jbzd.MinorSystems.InputSystem.Inputs
{
    public class UserInterfaceInput : JbzdInput
    {
        public event ClickNotify OnInventoryOpened;
        public event ClickNotify OnQuestLogOpened;
        public event ClickNotify OnInGameMenuOpened;
        public event ClickNotify OnEscapeClick;
        public event ClickNotify OnQuickSaveClick;
        public event ClickNotify OnTestLoadClick;
        public event ClickNotify OnSpaceClick;

        public override void Start()
        {
            ButtonMappings = new List<KeyActionPair>
            {
                new()
                {
                    KeyMapping = KeyMapping.Inventory,
                    Action = () => OnInventoryOpened?.Invoke()
                },
                new()
                {
                    KeyMapping = KeyMapping.QuestLog,
                    Action = () => OnQuestLogOpened?.Invoke()
                },
                new()
                {
                    KeyMapping = KeyMapping.InGameMenu,
                    Action = () => OnInGameMenuOpened?.Invoke()
                },
                new()
                {
                    KeyMapping = KeyMapping.Escape,
                    Action = () => OnEscapeClick?.Invoke()
                },
                new ()
                {
                    KeyMapping = KeyMapping.QuickSave,
                    Action = () => OnQuickSaveClick?.Invoke()
                },
                new ()
                {
                    KeyMapping = KeyMapping.TestQuickLoad,
                    Action = () => OnTestLoadClick?.Invoke()
                },
                new ()
                {
                    KeyMapping = KeyMapping.Space,
                    Action = () => OnSpaceClick?.Invoke()
                }
            };
        }
        
    }
}