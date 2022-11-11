using System;
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
        
        public PlayerStateMoveFixedUpdate(PlayerManager playerController, PlayerInput playerInput)
        {
            _playerController = playerController;
            _playerInput = playerInput;
        }
        
        public MonoBehaviourMethod MonoBehaviourMethodInWhichInvoked() => MonoBehaviourMethod.FixedUpdate;

        public PlayerState PlayerStateInWhichInvoked() => PlayerState.Move;

        public PlayerState StateLogic()
        {
            var playerState = PlayerStateInWhichInvoked();
            
            if (_playerController.NextFrameDash)
            {
                DashAttackMove();
                _playerController.NextFrameDash = false;
                return PlayerState.Move;
            }  

            if (_playerController.CanPlayerMove)
            {
                UpdateCharacterPosition();
                UpdateCharacterRotation(_movementVector);
                UpdateCharacterAnimation();
            }

            if(_playerInput.attackInputStatus.Basic) playerState = PlayerState.Attack;

            if(!_playerInput.movementInputStatus.Down && !_playerInput.movementInputStatus.Up && !_playerInput.movementInputStatus.Left && !_playerInput.movementInputStatus.Right)
            {
                playerState = PlayerState.Idle;
            }

            return playerState;
        }
        
        private void DashAttackMove()
        {
            var dashVector = _playerInput.mousePositionFlat - _playerController.transform.position;
            
            UpdateCharacterRotation(dashVector.normalized);

            _playerController.Rb.AddForce(dashVector.normalized * _playerController.DashAttackMovePower , ForceMode.Impulse);
            
            StickPlayerToGround();
        }
        
        private void UpdateCharacterPosition()
        {
            _movementVector = Vector3.zero;
            _movementVector += Vector3.forward * Convert.ToInt32(_playerInput.movementInputStatus.Up);
            _movementVector += Vector3.back * Convert.ToInt32(_playerInput.movementInputStatus.Down);
            _movementVector += Vector3.left * Convert.ToInt32(_playerInput.movementInputStatus.Left);
            _movementVector += Vector3.right * Convert.ToInt32(_playerInput.movementInputStatus.Right);
            
            StickPlayerToGround();

            _playerController.Rb.AddForce(_movementVector.normalized * _playerController.PlayerSpeed, ForceMode.VelocityChange);
        }

        private void UpdateCharacterRotation(Vector3 lookDirection)
        {
            if (lookDirection.magnitude == 0) return;
            
            var rotation = Quaternion.LookRotation(lookDirection);
            
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
        
        private void StickPlayerToGround()
        {
            if (Physics.Raycast(_playerController.transform.position + Vector3.up, Vector3.down, out RaycastHit hit) 
                && hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                _newPositionVector.x = _playerController.Rb.position.x;
                _newPositionVector.y = hit.point.y;
                _newPositionVector.z = _playerController.Rb.position.z;

                _playerController.Rb.MovePosition(_newPositionVector);
            }
        }
    }
}