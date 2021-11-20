using jbzdy.CharacterStats;
using UnityEngine;

/// <summary>
/// By SilverWalker
/// </summary>

namespace jbzdy.Player
{
    public class PlayerController : MonoBehaviour, IDamageable
    {
        [HideInInspector]
        public Animator characterAnimator;
        public Rigidbody rigidbody;
        private PlayerStats playerStats;
        private PlayerMovementController playerMovementController;
        private PlayerAttackController playerAttackController;

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
        [SerializeField]
        private Vector3 movementVector;

        public int MaximumHealth { get => maximumHealth; private set => maximumHealth = value; }
        public int CurrentHealth { get => currentHealth; private set => currentHealth = value; }
        public bool CanPlayerMove { get => canPlayerMove; set => canPlayerMove = value; }
        public bool IsAttacking { get => isAttacking; set => isAttacking = value; }
        public Vector3 MovementVector { get => movementVector; set => movementVector = value; }
        public float PlayerSpeed { get => playerSpeed; set => playerSpeed = value; }

        private void Start()
        {
            characterAnimator = GetComponentInChildren<Animator>();
            if (characterAnimator == null) Debug.Log("nie znaleziono animatora w postaci gracza");

            playerStats = GetComponent<PlayerStats>();
            rigidbody = GetComponent<Rigidbody>();

            playerMovementController = new PlayerMovementController(this);
            playerAttackController = new PlayerAttackController(this);
        }

        private void Update()
        {
            playerMovementController.UpdateCharacterMovement();

            playerAttackController.UpdateCharacterAttack();
        }

        public void SetDamage(int damageAmount, DamageType damageType)
        {
            playerStats.Health.BaseValue -= damageAmount;
        }

        public void SetDamage(int damageAmount, DamageType damageType, float criticalMultiplier, float criticalChance)
        {
            if (Random.Range(0.0f, 1.0f) <= criticalChance)
            {
                damageAmount = (int)(damageAmount * criticalMultiplier);
            }

            SetDamage(damageAmount, damageType);
        }

    } 
}