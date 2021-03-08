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

        [Header("States")]
        public bool isGrounded;

        [Header("References")]
        public new Transform camera;


        [Header("Movement Stats")]
        public float frontRayOffset = 0.5f;
        public float movementSpeed = 10;
        public float adaptSpeed = 1;
        public float rotationSpeed = 50;

        [HideInInspector]
        public LayerMask ignoreForGroundCheck;

        [HideInInspector]
        public string locomotionId = "locomotion";
        [HideInInspector]
        public string attackStateId = "attackState";

        public override void Init()
        {
            base.Init();

            //States definitions and initializations
            State locomotion = new State(
                new List<StateAction>() //Fixed Update
                {
                    new MovePlayer(this),
                },
                new List<StateAction>() //Update
                {
                    new InputManager(this),
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


            //Ignoring this layers
            ignoreForGroundCheck = ~(1 << 9 | 1 << 10);

            //States registration
            RegisterState(locomotionId, locomotion);
            RegisterState(attackStateId, attackState);

            //Changing state
            ChangeState(locomotionId);
        }

        private void Update()
        {
            delta = Time.deltaTime;
            Tick();
        }

        private void FixedUpdate()
        {
            delta = Time.fixedDeltaTime;
            FixedTick();
        }

        private void LateUpdate()
        {
            LateTick();
        }
    }
}

