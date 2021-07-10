using UnityEngine;

/// <summary>
/// By Tails, Edited by Silver
/// </summary>

public class PlayerController : MonoBehaviour , IDamageable
{
    public Animator characterAnimator;
    private PlayerMovementController playerMovementController;
    private PlayerAttackController playerAttackController;

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
        playerAttackController = new PlayerAttackController(this);
    }

    private void Update()
    {
        playerMovementController.UpdateCharacterMovement();

        playerAttackController.UpdateCharacterAttack();
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



