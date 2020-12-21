///<summary>
///Created by Kumdzio
///</summary>


using UnityEngine;

public class SecondAtkActivator : StateMachineBehaviour
{
    [SerializeField] private int useSecondAtkAfter = 3;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
     //   
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetInteger("atkCounter") >= useSecondAtkAfter)
        {
            animator.SetBool("shouldUseSecondAtk", true);
        }
        else if(animator.GetCurrentAnimatorStateInfo(0).IsName("Armature|Atk_2"))
        {
            animator.SetInteger("atkCounter", (int)animator.GetCurrentAnimatorStateInfo(0).normalizedTime);  
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
