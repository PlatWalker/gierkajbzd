using System;
using jbzd.Common.InputSystem.Inputs;
using UnityEngine;

namespace jbzd.MainHero.PlayerStateLogic
{
    public class PlayerStateMove : IPlayerStateLogic
    {
        private readonly PlayerInput _playerInput;
        private readonly PlayerManager _playerManager;

        private Vector3 _movementVector;
        private Vector3 _newPositionVector;
        
        public PlayerStateMove(
            PlayerManager playerManager,
            PlayerInput playerInput)
        {
            _playerManager = playerManager;
            _playerInput = playerInput;
        }

        public PlayerState InitPlayerState => PlayerState.Move;

        public PlayerState StateLogicForFixedUpdate()
        {
            var playerState = InitPlayerState;

            if (_playerInput.attackInputStatus.Basic)
            {
                return PlayerState.Attack;
            }
            
            if (_playerManager.CanPlayerMove)
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

        public PlayerState StateLogicForUpdate()
        {
            return PlayerState.Move;
        }

        public PlayerState StateLogicForLateUpdate()
        {
            if (!_playerManager.CanPlayerMove)
            {
                return PlayerState.Idle;
            }
            
            return _playerInput.attackInputStatus.Basic ? PlayerState.Attack : PlayerState.Move;
        }
        
        private void UpdateCharacterPosition()
        {
            _movementVector = Vector3.zero;
            _movementVector += Vector3.forward * Convert.ToInt32(_playerInput.movementInputStatus.Up);
            _movementVector += Vector3.back * Convert.ToInt32(_playerInput.movementInputStatus.Down);
            _movementVector += Vector3.left * Convert.ToInt32(_playerInput.movementInputStatus.Left);
            _movementVector += Vector3.right * Convert.ToInt32(_playerInput.movementInputStatus.Right);

            _playerManager.Rb.AddForce(_movementVector.normalized * _playerManager.PlayerSpeed, ForceMode.VelocityChange);
        }

        private void UpdateCharacterRotation()
        {
            if (_movementVector.magnitude == 0) return;
            
            var rotation = Quaternion.LookRotation(_movementVector);
            
            _playerManager.transform.rotation = rotation;
        }

        private void UpdateCharacterAnimation()
        {
            if (_movementVector.z != 0 || _movementVector.x != 0)
            {
                _playerManager.CharacterAnimator.SetBool(PlayerStringAnimParam.RunParam, true);
            }
            else
            {
                _playerManager.CharacterAnimator.SetBool(PlayerStringAnimParam.RunParam, false);
            }
        }
    }
}