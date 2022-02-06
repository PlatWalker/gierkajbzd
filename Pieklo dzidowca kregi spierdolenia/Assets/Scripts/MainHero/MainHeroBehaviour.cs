using UnityEngine;

/// <summary>
/// by SilverWalker
/// </summary>

namespace jbzdy.Player
{
    public class MainHeroBehaviour : StateMachineBehaviour
    {
        private PlayerController playerController;

        bool isAttackAnimationPlaying(AnimatorStateInfo stateInfo) =>
            stateInfo.IsName("Atk1") || stateInfo.IsName("Atk2") || stateInfo.IsName("Atk3") ||
            stateInfo.IsName("Atk4");

        bool isTransitionState(AnimatorStateInfo stateInfo) => stateInfo.IsName("Transition state");

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (playerController == null)
                playerController = GameManager.Instance.PlayerObject.GetComponent<PlayerController>();

            if (isTransitionState(stateInfo))
                playerController.CanPlayerMoveWithKeyboard = true;
            
            if (isAttackAnimationPlaying(stateInfo))
            {
                playerController.nextFrameDash = true;
                playerController.lengthOfCurrentAttackAnimation = stateInfo.length;
                animator.SetBool(StringAnimatorParameters.AttackInProgressParam, true);
            }

            animator.SetBool(StringAnimatorParameters.AttackParam, false);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!isTransitionState(stateInfo)) 
                animator.SetBool(StringAnimatorParameters.AttackParam, false);

            if (isAttackAnimationPlaying(stateInfo))
            {
                animator.SetBool(StringAnimatorParameters.AttackInProgressParam, false);
                playerController.playerAttackController.listOfEnemiesColliders.Clear();
            }
                
            
        }
    }
}
