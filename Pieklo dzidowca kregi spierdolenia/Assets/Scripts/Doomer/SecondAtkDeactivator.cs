///<summary>
///Created by Kumdzio
///</summary>


using UnityEngine;

public class SecondAtkDeactivator : StateMachineBehaviour
{

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("shouldUseSecondAtk", false);
        animator.SetInteger("atkCounter", 0);
    }

}
