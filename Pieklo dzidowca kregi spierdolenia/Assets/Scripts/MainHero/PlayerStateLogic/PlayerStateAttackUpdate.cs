using jbzd.Common.Enums;
using jbzd.Common.InputSystem.Inputs;
using UnityEngine;

namespace jbzd.MainHero.PlayerStateLogic
{
    public class PlayerStateAttackUpdate : IPlayerStateLogic
    {
        private readonly PlayerInput _playerInput;
        private readonly PlayerManager _playerController;
        
        public PlayerStateAttackUpdate(
            PlayerManager playerController,
            PlayerInput playerInput,
            MainHeroBehaviour behaviour)
        {
            _playerController = playerController;
            _playerInput = playerInput;
            
            behaviour.OnStateEnterPassed += OnAttackStateEnter;
            behaviour.OnStateExitPassed += OnAttackStateExit;
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
        
        bool isAttackAnimationPlayingJbzd(AnimatorStateInfo stateInfo) =>
            stateInfo.IsName("Atk1") || stateInfo.IsName("Atk2") || stateInfo.IsName("Atk3") ||
            stateInfo.IsName("Atk4");

        bool isTransitionStateJbzd(AnimatorStateInfo stateInfo) => stateInfo.IsName("Transition state");
        
        private void OnAttackStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (isTransitionStateJbzd(stateInfo))
            {
                _playerController.CanPlayerMove = true;
            }
            
            if (isAttackAnimationPlayingJbzd(stateInfo))
            {
                _playerController.NextFrameDash = true;
                animator.SetBool(PlayerStringAnimParam.AttackInProgressParam, true);
            }

            animator.SetBool(PlayerStringAnimParam.AttackParam, false);
        }

        private void OnAttackStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!isTransitionStateJbzd(stateInfo))
                animator.SetBool(PlayerStringAnimParam.AttackParam, false);

            if (isAttackAnimationPlayingJbzd(stateInfo))
                animator.SetBool(PlayerStringAnimParam.AttackInProgressParam, false); 
        }
    }
}