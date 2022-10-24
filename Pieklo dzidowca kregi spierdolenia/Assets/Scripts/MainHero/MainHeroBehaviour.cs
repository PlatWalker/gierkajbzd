
using UnityEngine;


namespace jbzd.MainHero
{
    public class MainHeroBehaviour : StateMachineBehaviour
    {
        public delegate void StateControl(Animator animator, AnimatorStateInfo stateInfo, int layerIndex);
        public event StateControl OnStateEnterPassed;
        public event StateControl OnStateExitPassed;
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) => OnStateEnterPassed?.Invoke(animator, stateInfo, layerIndex);

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) => OnStateExitPassed?.Invoke(animator, stateInfo, layerIndex);
       
    }
}
