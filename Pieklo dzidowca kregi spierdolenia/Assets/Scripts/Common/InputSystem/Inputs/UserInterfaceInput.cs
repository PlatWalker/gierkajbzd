using System;
using System.Collections.Generic;
using UnityEngine;

namespace jbzd.Common.InputSystem.Inputs
{
    public class UserInterfaceInput : JbzdInput
    {
        public event ClickNotify OnInventoryOpened;
        public event ClickNotify OnQuestLogOpened;
        public event ClickNotify OnPlayerMenuOpened;
        public event ClickNotify OnInGameMenuOpened;
        public event ClickNotify OnEscapeClick;

        public override void Start()
        {
            ButtonMappings = new List<KeyActionPair>
            {
                new()
                {
                    KeyMapping = KeyMapping.Inventory,
                    Action = OnInventoryOpened
                },
                new()
                {
                    KeyMapping = KeyMapping.QuestLog,
                    Action = OnQuestLogOpened
                },
                new()
                {
                    KeyMapping = KeyMapping.PlayerMenu,
                    Action = OnPlayerMenuOpened
                },
                new()
                {
                    KeyMapping = KeyMapping.InGameMenu,
                    Action = OnInGameMenuOpened
                },
                new()
                {
                    KeyMapping = KeyMapping.Escape,
                    Action = OnEscapeClick
                }
            };
        }

    }
}