using UnityEngine;

/// <summary>
/// by SilverWalker
/// </summary>

namespace jbzdy.Player
{
    public class MainHeroBehaviour : StateMachineBehaviour
    {
        private GameObject playerObject;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            playerObject = GameManager.Instance.PlayerObject;
            
            if (animator.GetLayerName(layerIndex) == "Attack")
            {
                if (stateInfo.IsName("Transition state"))
                {
                    playerObject.GetComponent<PlayerController>().CanPlayerMove = true;
                }
                else
                {
                    animator.SetBool(StringAnimatorParameters.AttackInProgressParam, true);
                }

                animator.SetBool(StringAnimatorParameters.AttackParam, false);
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (animator.GetLayerName(layerIndex) == "Attack" && stateInfo.IsName("Transition state") == false) 
                    animator.SetBool(StringAnimatorParameters.AttackParam, false);
        }
    }
}
