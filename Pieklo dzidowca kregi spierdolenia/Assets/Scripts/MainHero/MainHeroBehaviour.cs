using JG;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class MainHeroBehaviour : StateMachineBehaviour
{
    [SerializeField]
    private int NumberOfFramesBeetweenAttacks;

    private GameObject playerObject;

    public void Awake()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
    }

    //OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetLayerName(layerIndex) == "Attack")
        {
            if (stateInfo.IsName("Transition state"))
            {
                playerObject.GetComponent<PlayerController>().CanPlayerMove = true;
            }
            else
            {
                animator.SetBool("Attacking animation in progress", true);
            }
            
            animator.SetBool("Attack", false); 
        }
    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    //OnStateExit is called before OnStateExit is called on any state inside this state machine
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetLayerName(layerIndex) == "Attack" && stateInfo.IsName("Transition state") == false) animator.SetBool("Attacking animation in progress", false);
    }

    // OnStateMove is called before OnStateMove is called on any state inside this state machine
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateIK is called before OnStateIK is called on any state inside this state machine
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMachineEnter is called when entering a state machine via its Entry Node
    //override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    //{
    //    
    //}

    // OnStateMachineExit is called when exiting a state machine via its Exit Node
    //override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    //{
    //
    //}
}
