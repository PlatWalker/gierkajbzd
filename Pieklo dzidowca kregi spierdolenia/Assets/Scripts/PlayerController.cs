using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private InputHandler inputHandler;  //reference to input handler
    [SerializeField]
    private Interactable playerFocus;   //reference to interactables

    private Camera playerCamera;    //reference to player camera

    [SerializeField]
    private float playerSpeed = 0.2f;  //player speed
    [SerializeField]
    private float rotateSpeed;  //player rotate speed (frames per second you can rotate)
    [SerializeField]
    private bool RotateTowardMouse; //you can either move wsad and rotate that way or rotate towards mouse
    
    private Vector3 movementVector; //vector applied to movement;

    Animator characterAnimator;

    // Start is called before the first frame updatedd
    void Awake()
    {
        inputHandler = GetComponent<InputHandler>();
        playerCamera = Camera.main;
        characterAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateCharacterMovement();
        //UpdateCharacterAnimation();

        /*
        var targetVector = new Vector3(inputHandler.InputVector.x, 0, inputHandler.InputVector.y).normalized;   //player direction
        var movementVector = MoveTowardTarget(targetVector);

        //move to darection aiming
        if (!RotateTowardMouse)
        {
            RotateTowardMovementVector(movementVector);  //good luck working on that l o l
        }
        //rotate to direction traveling

        if (RotateTowardMouse)
        {
            RotateFromMouseVector();
        }

        if (Input.GetMouseButtonDown(1)) //right click for interaction
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;


            if (Physics.Raycast(ray, out hit)) //, 100, movementMask))
            {
                Debug.Log("Hitted " + hit.collider.name);

                //check if hit an interactable object and set focus on it
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                if (interactable != null)
                {
                    SetFocus(interactable);
                }
            }
        }
        */


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
        transform.position += movementVector * playerSpeed;
    }

    private void UpdateCharacterRotation()
    {
        //Looking at mouse pos, dangerous since if we get sth 3D it will break, but it will work for now
        transform.LookAt(InputController.Instance.mousePositionFlat);
    }
    /*
    private void RotateTowardMovementVector(Vector3 movementDirection)
    {
        if (movementDirection.magnitude == 0) { return; }
        var rotation = Quaternion.LookRotation(movementDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed);
    }


    private void RotateFromMouseVector()
    {
        Ray ray = playerCamera.ScreenPointToRay(inputHandler.MousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance: 300f))
        {
            var target = hitInfo.point;
            target.y = transform.position.y;
            transform.LookAt(target);
        }
    }



    private Vector3 MoveTowardTarget(Vector3 targetVector)
    {
        var speed = playerSpeed * Time.deltaTime;
        targetVector = Quaternion.Euler(0, playerCamera.gameObject.transform.rotation.eulerAngles.y, 0) * targetVector; //forward for us is what is forward for the camera
        var targetPosition = transform.position + targetVector * speed;
        transform.position = targetPosition;
        return targetVector;
    }

    private void SetFocus(Interactable newFocus)    //sets focus on the object
    {
        if (newFocus != playerFocus)
        {
            if (playerFocus != null)
            {
                playerFocus.OnDefocused();
            }

            playerFocus = newFocus;
        }

        newFocus.OnFocused(transform);
    }

    private void RemoveFocus(Interactable newFocus) //removes object focus (useless for now, saved for later)
    {
        if (playerFocus != null)
        {
            playerFocus.OnDefocused();
        }

        playerFocus.OnDefocused();
        playerFocus = null;
    }
    */
    private void UpdateCharacterAnimation()
    {
        // if movement, then set animation to play
        if (movementVector.z != 0 || movementVector.x != 0)
        {
            characterAnimator.SetBool("Walking", true);
        }
        // else stop animation
        else
        {
            characterAnimator.SetBool("Walking", false);
        }
    }
}
