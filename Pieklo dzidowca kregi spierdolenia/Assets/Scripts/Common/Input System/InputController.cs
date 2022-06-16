using UnityEngine;

namespace jbzd.Common.InputSystem
{
    /// <summary>
    /// By SilverWalker
    /// </summary>

    public class InputController : KeyMapping
    {
        public struct MovementInputStatus
        {
            public bool Up;
            public bool Down;
            public bool Left;
            public bool Right;
        }

        public struct AttackInputStatus
        {
            public bool Basic;
        }

        public struct UIinputStatus
        {
            public bool InGameMenu;
        }

        public UIinputStatus uIinputStatus;
        public MovementInputStatus movementInputStatus;
        public AttackInputStatus attackInputStatus;
        public delegate void InteractButtonClick();
        public static event InteractButtonClick OnInteractButtonClick;
        public Vector3 mousePositionFlat;

        private LayerMask _layerMask;
    
        private void Start()
        {
            _layerMask = LayerMask.GetMask("Ground");
        }

        private void Update()
        {
            UpdateAttackInput();
            UpdateMovementInput();
            UpdateMousePosition();

            UpdateUIInput();
            UpdateInteractInput();
        }

        private void UpdateInteractInput()
        {
            if (WasPressed(Interact.InteractKey))
                OnInteractButtonClick?.Invoke();
        }

        private void UpdateUIInput()
        {
            uIinputStatus.InGameMenu = WasPressed(UIinput.inGameMenu);
        }

        private void UpdateAttackInput()
        {
            attackInputStatus.Basic = WasPressed(PlayerAttack.basic);
        }

        private void UpdateMovementInput()
        {
            movementInputStatus.Up = Pressed(PlayerMovement.up);
            movementInputStatus.Down = Pressed(PlayerMovement.down);
            movementInputStatus.Left = Pressed(PlayerMovement.left);
            movementInputStatus.Right = Pressed(PlayerMovement.right);
        }

        private void UpdateMousePosition()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out RaycastHit hitInfo, 600f, _layerMask);
        
            mousePositionFlat.x = hitInfo.point.x;
            mousePositionFlat.y = 0;
            mousePositionFlat.z = hitInfo.point.z;
        }

        private bool WasPressed(KeyCode k)
        {
            return Input.GetKeyDown(k);
        }

        private bool Pressed(KeyCode k)
        {
            return Input.GetKey(k);
        }

    }
}