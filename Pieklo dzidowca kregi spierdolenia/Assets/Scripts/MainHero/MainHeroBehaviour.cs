using UnityEngine;

/// <summary>
/// by SilverWalker
/// </summary>

namespace jbzdy.Player
{
    public class MainHeroBehaviour : StateMachineBehaviour
    {
        private PlayerController playerController;
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            playerController = GameManager.Instance.PlayerObject.GetComponent<PlayerController>();

            if (animator.GetLayerName(layerIndex) == "Attack") {

                if (stateInfo.IsName("Transition state"))
                {
                    playerController.CanPlayerMoveWithKeyboard = true;
                }
                else
                {
                    animator.SetBool("Attacking animation in progress", true);
                    if (stateInfo.IsName("Atk1") || stateInfo.IsName("Atk2") || stateInfo.IsName("Atk3") || stateInfo.IsName("Atk4"))
                    {
                        playerController.playerAttackController.DashAttackMove();
                    }
                }

                animator.SetBool("Attack", false);
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (animator.GetLayerName(layerIndex) == "Attack" && stateInfo.IsName("Transition state") == false) 
                animator.SetBool("Attacking animation in progress", false);
        }
    }
}
