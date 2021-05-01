///<summary>
///Created by Kumdzio
///</summary>


using UnityEngine;
namespace jbzdy.Enemies
{
    public class SecondAtkActivator : StateMachineBehaviour
    {
        [SerializeField] private int useSecondAtkAfter = 3;

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (animator.GetInteger("atkCounter") >= useSecondAtkAfter)
            {
                animator.SetBool("shouldUseSecondAtk", true);
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Armature|Atk_2"))
            {
                animator.SetInteger("atkCounter", (int)animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            }
        }
    }
}