using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class DoomerController : MonoBehaviour
{
    [SerializeField] public Transform mainCharacterTransform;
    [SerializeField] private float movementSpeed=0.034f;
    [SerializeField] private float movementRushSpeed=0.066f;
    [SerializeField] private float jumpSpeed = 0.1f;
    [SerializeField] private float aggroRadius=10.0f;
    [SerializeField] private float attackRadius=1.5f;
    [SerializeField] private float firstAttackRadius = 3.0f;
    [SerializeField] private float rotationSpeed = 0.1f;
    private Vector3 jumpDirection;
    private Vector3 spawnPoint;
    private Animator doomerAnimator;
    private bool hasDoneAggro;
    private bool hasDoneSpecialAttack;
    private bool isUsingChargedAttack;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = new Vector3(this.gameObject.transform.position.x,this.gameObject.transform.position.y,this.gameObject.transform.position.z);
        doomerAnimator = GetComponent<Animator>();
        hasDoneSpecialAttack = false;
        hasDoneAggro = false;
        isUsingChargedAttack = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float distanceToMainChar = Vector3.Distance(this.gameObject.transform.position, mainCharacterTransform.position);
        hasDoneAggro = doomerAnimator.GetBool("hasFinishedAggro");
        hasDoneSpecialAttack = doomerAnimator.GetBool("hasFinishedFirstAttack");
        if (hasDoneSpecialAttack)
        {
            isUsingChargedAttack = false;
        }

        if (distanceToMainChar < aggroRadius && distanceToMainChar > attackRadius)// checking if main char is visible for enemy and should not attack
        {
            doomerAnimator.SetBool("isSeeingPlayer", true);
            doomerAnimator.SetBool("shouldReturnToSpawn", false);
            doomerAnimator.SetBool("isPlayerReached", false);
            UpdateEnemyRotation(mainCharacterTransform.position);
            if (hasDoneAggro)
            {
                if (distanceToMainChar <= firstAttackRadius && !hasDoneSpecialAttack) //checking if should start special attack
                {
                    doomerAnimator.SetBool("isPlayerReached", true);    //thanks to this animator know when start animation of jump
                    isUsingChargedAttack = true;
                    if (jumpDirection == new Vector3(0.0f,0.0f,0.0f)) 
                    {
                        jumpDirection = mainCharacterTransform.position; //saving jump direction so enemy cannot change direction in air
                            
                    }
                    if (Vector3.Distance(jumpDirection, this.gameObject.transform.position) > 0.5f)
                    {
                        //any direction given here to UpdateEnemyPostion will be ignored
                        UpdateEnemyPosition(jumpDirection, jumpSpeed);
                    }
                }
                else if(hasDoneSpecialAttack)
                {
                    //if attack has been made just move in normal speed to player
                    UpdateEnemyPosition(new Vector3(mainCharacterTransform.position.x - this.gameObject.transform.position.x, 0, mainCharacterTransform.position.z - this.gameObject.transform.position.z));
                }
                else
                { 
                    //when attack has been not made and cannot be made yet move to player inc harge
                    UpdateEnemyPosition(new Vector3(mainCharacterTransform.position.x - this.gameObject.transform.position.x, 0, mainCharacterTransform.position.z - this.gameObject.transform.position.z),movementRushSpeed);
                }

            }            
        }
        else if (distanceToMainChar <= attackRadius)
        {
            if (distanceToMainChar > 0.3)
            {
                UpdateEnemyRotation(mainCharacterTransform.position);
            }
            doomerAnimator.SetBool("isSeeingPlayer", true);
            doomerAnimator.SetBool("shouldReturnToSpawn", false);
            doomerAnimator.SetBool("isPlayerReached", true);
            //tutaj zadawanie obrażeń collider i te sprawy
            //dodać tutaj sprawdzenie czy zakończono atak specjalnt i jesli tak to normalne obrażenia a jak nie to dodatkowe obrażenia
        }
        else
        {
            doomerAnimator.SetBool("isSeeingPlayer", false);
            doomerAnimator.SetBool("isPlayerReached", false);
            if (Vector3.Distance(this.gameObject.transform.position, spawnPoint) > 2.0f)
            {
                doomerAnimator.SetBool("shouldReturnToSpawn", true);
                UpdateEnemyRotation(new Vector3(spawnPoint.x, 0, spawnPoint.z));
                UpdateEnemyPosition(new Vector3(spawnPoint.x - this.gameObject.transform.position.x, 0, spawnPoint.z - this.gameObject.transform.position.z));
            }
            else
            { 
                doomerAnimator.SetBool("shouldReturnToSpawn", false);
            }
        }
    }

    void UpdateEnemyPosition(Vector3 direction)
    {
        direction = Vector3.Normalize(direction);
        this.gameObject.transform.position += direction * movementSpeed;
    }
    void UpdateEnemyPosition(Vector3 direction,float speedModifier)
    {
        direction = Vector3.Normalize(direction);
        if (isUsingChargedAttack)
        {
            direction = new Vector3(
                jumpDirection.x - this.gameObject.transform.position.x,
                jumpDirection.y - this.gameObject.transform.position.y,
                jumpDirection.z - this.gameObject.transform.position.z
                );
            direction = Vector3.Normalize(direction);
        }
         this.gameObject.transform.position += direction * (movementSpeed+speedModifier);
    }

    void UpdateEnemyRotation(Vector3 direction)
    {
        if (isUsingChargedAttack)
        {
            direction.x = jumpDirection.x;
            direction.z = jumpDirection.z;
        }
        this.gameObject.transform.Rotate(0.0f, -90.0f, 0.0f); //reApply Bug of gizmos
        Vector3 targetDirection = direction - this.gameObject.transform.position;
        Vector3 newDirection = Vector3.RotateTowards(this.gameObject.transform.forward, targetDirection, rotationSpeed, 0.0f);
        this.gameObject.transform.rotation = Quaternion.LookRotation(newDirection);
        this.gameObject.transform.Rotate(0.0f, 90.0f, 0.0f); // removing Bug of gizmos

    }

}
