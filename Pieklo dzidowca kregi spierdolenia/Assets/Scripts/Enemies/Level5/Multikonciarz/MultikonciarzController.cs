// Developped by Vhart

using jbzd.Enemies;
using UnityEngine;
using UnityEngine.AI;
namespace jbzdy.Enemies
{
    public class MultikonciarzController : EnemyController
    {
        [SerializeField] DamageController RHCollider;
		[SerializeField] DamageController LHCollider;
        private enum MultikonciarzState
        {
            Idle,
            Attack,
            Chase,
            Return,
            Aggro,
            Dying,
            AIOff
        }
        private MultikonciarzState currentState;
        private MultikonciarzState resumeState;
        private EnemyDamagedEffect pushBackEffect;
        public GameObject MultikontoPrefab;
        public GameObject PoisonPoolPrefab;
        protected override void Start()
        {
            base.Start();
            easyAnimator = new EasyAnimatorController(GetComponent<Animator>(), new string[] { });
            NavAgent = GetComponent<NavMeshAgent>();
			RHCollider.SetUp(EnemyData.Damage);
			LHCollider.SetUp(EnemyData.Damage);
            pushBackEffect = GetComponent<EnemyDamagedEffect>();
        }
        protected override void Update()
        {
            if (CurrentHealth <= 0 && EnemyAlive)
            {
                currentState = MultikonciarzState.Dying;
            }

            HandleLogicPerformaceBoost();

            switch (currentState)
            {
                case MultikonciarzState.Idle:
                {
                    easyAnimator.SetBooleanTrue("IsIdling");
                    
                    if(!updateLogicFrame) break;
                    if(playerIsVisible)
                    {
                        currentState = MultikonciarzState.Chase;
                    }
                }
                break;
                case MultikonciarzState.Attack:
                {
                    easyAnimator.SetBooleanTrue("IsAttacking");
                    MoveTo(EnemyData.MainCharacterTransform.position, 0.0f, EnemyData.AttackRadius);

                    if(!updateLogicFrame) break;
                    if(distanceToMainChar > EnemyData.AttackRadius)
                    {
                        currentState = MultikonciarzState.Chase;
                        break;
                    }
                    
                }
                break;
                case MultikonciarzState.Chase:
                {
					easyAnimator.SetBooleanTrue("Move");
                    MoveTo(EnemyData.MainCharacterTransform.position, EnemyData.MovementSpeed, EnemyData.AttackRadius);

                    if (!updateLogicFrame) break;
                    if (distanceToMainChar <= EnemyData.AttackRadius)
					{
						currentState = MultikonciarzState.Attack;
						break;
                    }
                    if (!playerIsVisible)
                    {
                        GoToPoint = transform.position;
                        currentState = MultikonciarzState.Return;
                    }
                }
                break;
                case MultikonciarzState.Return:
                {
                    easyAnimator.SetBooleanTrue("IsWalking");
                    MoveTo(SpawnPoint, EnemyData.MovementSpeed, EnemyData.AttackRadius);

                    if(!updateLogicFrame) break;
                    if(playerIsVisible)
                    {
                        currentState = MultikonciarzState.Chase;
                        break;
                    }
                    if(Vector3.Distance(transform.position, SpawnPoint) < EnemyData.AttackRadius)
                    {
                        currentState = MultikonciarzState.Idle;
                    }
                }
                break;
                case MultikonciarzState.Aggro:
                {
                    easyAnimator.SetBooleanTrue("IsAggroing");

                    if(!updateLogicFrame) break;
                    if(playerIsVisible)
                    {
                        currentState = MultikonciarzState.Chase;
                    }
                    else
                    {
                        GoToPoint = EnemyData.MainCharacterTransform.position;
                        break;
                    }
                }
                break;
                case MultikonciarzState.Dying:
                {
                    Die();
                }
                break;
                case MultikonciarzState.AIOff:
                {
                    return;
                }

                default:
                {
                    Debug.Log("One or more Multikonciarz is in undefined state");
                    break;
                }
            }
        }
        protected override bool HandleTriggerByEnemy(Vector3 target)
        {
            return false;
        }

        public override void SwitchAI()
        {
            base.SwitchAI();

            if (turnOffAI)
            {
                resumeState = currentState;
                currentState = MultikonciarzState.AIOff;
            }
            else
            {
                currentState = resumeState;
            }
        }

        public override void SetDamage(int damageAmount, DamageType damageType)
        {
            pushBackEffect.ApplyEffect();
            base.SetDamage(damageAmount, damageType);
        }

        public void SpawnMultikonto() 
        {
            for (int i = 0; i < 3; i++)
            {
                var position = transform.position + new Vector3(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f));
                Instantiate(MultikontoPrefab, position, transform.rotation);
            }
        }

        public void SpawnPoisonPool()
        {
            var position = transform.position;
            Instantiate(PoisonPoolPrefab, position, transform.rotation);
        }

        public void DamageRH()
        {
            RHCollider.DamageDealed = false;
        }
        public void DamageLH()
        {
            LHCollider.DamageDealed = false;
        }
    }
}