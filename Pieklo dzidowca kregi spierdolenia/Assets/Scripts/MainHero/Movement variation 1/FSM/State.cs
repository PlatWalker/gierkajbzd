using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace JG
{
    //definition for player and game states

    public class State
    {
        bool forceExit;

        List<StateAction> fixedUpdateActions; 
        List<StateAction> updateActions; 
        List<StateAction> lateUpdateActions; 

        //class constructor for passing values to use in StateManagers
        public State(List<StateAction> fixedUpdateActions, List<StateAction> updateActions, List<StateAction> lateUpdateActions)
        {
            this.fixedUpdateActions = fixedUpdateActions;
            this.updateActions = updateActions;
            this.lateUpdateActions = lateUpdateActions;
        }

        public void Tick() //run in Update
        {
            ExecuteListOfActions(updateActions);
        }
        public void FixedTick() //run in FixedUpdate
        {
            ExecuteListOfActions(fixedUpdateActions);
        }

        public void LateTick() //run in LateUpdate
        {
            ExecuteListOfActions(lateUpdateActions);
            forceExit = false;
        }

        void ExecuteListOfActions(List<StateAction> action) //execute all ticks
        {
            for (int i = 0; i < action.Count; i++)
            {
                if(forceExit)
                {
                    return;
                }

                forceExit = action[i].Execute();
            }
        }
    }
}


