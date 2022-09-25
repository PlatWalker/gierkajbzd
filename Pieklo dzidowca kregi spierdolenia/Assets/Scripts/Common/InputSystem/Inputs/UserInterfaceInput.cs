using UnityEngine;

namespace jbzd.Common.InputSystem.Inputs
{
    public class UserInterfaceInput : IInput
    {
        public delegate void ClickNotify();
        public event ClickNotify OnInventoryOpened;
        public event ClickNotify OnQuestLogOpened;
        public event ClickNotify OnPlayerMenuOpened;
        public event ClickNotify OnInGameMenuOpened;
        public event ClickNotify OnEscapeClick;

        public void UpdateInputs()
        {
            if (!Input.anyKeyDown) return;

            if (Input.GetKeyDown(KeyMapping.Inventory)) OnInventoryOpened?.Invoke();
            if (Input.GetKeyDown(KeyMapping.QuestLog)) OnQuestLogOpened?.Invoke();
            if (Input.GetKeyDown(KeyMapping.PlayerMenu)) OnPlayerMenuOpened?.Invoke();
            if (Input.GetKeyDown(KeyMapping.InGameMenu)) OnInGameMenuOpened?.Invoke();
            if (Input.GetKeyDown(KeyMapping.Escape)) OnEscapeClick?.Invoke();
        }
        
    }
}