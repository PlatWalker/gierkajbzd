using System;
using jbzd;
using jbzd.Common.InputSystem;
using jbzd.Common.InputSystem.Inputs;
using jbzd.MainHero;
using UnityEngine;

/// <summary>
/// by SilverWalker
/// </summary>

namespace jbzdy.Player
{
    
    public class PlayerMovementController
    {
        private readonly PlayerController playerController;
        private readonly PlayerInput _playerInput;
        private Vector3 newPositionVector;
        
        private Vector3 velocity = Vector3.zero;
        
        public PlayerMovementController(PlayerController _playerController, PlayerInput playerInput)
        {
            playerController = _playerController;
            _playerInput = playerInput;
        }

        public void UpdateCharacterMovement()
        {
            if (playerController.NextFrameDash)
            {
                DashAttackMove();
                playerController.NextFrameDash = false;
                return;
            }
            
            if (playerController.CanPlayerMove)
            {
                UpdateCharacterPosition();
                UpdateCharacterRotation(playerController.MovementVector);
                UpdateCharacterAnimation();
            }
        }
        
        private void UpdateCharacterPosition()
        {
            playerController.MovementVector = Vector3.zero;
            playerController.MovementVector += Vector3.forward * Convert.ToInt32(_playerInput.movementInputStatus.Up);
            playerController.MovementVector += Vector3.back * Convert.ToInt32(_playerInput.movementInputStatus.Down);
            playerController.MovementVector += Vector3.left * Convert.ToInt32(_playerInput.movementInputStatus.Left);
            playerController.MovementVector += Vector3.right * Convert.ToInt32(_playerInput.movementInputStatus.Right);
            
            StickPlayerToGround();

            playerController.rb.AddForce(playerController.MovementVector.normalized * playerController.PlayerSpeed, ForceMode.VelocityChange);
        }

        private void UpdateCharacterRotation(Vector3 lookDirection)
        {
            if (lookDirection.magnitude == 0) return;
            
            var rotation = Quaternion.LookRotation(lookDirection);
            
            playerController.transform.rotation = rotation;
        }

        private void UpdateCharacterAnimation()
        {
            if (playerController.MovementVector.z != 0 || playerController.MovementVector.x != 0)
            {
                playerController.CharacterAnimator.SetBool(StringAnimatorParameters.RunParam, true);
            }
            else
            {
                playerController.CharacterAnimator.SetBool(StringAnimatorParameters.RunParam, false);
            }
        }
        
        private void StickPlayerToGround()
        {
            if (Physics.Raycast(playerController.transform.position + Vector3.up, Vector3.down, out RaycastHit hit) 
                && hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                newPositionVector.x = playerController.rb.position.x;
                newPositionVector.y = hit.point.y;
                newPositionVector.z = playerController.rb.position.z;

                playerController.rb.MovePosition(newPositionVector);
            }
        }

        private void DashAttackMove()
        {
            Vector3 dashVector = _playerInput.mousePositionFlat - playerController.transform.position;
            
            UpdateCharacterRotation(dashVector.normalized);

            playerController.rb.AddForce(dashVector.normalized * playerController.DashAttackMovePower , ForceMode.Impulse);
            
            
            
            StickPlayerToGround();
        }
    } 
}
