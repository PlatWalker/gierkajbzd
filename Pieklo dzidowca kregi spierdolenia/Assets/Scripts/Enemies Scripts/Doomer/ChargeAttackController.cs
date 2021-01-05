///<summary>
///Created by kumdzio
///</summary>


using UnityEngine;

public class ChargeAttackController : StateMachineBehaviour
{
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.8)
        {
            animator.SetBool("hasFinishedFirstAttack", true);
        }
    }
}
