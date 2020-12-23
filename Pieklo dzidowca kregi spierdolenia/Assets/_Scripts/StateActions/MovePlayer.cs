using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JG
{
    public class MovePlayer : StateAction
    {
        PlayerStateManager playerStates;

        public MovePlayer(PlayerStateManager playerStateManager)
        {
            playerStates = playerStateManager;
        }

        public override bool Execute()
        {
            float frontY = 0;
            RaycastHit hit;
            Vector3 origin = playerStates.mTransform.position + (playerStates.mTransform.forward * playerStates.frontRayOffset);
            origin.y += 0.5f;
            Debug.DrawRay(origin, -Vector3.up, Color.red, 0.01f, false);

            if (Physics.Raycast(origin, -Vector3.up, out hit, 1, playerStates.ignoreForGroundCheck))
            {
                float y = hit.point.y;
                frontY = y - playerStates.mTransform.position.y;
            }

            Vector3 currentVeloctiy = playerStates.rigidbody.velocity;
            Vector3 targetVelocity = playerStates.mTransform.forward * playerStates.moveAmount * playerStates.movementSpeed;
            
            //if(playerStates.isLockingOn)
            //{
            //    targetVelocity = playerStates.rotateDirection * playerStates.moveAmount * playerStates.movementSpeed;
            //}

            if(playerStates.isGrounded)
            {
                float moveAmount = playerStates.moveAmount;

                if(moveAmount > 0.1f)
                {
                    playerStates.rigidbody.isKinematic = false;
                    playerStates.rigidbody.drag = 0;

                    if(Mathf.Abs(frontY) > 0.02f)
                    {
                        targetVelocity.y = ((frontY > 0) ? frontY + 0.2f : frontY - 0.2f) * playerStates.movementSpeed;
                    }
                }
                else
                {
                    float abs = Mathf.Abs(frontY);
                    if(abs > 0.02f)
                    {
                        playerStates.rigidbody.isKinematic = true;
                        targetVelocity.y = 0;
                        playerStates.rigidbody.drag = 4;
                    }
                }

                HandleRotation();
                HandleAnimations();
            }
            else
            {
                playerStates.rigidbody.isKinematic = false;
                playerStates.rigidbody.drag = 0;
                targetVelocity.y = currentVeloctiy.y;
            }

            Debug.DrawRay((playerStates.mTransform.position + Vector3.up * 0.2f), targetVelocity, Color.green, 0.01f, false);

            //playerStates.rigidbody.velocity = Vector3.Lerp(currentVeloctiy, targetVelocity, playerStates.delta * playerStates.adaptSpeed);

            playerStates.rigidbody.velocity = targetVelocity;
            
            return false;
        }

        void HandleRotation()
        {
            float h = playerStates.horizontal;
            float v = playerStates.vertical;

            Vector3 targetDir = playerStates.camera.transform.forward * v;
            targetDir += playerStates.camera.transform.right * h;
            targetDir.Normalize();

            targetDir.y = 0;
            if (targetDir == Vector3.zero)
            {
                targetDir = playerStates.transform.forward;
            }

            Quaternion tr = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(playerStates.mTransform.rotation, tr, playerStates.delta * playerStates.moveAmount * playerStates.rotationSpeed);

            playerStates.mTransform.rotation = targetRotation;
        }

        void HandleAnimations()
        {
            if(playerStates.isGrounded)
            {
                float amount = playerStates.moveAmount;
                float forwardVal = 0f;

                if (amount > 0 && amount < 0.5f)
                {
                    forwardVal = 0.05f;
                }
                else if(amount > 0.05f)
                {
                    forwardVal = 1;
                }    

                playerStates.animator.SetFloat("forward", forwardVal, 0.2f, playerStates.delta);
            }
            else
            {

            }
        }
    }
}

