using UnityEngine;

namespace jbzd.Common.InputSystem
{
    public class UserInterfaceInput : IInput
    {
        public delegate void ClickNotify();
        public event ClickNotify OnInventoryOpened;
        public event ClickNotify OnQuestLogOpened;
        public event ClickNotify OnPlayerMenuOpened;
        public event ClickNotify OnInGameMenuOpened;

        public void UpdateInputs()
        {
            if (!Input.anyKeyDown) return;
            
            if (Input.GetKeyDown(KeyMapping.Inventory)) OnInventoryOpened?.Invoke();
            else if (Input.GetKeyDown(KeyMapping.QuestLog)) OnQuestLogOpened?.Invoke();
            else if (Input.GetKeyDown(KeyMapping.PlayerMenu)) OnPlayerMenuOpened?.Invoke();
            else if (Input.GetKeyDown(KeyMapping.InGameMenu)) OnInGameMenuOpened?.Invoke();
        }
        
    }
}