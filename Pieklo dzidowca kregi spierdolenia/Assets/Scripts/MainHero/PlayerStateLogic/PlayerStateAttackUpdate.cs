using jbzd.Common.InputSystem.Inputs;
using UnityEngine;

namespace jbzd.MainHero.PlayerStateLogic
{
    public class PlayerStateAttackUpdate : IPlayerStateLogic
    {
        private readonly PlayerInput _playerInput;
        private readonly PlayerController _playerController;
        
        public PlayerStateAttackUpdate(PlayerController playerController, PlayerInput playerInput)
        {
            _playerController = playerController;
            _playerInput = playerInput;
        }
        
        public MonoBehaviourMethod MonoBehaviourMethodInWhichInvoked() => MonoBehaviourMethod.Update;

        public PlayerState PlayerStateInWhichInvoked() => PlayerState.Attack;

        public PlayerState StateLogic()
        {
            var playerState = PlayerStateInWhichInvoked();
            
            var flatVector = _playerInput.mousePositionFlat;
            flatVector.y = _playerController.transform.position.y;
            _playerController.transform.LookAt(flatVector);
                
            _playerController.CanPlayerMove = false;
            _playerController.CharacterAnimator.SetBool(PlayerStringAnimParam.AttackParam, true);
                    
            if(_playerInput.movementInputStatus.Down || _playerInput.movementInputStatus.Up || _playerInput.movementInputStatus.Left || _playerInput.movementInputStatus.Right)
            {
                _playerController.CanPlayerMove = true;
                playerState = PlayerState.Move;
            }

            if(!_playerInput.movementInputStatus.Down && !_playerInput.movementInputStatus.Up && !_playerInput.movementInputStatus.Left && !_playerInput.movementInputStatus.Right)
            {
                _playerController.CanPlayerMove = true;
                playerState =  PlayerState.Idle;
            }

            return playerState;
        }
    }
}