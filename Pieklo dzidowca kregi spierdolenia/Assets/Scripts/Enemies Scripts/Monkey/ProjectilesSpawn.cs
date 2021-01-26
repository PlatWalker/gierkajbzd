using UnityEngine;
/// <summary>
/// Created by Kumdzio
/// </summary>
public class ProjectilesSpawn : StateMachineBehaviour
{
    private bool onlyOneProjectile;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if ((animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1.0) < 0.65)
        {
            onlyOneProjectile = true;
        }
        if (((animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1.0) > 0.65) && onlyOneProjectile)
        {
            animator.SetBool("spawnProjectile", true);
            onlyOneProjectile = false;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        onlyOneProjectile = false;
        animator.SetBool("spawnProjectile", false);
    }

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
