using UnityEngine;

namespace jbzd.Common.InputSystem
{
    public static class KeyMapping
    {
        #region UserInterface

        public const KeyCode Inventory = KeyCode.I;
        public const KeyCode QuestLog = KeyCode.Q;
        public const KeyCode InGameMenu = KeyCode.M;
        public const KeyCode Escape = KeyCode.Escape;
        public const KeyCode QuickSave = KeyCode.F4;
        public const KeyCode TestQuickLoad = KeyCode.F5;
        
        #endregion

        #region PlayerInput

        public const KeyCode PlayerMoveUp = KeyCode.W;
        public const KeyCode PlayerMoveDown = KeyCode.S;
        public const KeyCode PlayerMoveLeft = KeyCode.A;
        public const KeyCode PlayerMoveRight = KeyCode.D;

        public const KeyCode PlayerBasicAttack = KeyCode.Mouse0;

        public const KeyCode Interact = KeyCode.E;

        #endregion
    }
}