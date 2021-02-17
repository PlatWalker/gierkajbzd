///<summary>
/// Created by Szwagier
/// STRONGLY Edited by Kumdzio
///</summary>


using System.Diagnostics;
using UnityEngine;

public class GowniakController : EnemyController
{
    [SerializeField] private float aggroMaxTimeMs = 5000f;
    [SerializeField] private float spawnWanderRadius = 15f;
    [SerializeField] private float maxWanderDistance = 3f;
    [SerializeField] private float minWanderDistance = 1f;

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
            MultiUseTimer = 0f;
            currentState = GowniakState.Dying;
        }

        HandleLogicPerformaceBoost();

        switch (currentState)
        {
            case GowniakState.Aggro:
                {
                    easyAnimator.SetBooleanTrue("Aggro");


                    if (!updateLogicFrame) break;

                    if (aggroTimer != null && aggroTimer.IsRunning && aggroTimer.ElapsedMilliseconds > aggroMaxTimeMs)
                    {
                        aggroCommenced = true;
                        aggroTimer = null;
                    }
                    if (aggroCommenced)
                    {
                        aggroTimer = null;
                        currentState = GowniakState.Chase;
                    }
                    if (!playerIsVisible)
                    {
                        aggroTimer = null;
                        currentState = GowniakState.Wander;
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
                }
                break;
            case GowniakState.Chase:
                {
                    MoveTo(MainCharacterTransform.position, MovementSpeed, AttackRadius);
                    easyAnimator.SetBooleanTrue("Move");

                    if (!updateLogicFrame) break;
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
                }
                break;
            case GowniakState.Wander:
                {
                    //some code to times to times move
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
            while(tryCounter<TryXTimes)
        }
        return newPoint;
    }
}
