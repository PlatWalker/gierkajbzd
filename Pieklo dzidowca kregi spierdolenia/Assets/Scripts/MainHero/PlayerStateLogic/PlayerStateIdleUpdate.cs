using jbzd.Common.Enums;
using jbzd.Common.InputSystem.Inputs;
using UnityEngine;

namespace jbzd.MainHero.PlayerStateLogic
{
    public class PlayerStateIdleUpdate : IPlayerStateLogic
    {
        private readonly PlayerInput _playerInput;
        private readonly PlayerManager _playerController;
        
        public PlayerStateIdleUpdate(
            PlayerManager playerController,
            PlayerInput playerInput,
            MainHeroBehaviour behaviour)
        {
            _playerController = playerController;
            _playerInput = playerInput;
            behaviour.OnStateEnterPassed += OnIdleStateEnter;
            behaviour.OnStateExitPassed += OnIdleStateExit;
        }

        public MonoBehaviourMethod MonoBehaviourMethodInWhichInvoked() => MonoBehaviourMethod.Update;

        public PlayerState PlayerStateInWhichInvoked() => PlayerState.Idle;

        public PlayerState StateLogic()
        {
            var playerState = PlayerStateInWhichInvoked();
            
            if(_playerInput.attackInputStatus.Basic) playerState = PlayerState.Attack;

            if(_playerInput.movementInputStatus.Down || _playerInput.movementInputStatus.Up || _playerInput.movementInputStatus.Left || _playerInput.movementInputStatus.Right)
            {
                playerState = PlayerState.Move;
            }

            return playerState;
        }
        
        private void OnIdleStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

        private void OnIdleStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }
    }
}