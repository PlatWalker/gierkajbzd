using UnityEngine;

namespace jbzd.Common.InputSystem
{
    public static class KeyMapping
    {
        #region UserInterface

        public const KeyCode Inventory = KeyCode.I;
        public const KeyCode QuestLog = KeyCode.Q;
        public const KeyCode PlayerMenu = KeyCode.M;
        public const KeyCode InGameMenu = KeyCode.Escape;
        
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