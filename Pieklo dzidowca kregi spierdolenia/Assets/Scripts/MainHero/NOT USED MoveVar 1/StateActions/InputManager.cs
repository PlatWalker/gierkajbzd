using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JG
{
    //players input manager
    
    public class InputManager : StateAction
    {
        PlayerStateManager stateManager;

        bool isAttacking;

        //triggers and bumpers
        bool mouseRight, Rt, mouseLeft, Lt;
     
        //Inventory
        bool inventoryInput;
        
        //Prompts
        bool b_Input, y_Input, x_Input;

        //Dpad
        bool leftArrow, rightArrow, upArrow, downArrow;

        public InputManager(PlayerStateManager states)
        {
            stateManager = states;
        }

        public override bool Execute() //executing and defining player inputs
        {
            bool retVal;    //value of our players attack state

            stateManager.horizontal = Input.GetAxis("Horizontal");
            stateManager.vertical = Input.GetAxis("Vertical");

            mouseRight = Input.GetMouseButton((1)); //right mouse button
            //Rt = Input.GetButton("Rt");
            mouseLeft = Input.GetMouseButton((0)); //left mouse button
            //Lt = Input.GetButton("Lt");

            //inventoryInput = Input.GetButton("Inventory");

            //b_Input = Input.GetButton("B");
            //y_Input = Input.GetButton("X");
            //x_Input = Input.GetButton("Y");

            //leftArrow = Input.GetButton("Left");
            //rightArrow = Input.GetButton("Right");
            //upArrow = Input.GetButton("Up");
            //downArrow = Input.GetButton("Down");

            //calcualte players move amount
            stateManager.moveAmount = Mathf.Clamp01(Mathf.Abs(stateManager.horizontal) + Mathf.Abs(stateManager.vertical));

            retVal = false;//HandleAttacking(); 

            return retVal; 
        }

        //bool HandleAttacking()
        //{
        //    if (mouseLeft)
        //    {
        //        //isAttacking = true;
        //    }

        //    //Logic for interrupting an attack (it will happen at the same frame)
        //    //if (y_Input)
        //    //{
        //    //    isAttacking = false;
        //    //}


        //    if (isAttacking)
        //    {
        //        //Get attack animation from items..
        //        //Play animation
        //        //
        //        //Change player state
        //        //stateManager.ChangeState(stateManager.attackStateId);
        //    }

        //    return isAttacking;
        //}

       
    }
}

