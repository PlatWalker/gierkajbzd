using UnityEngine;

namespace jbzdy.Player
{
    public class PlayerAttackController
    {
        private PlayerController playerController;

        public PlayerAttackController(PlayerController _playerController)
        {
            playerController = _playerController;
        }

        public void UpdateCharacterAttack()
        {
            playerController.IsAttacking = GameManager.Instance.GameInputController.attackInputStatus.basic;

            if (playerController.IsAttacking)
            {
                if (playerController.characterAnimator.GetBool("Attacking animation in progress") == false)
                {
                    Vector3 flatVector = GameManager.Instance.GameInputController.mousePositionFlat;
                    flatVector.y = 0;
                    playerController.transform.LookAt(flatVector);
                }

                playerController.CanPlayerMove = false;
                playerController.characterAnimator.SetBool("Attack", true);
            }

        }
    } 
}
