///<summary>
///Created by Kumdzio
///</summary>

using UnityEngine;

public class DoomerController : EnemyController
{
    [Header("Special Attack")]
    [SerializeField] private float movementRushSpeed=1.05f;

    [SerializeField] private float jumpSpeed = 1.08f;

    [SerializeField] private float firstAttackRadius = 4.0f;

    private enum DoomerState
    {
        Idle,
        Attack,
        ChargedAttack,
        Chase,
        Return,
        Patrol,
        Aggro,
        Dying,
        HaveSeenPlayer,
        AIOff
    }

    DoomerState currentState;
    DoomerState resumeState;
   
    private Vector3 jumpDirection = Vector3.zero;

    public static bool HasDoneAggro { get; set; }
    private bool hasDoneSpecialAttack;

    private float distanceToMainChar;
    private bool playerIsVisible;
    private bool playerWasVisible;



    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
        easyAnimator = new EasyAnimatorController(GetComponent<Animator>(), new string[] {"shouldUseSecondAttack"});
        hasDoneSpecialAttack = false;
        currentState = DoomerState.Idle;
        NavAgent.angularSpeed = RotationSpeed;
        NavAgent.acceleration = 100;
    }

    //OnValidate is called when script is loaded and everytime when value is changed
    //in the inspector
    override protected void OnValidate()
    {
        base.OnValidate();
        if(NavAgent) NavAgent.angularSpeed = RotationSpeed;

        //dodać sprawdzanie
    }


    private void Update()
    {
        if (CurrentHealth <= 0)
        {
            MultiUseTimer = 0f;
            currentState = DoomerState.Dying;
            return;
        }

        //Beta version of performance booster
        bool updateLogicFrame = false;
        if (framesCounter == updateLogicEveryXFrames)
        {
            framesCounter = 0;
            updateLogicFrame = true;
        }
        else
        {
            framesCounter++;
        }

        if (updateLogicFrame)
        {
            distanceToMainChar = Vector3.Distance(transform.position, MainCharacterTransform.position);
            playerIsVisible = IsPlayerVisible();
        }

        switch (currentState)
        {
            case DoomerState.Idle:
                {
                    easyAnimator.SetBooleanTrue("isIdling");

                    if (!updateLogicFrame) break;

                    if (playerIsVisible)
                    

                    break;
                }

            case DoomerState.Attack:
                MoveTo(MainCharacterTransform.position, 0.0f, AttackRadius);

                easyAnimator.SetBooleanTrue("isAttacking");
                break;

            case DoomerState.ChargedAttack:
                if (jumpDirection == Vector3.zero)
                {
                    jumpDirection = MainCharacterTransform.position; //saving jump direction so enemy cannot change direction in air

                }

                if (Vector3.Distance(jumpDirection, gameObject.transform.position) > AttackRadius)
                {
                    MoveTo(jumpDirection, jumpSpeed, AttackRadius);
                    easyAnimator.SetBooleanTrue("isJumping");
                }
                else
                {
                    hasDoneSpecialAttack = true;
                }
                break;

            case DoomerState.Chase:
                if (hasDoneSpecialAttack)
                {
                    MoveTo(MainCharacterTransform.position, MovementSpeed, AttackRadius);
                    easyAnimator.SetBooleanTrue("isWalking");
                }
                else 
                {
                    MoveTo(MainCharacterTransform.position, movementRushSpeed, AttackRadius);
                    easyAnimator.SetBooleanTrue("isRunning");
                }
                
                break;

            case DoomerState.Return:
                easyAnimator.SetBooleanTrue("isWalking");
                MoveTo(SpawnPoint, MovementSpeed, AttackRadius);
                break;

            case DoomerState.Patrol:
                MultiUseTimer += Time.deltaTime;

                if (MultiUseTimer >= TimeBetweenPatrolSteps)
                {
                    PatrolStepsCounter++;
                    SaveAndGoToPoint(ChooseNewPatrollingPoint());
                }

                if (PatrolStepsCounter >= MaxPatrolSteps)
                {
                    PatrolStepsCounter = 0;
                    IsPatroling = false;
                    MultiUseTimer = 0f;
                }
                break;

            case DoomerState.Aggro:
                easyAnimator.SetBooleanTrue("isAggroing");
                MoveTo(MainCharacterTransform.position, 0f, AttackRadius);
                break;

            case DoomerState.Dying:
                Die();
                break;

            case DoomerState.HaveSeenPlayer:
                if (hasDoneSpecialAttack)
                {
                    MoveTo(GoToPoint, MovementSpeed, AttackRadius);
                    easyAnimator.SetBooleanTrue("isWalking");
                }
                else
                {
                    MoveTo(GoToPoint, movementRushSpeed, AttackRadius);
                    easyAnimator.SetBooleanTrue("isRunning");
                }
                break;

            case DoomerState.AIOff:
                return;

            default:
                Debug.Log("Some doomer is in werid and unrecognized state in Update");
                break;
        }

        if(updateLogicFrame) playerWasVisible = playerIsVisible;
    }
    override public void SetDamage(int damageAmount, DamageType damageType)
    {
        if (!HasDoneAggro) HasDoneAggro = true;
        base.SetDamage(damageAmount, damageType);
    }

   
}
