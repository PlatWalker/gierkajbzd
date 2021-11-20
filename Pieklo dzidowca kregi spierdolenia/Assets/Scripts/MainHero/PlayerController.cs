using jbzdy.CharacterStats;
using System;
using UnityEngine;

/// <summary>
/// By SilverWalker
/// </summary>

namespace jbzdy.Player
{
    public class PlayerController : MonoBehaviour, IDamageable
    {
        private PlayerStats playerStats;
        private PlayerMovementController playerMovementController;
        private PlayerAttackController playerAttackController;
        private Vector3 movementVector;

        [SerializeField]
        private float playerSpeed = 0.2f;
        [SerializeField]
        private int maximumHealth;
        [SerializeField]
        private int currentHealth;
        [SerializeField]
        private bool canPlayerMove = true;
        [SerializeField]
        private bool isAttacking;

        public Animator CharacterAnimator { get; set; }
        public Rigidbody rb { get; set; }
        public int MaximumHealth { get => maximumHealth; private set => maximumHealth = value; }
        public int CurrentHealth { get => currentHealth; private set => currentHealth = value; }
        public bool CanPlayerMove { get => canPlayerMove; set => canPlayerMove = value; }
        public bool IsAttacking { get => isAttacking; set => isAttacking = value; }
        public Vector3 MovementVector { get => movementVector; set => movementVector = value; }
        public float PlayerSpeed { get => playerSpeed; set => playerSpeed = value; }

        private void Start()
        {
            CharacterAnimator = GetComponentInChildren<Animator>();
            if (CharacterAnimator == null) Debug.Log("nie znaleziono animatora w postaci gracza");

            rb = GetComponent<Rigidbody>();
            playerStats = GetComponent<PlayerStats>();

            playerMovementController = new PlayerMovementController(this);
            playerAttackController = new PlayerAttackController(this);
        }

        private void FixedUpdate()
        {
            playerMovementController.UpdateCharacterMovement();
        }

        private void Update()
        {
            playerAttackController.UpdateCharacterAttack();
        }

        public void SetDamage(int damageAmount, DamageType damageType)
        {
            playerStats.Health.BaseValue -= damageAmount;
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
}