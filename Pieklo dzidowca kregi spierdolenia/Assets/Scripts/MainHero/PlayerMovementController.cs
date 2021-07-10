using System;
using UnityEngine;

namespace jbzdy.Player
{
    public class PlayerMovementController
    {
        public PlayerController playerController;

        public PlayerMovementController(PlayerController _playerController)
        {
            playerController = _playerController;
        }

        public void UpdateCharacterMovement()
        {
            if (playerController.CanPlayerMove == true)
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

            playerController.transform.position += playerController.MovementVector.normalized * playerController.PlayerSpeed;
        }

        private void UpdateCharacterRotation()
        {
            if (playerController.MovementVector.magnitude == 0 || playerController.characterAnimator.GetBool("Attack") == true) return;

            var rotation = Quaternion.LookRotation(playerController.MovementVector);
            playerController.transform.rotation = rotation;
        }

        private void UpdateCharacterAnimation()
        {
            if (playerController.MovementVector.z != 0 || playerController.MovementVector.x != 0)
            {
                playerController.characterAnimator.SetBool("Run", true);
            }
            else
            {
                playerController.characterAnimator.SetBool("Run", false);
            }
        }
    } 
}
