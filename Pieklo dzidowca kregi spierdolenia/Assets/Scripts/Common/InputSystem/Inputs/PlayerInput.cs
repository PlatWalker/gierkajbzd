using System.Collections.Generic;
using UnityEngine;
using jbzd.Common.InputSystem.InputStatuses;

namespace jbzd.Common.InputSystem.Inputs
{
    public class PlayerInput : JbzdInput
    {
        public event ClickNotify OnInteractClick;
        
        public MovementInputStatus movementInputStatus;
        public AttackInputStatus attackInputStatus;
        
        public Vector3 mousePositionFlat;
        
        public override void Start()
        {
            ButtonMappings = new List<KeyActionPair>
            {
                new()
                {
                    KeyMapping = KeyMapping.Interact,
                    Action = OnInteractClick
                }
            };
        }
        
        public override void UpdateInputs()
        {
            UpdateMovementInput();
            UpdateAttackInput();
            UpdateMousePosition();
            
            base.UpdateInputs();
        }

        private void UpdateAttackInput()
        {
            attackInputStatus.Basic = Input.GetKeyDown(KeyMapping.PlayerBasicAttack);
        }

        private void UpdateMovementInput()
        {
            movementInputStatus.Up = Input.GetKey(KeyMapping.PlayerMoveUp);
            movementInputStatus.Down = Input.GetKey(KeyMapping.PlayerMoveDown);
            movementInputStatus.Left = Input.GetKey(KeyMapping.PlayerMoveLeft);
            movementInputStatus.Right = Input.GetKey(KeyMapping.PlayerMoveRight);
        }
        
        private void UpdateMousePosition()
        {
            var ray = Camera.main!.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out var hitInfo, 600f, LayerMask.GetMask("Ground"));
        
            mousePositionFlat.x = hitInfo.point.x;
            mousePositionFlat.y = 0;
            mousePositionFlat.z = hitInfo.point.z;
        }
    }
}