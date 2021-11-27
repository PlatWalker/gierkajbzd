using jbzdy.CharacterStats;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// By SilverWalker
/// </summary>

namespace jbzdy.Player
{
    public struct StringAnimatorParameters
    {
        public static string AttackParam => "Attack";
        public static string AttackInProgressParam => "Attacking animation in progress";
        public static string RunParam => "Run";
    }
    
    public class PlayerController : MonoBehaviour, IDamageable
    {
        private PlayerStats playerStats;
        public PlayerMovementController playerMovementController;
        private PlayerAttackController playerAttackController;

        public bool nextFrameDash = false;

        [SerializeField]
        private float playerSpeed = 200f;
        [SerializeField]
        private int maximumHealth;
        [SerializeField]
        private int currentHealth;
        [SerializeField]
        private bool canPlayerMoveWithKeyboard = true;
        [SerializeField]
        private bool isAttacking;

        [SerializeField] 
        private float dashAttackMovePower;

        public Animator CharacterAnimator { get; private set; }
        public Rigidbody rb { get; private set; }
        public Vector3 MovementVector { get; set; }

        public int MaximumHealth { get => maximumHealth; private set => maximumHealth = value; }
        public int CurrentHealth { get => currentHealth; private set => currentHealth = value; }
        public bool CanPlayerMoveWithKeyboard { get => canPlayerMoveWithKeyboard; set => canPlayerMoveWithKeyboard = value; }
        public bool IsAttacking { get => isAttacking; set => isAttacking = value; }
        public float PlayerSpeed { get => playerSpeed; set => playerSpeed = value; }
        public float DashAttackMovePower { get => dashAttackMovePower; set => dashAttackMovePower = value; }

        private void Start()
        {
            CharacterAnimator = GetComponentInChildren<Animator>();
            if (CharacterAnimator == null) Debug.Log("nie znaleziono animatora w postaci gracza");
            AnimatorParametersCheck();
            
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

        private void AnimatorParametersCheck()
        {
            if(CharacterAnimator.parameters.Any(x => x.name == StringAnimatorParameters.AttackParam) == false) 
                Debug.Log("Blad w nazwie parametru atakowania");
            if(CharacterAnimator.parameters.Any(x => x.name == StringAnimatorParameters.AttackInProgressParam) == false)
                Debug.Log("Blad w nazwie parametru progresu animacji atakowania");
            if(CharacterAnimator.parameters.Any(x => x.name == StringAnimatorParameters.RunParam) == false)
                Debug.Log("Blad w nazwie parametru biegania");
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