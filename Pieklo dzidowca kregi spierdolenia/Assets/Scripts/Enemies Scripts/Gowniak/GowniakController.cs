///<summary>
/// Created by Kumdzio
///</summary>


using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;
namespace jbzdy.Enemies
{
    public class GowniakController : EnemyController
    {
        [Header("Gowniak specific")]
        [SerializeField] private float aggroMaxTime = 2f;
        [SerializeField] private float spawnWanderRadius = 15f;
        [SerializeField] private float wanderEveryXSeconds = 3f;
        private static bool aggroCommenced = false;
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

        override protected void Start()
        {
            base.Start();
            easyAnimator = new EasyAnimatorController(GetComponent<Animator>(), new string[] { });
            NavAgent = GetComponent<NavMeshAgent>();
        }

        override protected void Update()
        {
            if (CurrentHealth <= 0 && EnemyAlive)
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
                            GoToPoint = transform.position;
                            currentState = GowniakState.Wander;
                        }
                        if (distanceToMainChar > EnemyData.AttackRadius)
                        {
                            currentState = GowniakState.Chase;
                        }
                    }
                    break;
                case GowniakState.Chase:
                    {
                        MoveTo(EnemyData.MainCharacterTransform.position, EnemyData.MovementSpeed, EnemyData.AttackRadius);
						easyAnimator.SetBooleanTrue("Move");

                        if (!updateLogicFrame) break;

                        if (distanceToMainChar <= EnemyData.AttackRadius)
						{
							currentState = GowniakState.Attack;
							break;
                        }
                        if (!playerIsVisible)
                        {
                            GoToPoint = transform.position;
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

						MoveTo(GoToPoint, EnemyData.MovementSpeed, EnemyData.AttackRadius);
						if (Vector3.Distance(transform.position, GoToPoint) <= EnemyData.AttackRadius)
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
                GoToPoint = GenerateNewDestination(currentState == GowniakState.Wander);
            }
            base.SetDamage(damageAmount, damageType);
        }

        //for further improvement in performance there can be used multitasking
        //and then do not move until worker call the delegate function to move enemy
        private Vector3 GenerateNewDestination(bool WanderTowardsSpawn)
        {
            const int TRYXTIMES = 10;
            int tryCounter = 0;
            Vector3 newPoint = new Vector3(transform.position.x, transform.position.y, transform.position.z);

            if (WanderTowardsSpawn)
            {
                while ((tryCounter < TRYXTIMES) && (Vector3.Distance(newPoint, SpawnPoint) >= Vector3.Distance(transform.position, SpawnPoint)))
                {
                    tryCounter++;
                    newPoint = ChooseNewPatrollingPoint();
                }
            }
            else
            {
                while (tryCounter < TRYXTIMES && Vector3.Distance(newPoint, SpawnPoint) > spawnWanderRadius)
                {
                    tryCounter++;
                    newPoint = ChooseNewPatrollingPoint();
                }
            }

            if (tryCounter == TRYXTIMES)
            {
                if (WanderTowardsSpawn)
                {
                    if (Vector3.Distance(newPoint, SpawnPoint) > Vector3.Distance(transform.position, SpawnPoint))
                    {
                        return transform.position;
                    }
                }
                else
                {
                    if (Vector3.Distance(newPoint, SpawnPoint) > spawnWanderRadius)
                    {
                        return transform.position;
                    }
                }
            }
            return newPoint;
        }
    }
}