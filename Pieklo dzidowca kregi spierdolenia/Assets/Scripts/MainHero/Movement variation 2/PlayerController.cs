using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Timeline;
using UnityEngine.UI;

/// <summary>
/// By Tails, Edited by Silver
/// </summary>

public class PlayerController : MonoBehaviour , IDamageable
{
    private InputHandler inputHandler;
    private Animator characterAnimator;
    private GameObject characterObject;

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
    private int _health;

    Vector3 attackVector = Vector3.zero;            // 
    Vector3 reflectedAttackVector = Vector3.zero;   // could be local. to change after merge 
    Vector3 animationVector = Vector3.zero;         // 
    Vector3 moveVectorSaved = Vector3.zero;

    private bool attackSequenceLock = false; // determines whatever sequence is still in progress

    public int Health { get => _health; private set => _health = value; }

    private int kupa 
    {
        get
        {
            float maxHP = 250;
            int cosiek = (int)(((float)Health / maxHP) * 100);
            return cosiek;
        }
        
    }

    public int GetHealthPercentage { get => kupa; }

    private void Awake()
    {
        inputHandler = GetComponent<InputHandler>();
        characterAnimator = GetComponentInChildren<Animator>();
        characterObject = gameObject.transform.GetChild(0).gameObject;
    }

    void Update()
    {
        UpdateCharacterMovement();

        UpdateCharacterAttack();

        ToDeleteAfterMergeItsjustfordebugging(); // just for debugging can be deleted after merge iwth main branch
    }

    private void ToDeleteAfterMergeItsjustfordebugging()
    {
        if (reflectedAttackVector != null)
        {
            Debug.DrawLine(transform.position, transform.position + reflectedAttackVector * 10, Color.red);
            Debug.DrawLine(transform.position, transform.position + animationVector * 10, Color.yellow);
            Debug.DrawLine(transform.position, transform.position + attackVector * 10, Color.blue);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {

            Debug.Log(Vector3.Angle(movementVector, Vector3.forward));

        }
    }

    private void UpdateCharacterAttack()
    {

        if (InputController.Instance.attackInputStatus.normal == true && attackSequenceLock == false)
        {

            attackVector =  InputController.Instance.mousePositionFlat - transform.position;

            // reflect attack vector to compensate movement(momentum) vector

            reflectedAttackVector = (2 * Vector3.Dot(movementVector.normalized, attackVector.normalized) * movementVector.normalized) - attackVector.normalized; 

            // rotate reflected vector to let it point a direction of animation to use. It's important for blender tree.

            float Beta = -1;

            if (movementVector == Vector3.forward) Beta = 0;
            else if (movementVector == (Vector3.left + Vector3.forward)) Beta = 315;
            else if (movementVector == Vector3.left) Beta = 270;
            else if (movementVector == (Vector3.left + Vector3.back)) Beta = 225;
            else if (movementVector == Vector3.back) Beta = 180;
            else if (movementVector == (Vector3.back + Vector3.right)) Beta = 135;
            else if (movementVector == Vector3.right) Beta = 90;
            else if (movementVector == (Vector3.forward + Vector3.right)) Beta = 45;

            if (Beta >= 0)
            {
                animationVector = Quaternion.AngleAxis( Beta, Vector3.down ) * reflectedAttackVector;
            }

            // start attack animation

            Vector3 flatVector = InputController.Instance.mousePositionFlat;
            flatVector.y = 0;
            transform.LookAt(flatVector);
            characterAnimator.SetFloat("x", animationVector.x);
            characterAnimator.SetFloat("z", animationVector.z);
            characterAnimator.SetBool("Attack", true);
            attackSequenceLock = true;
        }
        else if(InputController.Instance.attackInputStatus.normal == false && attackSequenceLock == true)
        {
            attackSequenceLock = false;
            characterAnimator.SetBool("Attack", false);
        }


    }

    #region Movement

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
        transform.position += movementVector.normalized * playerSpeed;
    }

    private void UpdateCharacterRotation()
    {
        if (movementVector.magnitude == 0 || characterAnimator.GetBool("Attack") == true) return;

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

    #endregion

    public void SetDamage(int damageAmount, DamageType damageType)
    {
        Health -= damageAmount;
    }

    public void SetDamage(int damageAmount, DamageType damageType, float criticalMultiplier, float criticalChance)
    {
        if (UnityEngine.Random.Range(0.0f, 1.0f) <= criticalChance)
        {
            damageAmount = (int)(damageAmount * criticalMultiplier);
        }

        SetDamage(damageAmount, damageType);
    }

}
