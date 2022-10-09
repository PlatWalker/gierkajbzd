using jbzd;
using jbzd.MainHero;
using UnityEngine;

/// <summary>
/// by SilverWalker
/// </summary>

namespace jbzd.MainHero
{
    public class MainHeroBehaviour : StateMachineBehaviour
    {
        private PlayerController playerController;
        public delegate void StateControl(Animator animator, AnimatorStateInfo stateInfo, int layerIndex);
        public event StateControl stateControl;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) => stateControl?.Invoke(animator, stateInfo, layerIndex);

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) => stateControl?.Invoke(animator, stateInfo, layerIndex);
       
    }
}
