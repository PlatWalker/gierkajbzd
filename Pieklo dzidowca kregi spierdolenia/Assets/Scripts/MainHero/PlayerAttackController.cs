using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// by SilverWalker
/// </summary>

namespace jbzdy.Player
{
    public class PlayerAttackController
    {
        private readonly PlayerController playerController;
        
        public List<Collider> listOfEnemiesColliders = new List<Collider>();
        
        public PlayerAttackController(PlayerController _playerController)
        {
            playerController = _playerController;
        }

        public void UpdateCharacterAttack()
        {
            playerController.IsAttacking = GameManager.Instance.GameInputController.attackInputStatus.Basic;

            if (playerController.IsAttacking)
            {
                Vector3 flatVector = GameManager.Instance.GameInputController.mousePositionFlat;
                flatVector.y = playerController.transform.position.y;
                playerController.transform.LookAt(flatVector);
                
                playerController.CanPlayerMove = false;
                playerController.CharacterAnimator.SetBool(StringAnimatorParameters.AttackParam, true);

            }

        }
    } 
}
