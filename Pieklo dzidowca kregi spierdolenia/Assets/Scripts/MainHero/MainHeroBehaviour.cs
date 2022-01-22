using UnityEngine;

/// <summary>
/// by SilverWalker
/// </summary>

namespace jbzdy.Player
{
    //TODO refactr, komunikacja powinna odbywac sie na zasadzie pobierania stanow przez player controller z tej klasy
    // w tej chwili wbplywamy na wartosci klasy nadrzednej lamiac zasady SOLID
    public class MainHeroBehaviour : StateMachineBehaviour
    {
        private PlayerController playerController;
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            playerController = GameManager.Instance.PlayerObject.GetComponent<PlayerController>();

            if (animator.GetLayerName(layerIndex) == "Attack") {
                
                if (stateInfo.IsName("Transition state"))
                {
                    playerController.CanPlayerMoveWithKeyboard = true;
                }
                else
                {
                    animator.SetBool(StringAnimatorParameters.AttackInProgressParam, true);
                    
                    if (stateInfo.IsName("Atk1") || stateInfo.IsName("Atk2") || stateInfo.IsName("Atk3") || stateInfo.IsName("Atk4"))
                    {
                        playerController.nextFrameDash = true;
                        playerController.lengthOfCurrentAttackAnimation = stateInfo.length;
                    }
                }

                animator.SetBool(StringAnimatorParameters.AttackParam, false);
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (animator.GetLayerName(layerIndex) == "Attack" && stateInfo.IsName("Transition state") == false) 
                animator.SetBool(StringAnimatorParameters.AttackParam, false);
        }
    }
}
