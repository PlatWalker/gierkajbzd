using jbzdy.Managers;
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
    private Animator characterAnimator;
    private GameObject characterObject;

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
    [SerializeField]
    private int _health;

    public int Health { get => _health; private set => _health = value; }

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

    private int kupa // do zmiany jak wejdzie poprawiony IDEAMGABLE
    {
        get
        {
            float maxHP = 250;
            int cosiek = (int)(((float)Health / maxHP) * 100);
            return cosiek;
        }
        
    }

    public int GetHealthPercentage { get => kupa; }

    private bool isAttacking;

    private void Awake()
    {
        characterAnimator = GetComponentInChildren<Animator>();
        characterObject = gameObject.transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        UpdateCharacterMovement();

        UpdateCharacterAttack();
    }

    private void UpdateCharacterAttack()
    {
        
        isAttacking = InputController.Instance.attackInputStatus.normal;

        if (isAttacking)
        {
            if (characterAnimator.GetBool("Attacking animation in progress") == false)
            {
                Vector3 flatVector = InputController.Instance.mousePositionFlat;
                flatVector.y = 0;
                transform.LookAt(flatVector);
            }

            CanPlayerMove = false;
            characterAnimator.SetBool("Attack", true);
        }

    }

    #region Movement

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



