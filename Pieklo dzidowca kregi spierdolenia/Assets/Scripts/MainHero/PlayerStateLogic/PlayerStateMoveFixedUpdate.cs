using System;
using jbzd.Common.Enums;
using jbzd.Common.InputSystem.Inputs;
using UnityEngine;

namespace jbzd.MainHero.PlayerStateLogic
{
    public class PlayerStateMoveFixedUpdate : IPlayerStateLogic
    {
        private readonly PlayerInput _playerInput;
        private readonly PlayerManager _playerController;

        private Vector3 _movementVector;
        private Vector3 _newPositionVector;
        
        public PlayerStateMoveFixedUpdate(
            PlayerManager playerController,
            PlayerInput playerInput,
            MainHeroBehaviour behaviour)
        {
            _playerController = playerController;
            _playerInput = playerInput;
            behaviour.OnStateEnterPassed += OnMoveStateEnter;
            behaviour.OnStateExitPassed += OnMoveStateExit;
        }
        
        public MonoBehaviourMethod MonoBehaviourMethodInWhichInvoked() => MonoBehaviourMethod.FixedUpdate;

        public PlayerState PlayerStateInWhichInvoked() => PlayerState.Move;
        
        public PlayerState StateLogic()
        {
            var playerState = PlayerStateInWhichInvoked();

            if (_playerInput.attackInputStatus.Basic)
            {
                return PlayerState.Attack;
            }
            
            if (_playerController.CanPlayerMove)
            {
                UpdateCharacterPosition();
                UpdateCharacterRotation();
                UpdateCharacterAnimation();
            }
            else
            {
                return PlayerState.Idle;
            }

            if(IsNotMoving())
            {
                playerState = PlayerState.Idle;
            }
            
            return playerState;
            
            //----------- local functions -----------//
            
            bool IsNotMoving() => _playerInput.movementInputStatus is
                {Down: false, Up: false, Left: false, Right: false};
        }
        
        private void OnMoveStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){ }
        
        private void OnMoveStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

        private void UpdateCharacterPosition()
        {
            _movementVector = Vector3.zero;
            _movementVector += Vector3.forward * Convert.ToInt32(_playerInput.movementInputStatus.Up);
            _movementVector += Vector3.back * Convert.ToInt32(_playerInput.movementInputStatus.Down);
            _movementVector += Vector3.left * Convert.ToInt32(_playerInput.movementInputStatus.Left);
            _movementVector += Vector3.right * Convert.ToInt32(_playerInput.movementInputStatus.Right);

            _playerController.Rb.AddForce(_movementVector.normalized * _playerController.PlayerSpeed, ForceMode.VelocityChange);
        }

        private void UpdateCharacterRotation()
        {
            if (_movementVector.magnitude == 0) return;
            
            var rotation = Quaternion.LookRotation(_movementVector);
            
            _playerController.transform.rotation = rotation;
        }

        private void UpdateCharacterAnimation()
        {
            if (_movementVector.z != 0 || _movementVector.x != 0)
            {
                _playerController.CharacterAnimator.SetBool(PlayerStringAnimParam.RunParam, true);
            }
            else
            {
                _playerController.CharacterAnimator.SetBool(PlayerStringAnimParam.RunParam, false);
            }
        }
    }
}