using System;
using UnityEngine;

/// <summary>
/// By Tails, Edited by Silver
/// </summary>

public class PlayerController : MonoBehaviour
{
    private InputHandler inputHandler;

    //[SerializeField]
    //private Interactable playerFocus;

    [SerializeField]
    private float playerSpeed = 0.2f;
    [SerializeField]
    private float rotateSpeed;
    [SerializeField]
    private bool RotateTowardMouse; //you can either move wsad and rotate that way or rotate towards mouse
    [SerializeField]
    private Vector3 movementVector;

    [SerializeField]
    private bool canPlayerMove = true;

    Animator characterAnimator;

    public bool CanPlayerMove
    {
        get
        {
            return canPlayerMove;
        }

        set
        {
            canPlayerMove = value;
        }
    }

    private void Awake()
    {
        inputHandler = GetComponent<InputHandler>();
        characterAnimator = GetComponentInChildren<Animator>();
    }

    void FixedUpdate()
    {
        UpdateCharacterMovement();
    }

    private void UpdateCharacterMovement()
    {
        if (CanPlayerMove == true)
        {
            UpdateCharacterPosition();
            UpdateCharacterRotation();
            UpdateCharacterAnimation();
        }
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
        transform.position += movementVector.normalized * playerSpeed;
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