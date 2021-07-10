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
    public Animator characterAnimator;
    private PlayerMovementController playerMovementController;

    [SerializeField]
    private float playerSpeed = 0.2f;
    [SerializeField]
    private float rotateSpeed;
    [SerializeField]
    private Vector3 movementVector;
    [SerializeField]
    private bool isAttacking;
    [SerializeField]
    private int maximumHealth;
    [SerializeField]
    private int currentHealth;
    [SerializeField]
    private bool canPlayerMove = true;

    public int MaximumHealth { get => maximumHealth; private set => maximumHealth = value; }
    public int CurrentHealth { get => currentHealth; private set => currentHealth = value; }
    public bool CanPlayerMove { get => canPlayerMove; set => canPlayerMove = value; }
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }
    public Vector3 MovementVector { get => movementVector; set => movementVector = value; }
    public float RotateSpeed { get => rotateSpeed; set => rotateSpeed = value; }
    public float PlayerSpeed { get => playerSpeed; set => playerSpeed = value; }

    private void Awake()
    {
        characterAnimator = GetComponentInChildren<Animator>();
        playerMovementController = new PlayerMovementController(this);
    }

    private void Update()
    {
        playerMovementController.UpdateCharacterMovement();

        UpdateCharacterAttack();
    }

    private void UpdateCharacterAttack()
    {
        isAttacking = GameManager.Instance.GameInputController.attackInputStatus.basic;

        if (isAttacking)
        {
            if (characterAnimator.GetBool("Attacking animation in progress") == false)
            {
                Vector3 flatVector = GameManager.Instance.GameInputController.mousePositionFlat;
                flatVector.y = 0;
                transform.LookAt(flatVector);
            }

            CanPlayerMove = false;
            characterAnimator.SetBool("Attack", true);
        }

    }

    public void SetDamage(int damageAmount, DamageType damageType)
    {
        CurrentHealth -= damageAmount;
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



