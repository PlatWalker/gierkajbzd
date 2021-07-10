using UnityEngine;

namespace jbzdy.Player
{
    public class MainHeroBehaviour : StateMachineBehaviour
    {
        private GameObject playerObject;

        public void Update()
        {
            playerObject = GameManager.Instance.PlayerObject;
        }

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
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
                    animator.SetBool("Attacking animation in progress", true);
                }

                animator.SetBool("Attack", false);
            }
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (animator.GetLayerName(layerIndex) == "Attack" && stateInfo.IsName("Transition state") == false) animator.SetBool("Attacking animation in progress", false);
        }
    }
}
