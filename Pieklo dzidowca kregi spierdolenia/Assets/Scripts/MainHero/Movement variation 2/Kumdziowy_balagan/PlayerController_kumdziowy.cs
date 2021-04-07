using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// By Tails, Edited by Silver
/// </summary>

public class PlayerController_kumdziowy : MonoBehaviour
{
    private InputHandler inputHandler;

    //[SerializeField]
    //private Interactable playerFocus;

    [SerializeField]
    private float playerAcceleration = 2f;
    [SerializeField]
    private float playerDecceleration = 3f;
    [SerializeField]
    private float playerMaxSpeed = 20.0f;
    [SerializeField]
    private float rotateSpeed;
    [SerializeField]
    private bool RotateTowardMouse; //you can either move wsad and rotate that way or rotate towards mouse
    [SerializeField]
    private Vector3 movementVector;

    Animator characterAnimator;

    Rigidbody rbody;

    private void Awake()
    {
        inputHandler = GetComponent<InputHandler>();
        characterAnimator = GetComponentInChildren<Animator>();
        rbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        UpdateCharacterMovement();
    }

    private void UpdateCharacterMovement()
    {
        UpdateCharacterPosition();
        UpdateCharacterRotation();
        UpdateCharacterAnimation();
    }

    private void UpdateCharacterPosition()
    {
        
        // I KNOW RIGHT? I just didnt want any if
        // Basically if someone knows that ToInt32 is using ifs and its heavier
        // Let me know then ill recreate it as A?1:0 statement
        movementVector = Vector3.zero;
        movementVector += Vector3.forward * Convert.ToInt32(InputController.Instance.movementInputStatus.up);
        movementVector += Vector3.back * Convert.ToInt32(InputController.Instance.movementInputStatus.down);
        movementVector += Vector3.left * Convert.ToInt32(InputController.Instance.movementInputStatus.left);
        movementVector += Vector3.right * Convert.ToInt32(InputController.Instance.movementInputStatus.right);
        
        // Applying above calculations
        //transform.position += movementVector.normalized * playerAcceleration;
        //this is not working good because it is skipping all the physics and collisions
        //insted we should use Rigidbody.AddForce(forceVektor,ForceMode.VelocityChange)
        
        HandleButtonRaction( InputController.Instance.movementInputStatus.up, Vector3.forward, rbody.velocity.z);
        HandleButtonRaction( InputController.Instance.movementInputStatus.down, Vector3.back, -rbody.velocity.z);
        HandleButtonRaction( InputController.Instance.movementInputStatus.left, Vector3.left, -rbody.velocity.x);
        HandleButtonRaction( InputController.Instance.movementInputStatus.right, Vector3.right, rbody.velocity.x);

        //if (rbody.velocity.magnitude < playerMaxSpeed)
        //{
           // rbody.AddForce(movementVector.normalized * playerAcceleration, ForceMode.VelocityChange);
        //}

        void HandleButtonRaction(bool isKeyPressed, Vector3 directionWhenPressed, float velocityToCheck)
        {
            if (isKeyPressed)
            {
                if (velocityToCheck < playerMaxSpeed)
                {
                    rbody.AddForce(directionWhenPressed * playerAcceleration,ForceMode.VelocityChange);
                }
            }
            else
            {
                if (velocityToCheck > 0)
                {
                    rbody.AddForce(-directionWhenPressed * velocityToCheck/2, ForceMode.VelocityChange);
                }
            }
        }
    }

    //it is to avoid jumping when ending going up ramps
    private void OnCollisionExit(Collision collision)
    {
        if (rbody.velocity.y > 1)
        {
            rbody.AddForce(new Vector3(0, -rbody.velocity.y, 0), ForceMode.VelocityChange);
        }
    }

    private void UpdateCharacterRotation()
    {
        if (movementVector.magnitude == 0) return;

        var rotation = Quaternion.LookRotation(movementVector);
        transform.rotation = rotation;
    }
    
    private void UpdateCharacterAnimation()
    {
        // if movement, then set animation to play
        if (movementVector.z != 0 || movementVector.x != 0)
        {
            characterAnimator.SetBool("Run", true);
        }
        else
        {
            characterAnimator.SetBool("Run", false);
        }
    }
}
