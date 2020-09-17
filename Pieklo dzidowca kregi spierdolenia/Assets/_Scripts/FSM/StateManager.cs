using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace JG 
{
    //template for player states 

    public abstract class StateManager : MonoBehaviour
    {
        State currentState;
        Dictionary<string, State> allStates = new Dictionary<string, State>();

        [HideInInspector]
        public Transform mTransform;    //objects transform
         
        // Start is called before the first frame update
        void Start()
        {
            mTransform = transform;

            Init();
        }

        public abstract void Init();

        public void Tick()
        {
            if(currentState == null)
            {
                return;
            }

            currentState.Tick();
        }

        public void FixedTick()
        {
            if (currentState == null)
            {
                return;
            }

            currentState.FixedTick();
        }

        public void LateTick()
        {
            if (currentState == null)
            {
                return;
            }

            currentState.LateTick();
        }

        public void ChangeState(string targetId)
        {
            if(currentState != null)
            {
                //run on exit actions of currentState
            }

            State targetState = GetState(targetId);


            //run on enter actions of currentState

            currentState = targetState;
        }

        State GetState(string targetId) //getting state to initialize it
        {
            allStates.TryGetValue(targetId, out State retVal);
            return retVal;
        }

        protected void RegisterState(string stateId, State state)   //adding new state
        {
            allStates.Add(stateId, state);
        }
    }

}


