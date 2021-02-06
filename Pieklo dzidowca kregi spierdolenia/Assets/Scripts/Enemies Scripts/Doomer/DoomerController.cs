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

    DoomerState state;
   
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
        state = DoomerState.Idle;
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

    // Update is called once per frame
    override protected void FixedUpdate()
    {
        switch (state)
        {
            case DoomerState.Idle:
                easyAnimator.SetBooleanTrue("isIdling");
                break;
            case DoomerState.Walk:
                MoveTo(MainCharacterTransform.position, MovementSpeed, AttackRadius);
                easyAnimator.SetBooleanTrue("isWalking");
                break;
            case DoomerState.Run:
                MoveTo(MainCharacterTransform.position, movementRushSpeed, AttackRadius);
                easyAnimator.SetBooleanTrue("isRunning");
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
                }
                break;
            case DoomerState.Attack:

                MoveTo(MainCharacterTransform.position, 0.0f, AttackRadius);

                easyAnimator.SetBooleanTrue("isAttacking");
                //tutaj zadawanie obrażeń - collider i te sprawy
                //dodać tutaj sprawdzenie czy zakończono atak specjalnt i jesli tak to normalne obrażenia a jak nie to dodatkowe obrażenia
                break;
            case DoomerState.Aggro:
                easyAnimator.SetBooleanTrue("isAggroing");
                MoveTo(MainCharacterTransform.position, 0f, AttackRadius);
                break;
            case DoomerState.Dying:
                Die();
                break;
            case DoomerState.GoingToPoint:
                //if this is first frame of state going to point then should save position where go to
                if (!ShouldGoToPoint) SaveAndGoToPoint(MainCharacterTransform.position);

                //if did jumped then go there in normal walking speed
                if (hasDoneSpecialAttack)
                {
                    MoveTo(GoToPoint, MovementSpeed, AttackRadius);
                    easyAnimator.SetBooleanTrue("isWalking");
                }
                //else in charge speed
                else
                {
                    MoveTo(GoToPoint, movementRushSpeed, AttackRadius);
                    easyAnimator.SetBooleanTrue("isRunning");
                }
                //check if should end going to the point
                if (Vector3.Distance(transform.position, GoToPoint) < AttackRadius)
                {
                    ShouldGoToPoint = false;
                    state = DoomerState.Patrol;
                    MultiUseTimer = 0f;
                }
                break;
            case DoomerState.AIOff:
                return;
            default:
                Debug.Log("Some doomer is in weird and unrecognized state");
                break;
        }
    }

    private void Update()
    {
        if (CurrentHealth <= 0)
        {
            state = DoomerState.Dying;
            return;
        }

        //Alpha version of performance booster
        if (framesCounter == updateLogicEveryXFrames)
        {
            framesCounter = 0;
        }
        else
        {
            framesCounter++;
            return;
        }
        
        distanceToMainChar = Vector3.Distance(transform.position, MainCharacterTransform.position);
        playerIsVisible = IsPlayerVisible();

        if (playerIsVisible)
        {
            //in this case it means that player is closer that aggroRadius but further than attackRadius
            if (distanceToMainChar > AttackRadius)
            {
                if (HasDoneAggro)
                {
                    //checking if should start special attack
                    if (distanceToMainChar <= firstAttackRadius && !hasDoneSpecialAttack)
                    {
                        state = DoomerState.ChargedAttack;
                    }
                    else if (hasDoneSpecialAttack)
                    {
                        //if attack has been made just move in normal speed to player
                        state = DoomerState.Walk;
                    }
                    else
                    {
                        //when attack has been not made and cannot be made yet move to player in charge
                        state = DoomerState.Run;
                    }

                }
                //if aggro has not been made then do it
                else
                {
                    state = DoomerState.Aggro;
                }
            }
            //if in range just attack player
            else if (distanceToMainChar <= AttackRadius)
            {
                state = DoomerState.Attack;
            }
        }//if not seeing player check if should go somewhere
        else if (ShouldGoToPoint)
        {
            state = DoomerState.GoingToPoint;
        }
        //it means that player is not visible and dont have to go anywhere now
        else
        {
            //if was not returning back or beeing idle means that i was seeing player 
            //so i should go and check where did he go
            if (playerWasVisible)
            {
                IsPatroling = true;
                state = DoomerState.GoingToPoint;
            }
            else if (IsPatroling)
            {
                state = DoomerState.Patrol;
            }
            //if was returning back and not reached spawn then stay doing this
            else if (Vector3.Distance(transform.position, SpawnPoint) > 2.0f)
            {
                state = DoomerState.Return;
            }
            else
            {
                state = DoomerState.Idle;
            }

        }
        playerWasVisible = playerIsVisible;
    }
    override public void SetDamage(int damageAmount, DamageType damageType)
    {
        if (!HasDoneAggro) HasDoneAggro = true;
        base.SetDamage(damageAmount, damageType);
    }

    private enum DoomerState
    {
        Idle,
        Walk,
        Run,
        ChargedAttack,
        Return,
        Patrol,
        Attack,
        Aggro,
        Dying,
        GoingToPoint,
        AIOff
    }
}
