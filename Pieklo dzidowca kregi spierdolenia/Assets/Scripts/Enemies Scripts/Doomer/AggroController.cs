///<summary>
///Created by kumdzio
///</summary>

using UnityEngine;
namespace jbzdy.Enemies
{
    public class AggroController : StateMachineBehaviour
    {
        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99 && animator.GetCurrentAnimatorStateInfo(0).IsName("Armature|Aggro"))
            {
                DoomerController.HasDoneAggro = true;
            }
        }
    }
}