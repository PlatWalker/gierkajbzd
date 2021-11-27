using System;
using UnityEngine;

/// <summary>
/// by SilverWalker
/// </summary>

namespace jbzdy.Player
{
    public class PlayerMovementController
    {
        private readonly PlayerController playerController;
        private Vector3 newPositionVector;

        public PlayerMovementController(PlayerController _playerController)
        {
            playerController = _playerController;
            
        }

        public void UpdateCharacterMovement()
        {
            if (playerController.CanPlayerMoveWithKeyboard)
            {
                UpdateCharacterPosition();
                UpdateCharacterRotation();
                UpdateCharacterAnimation();
            }
        }

        private void UpdateCharacterPosition()
        {
            playerController.MovementVector = Vector3.zero;
            playerController.MovementVector += Vector3.forward * Convert.ToInt32(GameManager.Instance.GameInputController.movementInputStatus.up);
            playerController.MovementVector += Vector3.back * Convert.ToInt32(GameManager.Instance.GameInputController.movementInputStatus.down);
            playerController.MovementVector += Vector3.left * Convert.ToInt32(GameManager.Instance.GameInputController.movementInputStatus.left);
            playerController.MovementVector += Vector3.right * Convert.ToInt32(GameManager.Instance.GameInputController.movementInputStatus.right);

            StickPlayerToGround();

            playerController.rb.AddForce(playerController.MovementVector.normalized * playerController.PlayerSpeed * Time.fixedDeltaTime , ForceMode.VelocityChange);
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

        private void UpdateCharacterRotation()
        {
            if (playerController.MovementVector.magnitude == 0 || playerController.CharacterAnimator.GetBool("Attack") == true) return;

            var rotation = Quaternion.LookRotation(playerController.MovementVector);
            playerController.transform.rotation = rotation;
        }

        private void UpdateCharacterAnimation()
        {
            if (playerController.MovementVector.z != 0 || playerController.MovementVector.x != 0)
            {
                playerController.CharacterAnimator.SetBool("Run", true);
            }
            else
            {
                playerController.CharacterAnimator.SetBool("Run", false);
            }
        }
    } 
}
