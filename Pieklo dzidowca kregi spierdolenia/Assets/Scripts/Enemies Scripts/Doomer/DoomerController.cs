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


    override protected void Update()
    {
        //debug
        CurrentAnimation = easyAnimator.GetCurrentTrueBoolean();
        
        if (CurrentHealth <= 0 && Alive)
        {
            MultiUseTimer = 0f;
            currentState = DoomerState.Dying;
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

        //code below is strongly undebuggable -you have to remember that distance to main char is 
        //updating/counted again only every (see: updateLogicEveryXFrames) frames
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

                    if (!updateLogicFrame) break; //code below is ignored if this is not update-logic frame

                    if (playerIsVisible)
                    {
                        if (HasDoneAggro)
                        {
                            currentState = DoomerState.Chase;
                            TriggerNearEnemies(MainCharacterTransform.position);
                        }
                        else
                        {
                            TriggerNearEnemies(transform.position);
                            currentState = DoomerState.Aggro;
                        }
                    }
                     
                }
                break;

            case DoomerState.Attack:
                {
                    MoveTo(MainCharacterTransform.position, 0.0f, AttackRadius);
                    easyAnimator.SetBooleanTrue("isAttacking");

                    if (!updateLogicFrame) break; //code below is ignored if this is not update-logic frame

                    if (distanceToMainChar > AttackRadius)
                    {
                        currentState = DoomerState.Chase;
                    }
                }
                break;

            case DoomerState.ChargedAttack:
                {
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

                    if (!updateLogicFrame) break; //code below is ignored if this is not update-logic frame

                    if (hasDoneSpecialAttack)
                    {
                        if (distanceToMainChar <= AttackRadius)
                        {
                            currentState = DoomerState.Attack;
                        }
                        else
                        {
                            currentState = DoomerState.Chase;
                        }
                    }
                }
                break;

            case DoomerState.Chase:
                {
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

                    if (!updateLogicFrame) break; //code below is ignored if this is not update-logic frame

                    if (!playerIsVisible)
                    {
                        GoToPoint = MainCharacterTransform.position;
                        currentState = DoomerState.HaveSeenPlayer;
                        break;
                    }

                    if (hasDoneSpecialAttack)
                    {
                        if (distanceToMainChar <= AttackRadius)
                        {
                            currentState = DoomerState.Attack;
                        }
                    }
                    else
                    {
                        if(distanceToMainChar<= firstAttackRadius)
                        {
                            currentState = DoomerState.ChargedAttack;
                        }
                    }
                }
                break;

            case DoomerState.Return:
                {
                    easyAnimator.SetBooleanTrue("isWalking");
                    MoveTo(SpawnPoint, MovementSpeed, AttackRadius);

                    if (!updateLogicFrame) break; //code below is ignored if this is not update-logic frame

                    if (playerIsVisible)
                    {
                        currentState = DoomerState.Chase;
                        TriggerNearEnemies(MainCharacterTransform.position);
                        break;
                    }
                    if (Vector3.Distance(transform.position, SpawnPoint) < AttackRadius)
                    {
                        currentState = DoomerState.Idle;
                    }
                }
                break;

            case DoomerState.Patrol:
                {
                    MultiUseTimer += Time.deltaTime;
                    MoveTo(GoToPoint, MovementSpeed, AttackRadius);
                    if (Vector3.Distance(transform.position, GoToPoint) <= AttackRadius)
                    {
                        easyAnimator.SetBooleanTrue("isIdling");
                    }
                    else
                    {
                        easyAnimator.SetBooleanTrue("isWalking");
                    }

                    if (!updateLogicFrame) break; //code below is ignored if this is not update-logic frame

                    if (playerIsVisible)
                    {
                        PatrolStepsCounter = 0;
                        MultiUseTimer = 0f;
                        currentState = DoomerState.Chase;
                        TriggerNearEnemies(MainCharacterTransform.position);
                        break;
                    }

                    if (MultiUseTimer >= TimeBetweenPatrolSteps)
                    {
                        PatrolStepsCounter++;
                        MultiUseTimer = 0f;
                        GoToPoint = ChooseNewPatrollingPoint();
                    }

                    if (PatrolStepsCounter >= MaxPatrolSteps)
                    {
                        PatrolStepsCounter = 0;
                        MultiUseTimer = 0f;
                        currentState = DoomerState.Return;
                        break;
                    }
                }
                break;

            case DoomerState.Aggro:
                {
                    easyAnimator.SetBooleanTrue("isAggroing");
                    MoveTo(MainCharacterTransform.position, 0f, AttackRadius);

                    if (!updateLogicFrame) break; //code below is ignored if this is not update-logic frame

                    if (!playerIsVisible)
                    {
                        GoToPoint = MainCharacterTransform.position;
                        currentState = DoomerState.HaveSeenPlayer;
                        break;
                    }
                    if (HasDoneAggro)
                    {
                        currentState = DoomerState.Chase;
                        TriggerNearEnemies(MainCharacterTransform.position);
                    }
                }
                break;

            case DoomerState.Dying:
                if(Alive) TriggerNearEnemies(transform.position);
                Die();
                break;

            case DoomerState.HaveSeenPlayer:
                {
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

                    if (!updateLogicFrame) break; //code below is ignored if this is not update-logic frame

                    if (playerIsVisible)
                    {
                        currentState = DoomerState.Chase;
                        TriggerNearEnemies(MainCharacterTransform.position);
                        break;
                    }
                    if (Vector3.Distance(transform.position, GoToPoint) <= AttackRadius)
                    {
                        currentState = DoomerState.Patrol;
                    }
                }
                break;

            case DoomerState.AIOff:
                return;

            default:
                Debug.Log("Some doomer is in werid and unrecognized state in Update");
                break;
        }
    }

    override public void SwitchAI()
    {
        base.SwitchAI();
        if (turnOffAI)
        {
            resumeState = currentState;
            currentState = DoomerState.AIOff;
        }
        else
        {
            currentState = resumeState;
        }
    }

    protected override bool HandleTriggerByEnemy(Vector3 target)
    {
        if (currentState==DoomerState.Idle||currentState==DoomerState.Patrol||currentState==DoomerState.Return)
        {
            GoToPoint = target;
            currentState = DoomerState.HaveSeenPlayer;
            return true;
        }
        return false;
    }


    override public void SetDamage(int damageAmount, DamageType damageType)
    {
        if (!HasDoneAggro) HasDoneAggro = true;
        if (currentState != DoomerState.ChargedAttack)
        {
            GoToPoint = MainCharacterTransform.position;
            currentState = DoomerState.HaveSeenPlayer;
        }
        base.SetDamage(damageAmount, damageType);
    }

   
}
