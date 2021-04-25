using UnityEngine;
/// <summary>
/// Created By Kumdzio
/// </summary>
namespace jbzdy.Enemies
{
    public class MonkeyController : EnemyController
    {
        [Header("Running from player")]
        [SerializeField] private float runAwayRadius = 5.0f;
        [SerializeField] private float runSpeedModifier = 0.7f;
        [SerializeField] private int maxRunAxisDistance = 5;
        [SerializeField] private float pauseTimeWhenRunning = 2.5f;
        private Vector3 runTarget = Vector3.zero;

        [Header("Projectile")]
        [SerializeField] private GameObject projectileObject = null;
        [SerializeField] private Transform projectileSpawnPoint = null;
        [SerializeField] private float throwPower = 1.0f;
        [SerializeField] private float throwTargetHeight = 1.8f;
        private enum MonkeyState
        {
            Idle,
            Chase,
            Attack,
            Run,
            Search,
            Dying,
            Return,
            AIOff
        }
        private MonkeyState currentState;
        private MonkeyState resumeState;

        override protected void Start()
        {
            base.Start();
            string[] ignoredBooleans = new string[] { "spawnProjectile" };
            easyAnimator = new EasyAnimatorController(GetComponent<Animator>(), ignoredBooleans);
            currentState = MonkeyState.Idle;
        }

        override protected void Update()
        {
            base.Update();

            if (CurrentHealth <= 0 && EnemyAlive)
            {
                currentState = MonkeyState.Dying;
            }

            switch (currentState)
            {
                case MonkeyState.AIOff:
                    return;
                case MonkeyState.Attack:
                    {
                        MoveTo(EnemyData.MainCharacterTransform.position, 0, EnemyData.AttackRadius);
						easyAnimator.SetBooleanTrue("isAttacking");
						HandleThrowing();
	
						if (!updateLogicFrame) break;

                        if (distanceToMainChar > EnemyData.AttackRadius)
                        {
                            currentState = MonkeyState.Chase;
                            break;
                        }

                        if (distanceToMainChar < runAwayRadius)
                        {
                            currentState = MonkeyState.Run;
                        }
                    }
                    break;
                case MonkeyState.Chase:
                    {
                        MoveTo(EnemyData.MainCharacterTransform.position, EnemyData.MovementSpeed, (EnemyData.AttackRadius - 0.5f));
						easyAnimator.SetBooleanTrue("isWalking");

                        if (!updateLogicFrame) break;

                        if (!playerIsVisible)
						{
							currentState = MonkeyState.Search;
							MoveTo(transform.position, EnemyData.MovementSpeed, 1);
							break;
						}

						if (distanceToMainChar <= EnemyData.AttackRadius)
                        {
                            currentState = MonkeyState.Attack;
                        }

                    }
                    break;
                case MonkeyState.Dying:
                    {
                        Die();
                    }
                    break;
                case MonkeyState.Idle:
                    {
                        easyAnimator.SetBooleanTrue("Idle");

                        if (!updateLogicFrame) break;

                        if (playerIsVisible)
                        {
                            currentState = MonkeyState.Chase;
                        }
                    }
                    break;
                case MonkeyState.Run:
                    {
                        if (runTarget == Vector3.zero)
                        {
                            runTarget = ChooseNewPatrollingPoint();
                            MultiUseTimer = 0f;
                        }

                        float distanceToRunTarget = Vector3.Distance(transform.position, runTarget);

                       if (distanceToRunTarget < 0.5f)
						{
							//when waiting to run again just attack player
							MultiUseTimer += Time.deltaTime;
							MoveTo(EnemyData.MainCharacterTransform.position, 0, EnemyData.AttackRadius);
							easyAnimator.SetBooleanTrue("isAttacking");
							HandleThrowing();
                        }
                        else
                        {
                             MoveTo(runTarget, EnemyData.MovementSpeed * runSpeedModifier, 0.5f);
							easyAnimator.SetBooleanTrue("isWalking");
						}	

                        if (MultiUseTimer >= pauseTimeWhenRunning)
                        {
                            runTarget = Vector3.zero;
                        }

                        if (!updateLogicFrame) break;


                        if (distanceToMainChar > EnemyData.AttackRadius)
						{
							MultiUseTimer = 0;
							runTarget = Vector3.zero;
							currentState = MonkeyState.Chase;
							break;
						}

                        if (distanceToMainChar > runAwayRadius && distanceToRunTarget < 0.5f)
                        {
                            MultiUseTimer = 0;
                            runTarget = Vector3.zero;
                            currentState = MonkeyState.Attack;
                        }

                    }
                    break;

                case MonkeyState.Search:

                    {
                        MultiUseTimer += Time.deltaTime;
                        MoveTo(GoToPoint, 0, 0);

                        if (MultiUseTimer < 0.2)
                        {
                            easyAnimator.SetBooleanTrue("isWalking");
                        }
                        else
                        {
                            easyAnimator.SetBooleanTrue("Idle");
                        }

                        if (MultiUseTimer >= EnemyData.TimeBetweenPatrolSteps)
                        {
                            GoToPoint = new Vector3(transform.position.x + Random.Range(-maxRunAxisDistance, maxRunAxisDistance),
                                                     transform.position.y,
                                                     transform.position.z + Random.Range(-maxRunAxisDistance, maxRunAxisDistance));
                            MultiUseTimer = 0;
                            PatrolStepsCounter++;
                        }

                        if (!updateLogicFrame) break;


                        if (playerIsVisible)
                        {
                            MultiUseTimer = 0;
                            PatrolStepsCounter = 0;
                            currentState = MonkeyState.Chase;
                            break;
                        }

                        if (PatrolStepsCounter > EnemyData.MaxPatrolSteps)
						{
							if (Vector3.Distance(transform.position, SpawnPoint) > 1)
                            {
                                currentState = MonkeyState.Return;
                                MultiUseTimer = 0;
                                PatrolStepsCounter = 0;
                            }
                            else
                            {
                                currentState = MonkeyState.Idle;
                                MultiUseTimer = 0;
                                PatrolStepsCounter = 0;
                            }
                        }
                    }
                    break;
                case MonkeyState.Return:
                    {
                        MoveTo(SpawnPoint, EnemyData.MovementSpeed, 1);
                        easyAnimator.SetBooleanTrue("isWalking");

                        if (!updateLogicFrame) break;

                        if (Vector3.Distance(transform.position, SpawnPoint) < 1)
                        {
                            currentState = MonkeyState.Idle;
                            break;
                        }

                        if (playerIsVisible)
                        {
                            currentState = MonkeyState.Chase;
                        }

                    }
                    break;
            }
        }

        private void HandleThrowing()
        {
            if (easyAnimator.GetBoolean("spawnProjectile"))
            {


				GameObject projectile = Instantiate(projectileObject, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
				Rigidbody rigidbody = projectile.GetComponent<Rigidbody>();
				Vector3 throwDirection = (EnemyData.MainCharacterTransform.position - projectile.transform.position);

                throwDirection.y += throwTargetHeight;
                throwDirection *= throwPower;
                rigidbody.AddForce(throwDirection, ForceMode.Impulse);
                easyAnimator.SetBooleanDirectly("spawnProjectile", false);

                //Here add some rotatiom of projectile 
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
                currentState = MonkeyState.AIOff;
            }
            else
            {
                currentState = resumeState;
            }
        }
    }
}