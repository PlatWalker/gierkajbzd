using System;
using System.Collections.Generic;
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
    public class PlayerAttackController
    {
        private readonly PlayerController playerController;
        private readonly PlayerInput _playerInput;
        
        public List<Collider> listOfEnemiesColliders = new List<Collider>();
        
        public PlayerAttackController(PlayerController _playerController, PlayerInput playerInput)
        {
            playerController = _playerController;
            _playerInput = playerInput;
        }

        public void UpdateCharacterAttack()
        {
            playerController.IsAttacking = _playerInput.attackInputStatus.Basic;

            if (playerController.IsAttacking)
            {
                Vector3 flatVector = _playerInput.mousePositionFlat;
                flatVector.y = playerController.transform.position.y;
                playerController.transform.LookAt(flatVector);
                
                playerController.CanPlayerMove = false;
                playerController.CharacterAnimator.SetBool(StringAnimatorParameters.AttackParam, true);

            }

        }
    } 
}
