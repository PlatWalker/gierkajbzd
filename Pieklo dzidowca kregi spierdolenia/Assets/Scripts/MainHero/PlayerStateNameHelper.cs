using UnityEngine;

namespace jbzd.MainHero
{
    public static class PlayerStateNameHelper
    {
        private static string TransitionState => "Transition state";
        private static string FirstComboAttack => "Atk1";
        private static string SecondComboAttack => "Atk2";

        public static string GetStateName(AnimatorStateInfo animatorStateInfo)
        {
            if (animatorStateInfo.IsName(FirstComboAttack)) return FirstComboAttack;
            if (animatorStateInfo.IsName(SecondComboAttack)) return SecondComboAttack;
            if (animatorStateInfo.IsName(TransitionState)) return TransitionState;
            
            Debug.LogError("There is state in animator that need to be added to helper class!");
            return string.Empty;
        }

        public static bool IsAttacking(AnimatorStateInfo stateInfo)=> stateInfo.IsName(FirstComboAttack) || stateInfo.IsName(SecondComboAttack);
    }
}