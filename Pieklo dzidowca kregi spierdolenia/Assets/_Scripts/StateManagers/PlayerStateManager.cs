using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JG
{
    //StateManager containing all possible player actions to run them in separate ticks
    //Each state has 3 Update methods 
    
    
    //To be able to switch to a newly created state you must initialize it and register it

    public class PlayerStateManager : ObjectsStateManager
    {
        [Header("Inputs")]
        public float moveAmount;
        public Vector3 rotateDirection;

        public string locomotionId = "locomotion";
        public string attackStateId = "attackState";

        public override void Init()
        {
            base.Init();

            //States definitions and initializations
            State locomotion = new State(
                new List<StateAction>() //Fixed Update
                {
                    new InputManager(this),
                },
                new List<StateAction>() //Update
                {

                },
                new List<StateAction>() //Late Update
                {

                }
                );

            State attackState = new State(
                new List<StateAction>() //Fixed Update
                {

                },
                new List<StateAction>() //Update
                {

                },
                new List<StateAction>() //Late Update
                {

                }
                );


            //States registration
            RegisterState(locomotionId, locomotion);
            RegisterState(attackStateId, attackState);

            //Changing state
            ChangeState(locomotionId);
        }

        private void Update()
        {
            Tick();
        }

        private void FixedUpdate()
        {
            FixedTick();
        }

        private void LateUpdate()
        {
            LateTick();
        }
    }
}

