///<summary>
/// Created by Szwagier
/// STRONGLY Edited by Kumdzio
///</summary>


using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

public class GowniakController : EnemyController
{
    [SerializeField] private float aggroMaxTime = 2f;
    [SerializeField] private float spawnWanderRadius = 15f;
    [SerializeField] private float maxWanderDistance = 3f;
    [SerializeField] private float minWanderDistance = 1f;
    [SerializeField] private float wanderEveryXSeconds = 3f;

    private static bool aggroCommenced;

    private Stopwatch aggroTimer;

    private float normalSpeedModifier = 1f;

    GowniakState currentState;
    GowniakState resumeState;

    private enum GowniakState
    {
        Idle,
        Aggro,
        Chase,
        Attack,
        Wander,
        Dying,
        AIOff
    }

    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
        easyAnimator = new EasyAnimatorController(GetComponent<Animator>(), new string[] { });
        NavAgent = GetComponent<NavMeshAgent>();
    }

    //OnValidate is called when script is loaded and everytime when value is changed
    //in the inspector
    //void OnValidate()
    //{
    //here will be code to handle debuging and balance changes in values
    //for example there have to be check if the AggroRadius is bigger that AttackRadius etc.
    //}

    // Update is called once per frame
    override protected void Update()
    {
        if (CurrentHealth <= 0 && Alive)
        {
            currentState = GowniakState.Dying;
        }

        HandleLogicPerformaceBoost();

        switch (currentState)
        {
            case GowniakState.Aggro:
                {
                    MultiUseTimer += Time.deltaTime;
                    easyAnimator.SetBooleanTrue("Aggro");


                    if (!updateLogicFrame) break;

                    if (MultiUseTimer > aggroMaxTime)
                    {
                        MultiUseTimer = 0;
                        currentState = GowniakState.Chase;
                    }
                    if (!playerIsVisible)
                    {
                        MultiUseTimer = 0;
                        currentState = GowniakState.Idle;
                    }
                }
                break;
            case GowniakState.AIOff:
                return;
            case GowniakState.Attack:
                {
                    easyAnimator.SetBooleanTrue("Attack");
                    //attack code goes here

                    if (!updateLogicFrame) break;

                    if (!playerIsVisible)
                    {
                        currentState = GowniakState.Wander;
                    }
                    if (distanceToMainChar > AttackRadius)
                    {
                        currentState = GowniakState.Chase;
                    }

                }
                break;
            case GowniakState.Chase:
                {
                    MoveTo(MainCharacterTransform.position, MovementSpeed, AttackRadius);
                    easyAnimator.SetBooleanTrue("Move");

                    if (!updateLogicFrame) break;

                    if (distanceToMainChar <= AttackRadius)
                    {
                        currentState = GowniakState.Attack;
                        break;
                    }
                    if (!playerIsVisible)
                    {
                        currentState = GowniakState.Wander;
                    }
                }
                break;
            case GowniakState.Dying:
                {
                    Die();
                }
                break;
            case GowniakState.Idle:
                {
                    easyAnimator.SetBooleanTrue("Idle");

                    if (!updateLogicFrame) break;

                    if (playerIsVisible)
                    {
                        if (aggroCommenced)
                        {
                            currentState = GowniakState.Chase;
                        }
                        else
                        {
                            currentState = GowniakState.Aggro;
                        }
                    }
                }
                break;
            case GowniakState.Wander:
                {
                    MultiUseTimer += Time.deltaTime;

                    MoveTo(GoToPoint, MovementSpeed, AttackRadius);
                    if (Vector3.Distance(transform.position, GoToPoint) <= AttackRadius)
                    {
                        easyAnimator.SetBooleanTrue("Idle");
                    }
                    else
                    {
                        easyAnimator.SetBooleanTrue("Move");
                    }


                    if (!updateLogicFrame) break;

                    if (playerIsVisible)
                    {
                        MultiUseTimer = 0f;
                        currentState = GowniakState.Chase;
                        break;
                    }

                    if (Vector3.Distance(transform.position, SpawnPoint) <= spawnWanderRadius)
                    {
                        MultiUseTimer = 0f;
                        currentState = GowniakState.Idle;
                        break;
                    }

                    if (MultiUseTimer >= wanderEveryXSeconds)
                    {
                        MultiUseTimer = 0f;
                        GoToPoint = GenerateNewDestination(true);
                    }
                }
                break;
            default:
                UnityEngine.Debug.Log("Some Gowniak is in strange and unrecognized state");
                break;
        }
    }

    protected override bool HandleTriggerByEnemy(Vector3 target)
    {
        return false;
    }

    override public void SwitchAI()
    {
        base.SwitchAI();
        if (turnOffAI)
        {
            resumeState = currentState;
            currentState = GowniakState.AIOff;
        }
        else
        {
            currentState = resumeState;

        }
    }

    override public void SetDamage(int damageAmount, DamageType damageType)
    {
        if (currentState == GowniakState.Idle || currentState == GowniakState.Wander)
        {
            GoToPoint = GenerateNewDestination(currentState==GowniakState.Wander);
        }
        base.SetDamage(damageAmount, damageType);
    }

    private Vector3 GenerateNewDestination(bool WanderTowardsSpawn)
    {
        const int TryXTimes = 10;
        int tryCounter = 0;
        Vector3 newPoint = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        if (WanderTowardsSpawn)
        {
            while (tryCounter < TryXTimes && Vector3.Distance(newPoint, SpawnPoint) > Vector3.Distance(transform.position,SpawnPoint))
            {
                tryCounter++;
                newPoint.x = transform.position.x + Random.Range(-maxWanderDistance, maxWanderDistance);
                newPoint.z = transform.position.z + Random.Range(-maxWanderDistance, maxWanderDistance);
            }
        }
        else
        {
            while (tryCounter < TryXTimes && Vector3.Distance(newPoint,SpawnPoint)>spawnWanderRadius)
            {
                tryCounter++;
                newPoint.x = transform.position.x + Random.Range(-maxWanderDistance, maxWanderDistance);
                newPoint.z = transform.position.z + Random.Range(-maxWanderDistance, maxWanderDistance);
            }
        }

        //if reached final iteration, check if the newPoint is correct
        if (tryCounter == TryXTimes)
        {
            if (WanderTowardsSpawn)
            {
                if (Vector3.Distance(newPoint, SpawnPoint) > Vector3.Distance(transform.position, SpawnPoint))
                {
                    newPoint = transform.position;
                }
            }
            else
            {
                if(Vector3.Distance(newPoint, SpawnPoint) > spawnWanderRadius)
                {
                    newPoint = transform.position;
                }
            }
        }
        return newPoint;
    }
}
