// Developped by Vhart

using jbzd.Enemies;
using jbzd.Enemies.Obsolete;
using UnityEngine;
using UnityEngine.AI;
namespace jbzdy.Enemies
{
    public class MultikontoController : EnemyController
    {
        [SerializeField] DamageController RHCollider;
        private enum MultikontoState
        {
            Summon,
            Idle,
            Attack,
            Chase,
            Return,
            Aggro,
            Dying,
            AIOff
        }
        private MultikontoState currentState;
        private MultikontoState resumeState;

        private EnemyDamagedEffect pushBackEffect;

        override protected void Start()
        {
            base.Start();
            easyAnimator = new EasyAnimatorController(GetComponent<Animator>(), new string[] { });
            NavAgent = GetComponent<NavMeshAgent>();
			RHCollider.SetUp(EnemyData.Damage);
            pushBackEffect = GetComponent<EnemyDamagedEffect>();
        }

        override protected void Update()
        {
            if (CurrentHealth <= 0 && EnemyAlive)
            {
                currentState = MultikontoState.Dying;
            }

            HandleLogicPerformaceBoost();

            switch (currentState)
            {
                case MultikontoState.Summon:
                {
                    easyAnimator.SetBooleanTrue("Summon");
                    if(!updateLogicFrame) break;

                    float clipTime = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime;
					if (clipTime > 0.99f)
					{
                        currentState = MultikontoState.Idle;
                    }
                }
                break;
                case MultikontoState.Idle:
                {
                    easyAnimator.SetBooleanTrue("IsIdling");
                    if(!updateLogicFrame) break;
                    if(playerIsVisible)
                    {
                        currentState = MultikontoState.Chase;
                    }
                }
                break;
                case MultikontoState.Attack:
                {
                    easyAnimator.SetBooleanTrue("IsAttacking");
                    MoveTo(EnemyData.MainCharacterTransform.position, 0.0f, EnemyData.AttackRadius);

                    if(!updateLogicFrame) break;
                    if(distanceToMainChar > EnemyData.AttackRadius)
                    {
                        currentState = MultikontoState.Chase;
                        break;
                    }
                    
                }
                break;
                case MultikontoState.Chase:
                {
                    MoveTo(EnemyData.MainCharacterTransform.position, EnemyData.MovementSpeed, EnemyData.AttackRadius);
					easyAnimator.SetBooleanTrue("Move");

                    if (!updateLogicFrame) break;
                    if (distanceToMainChar <= EnemyData.AttackRadius)
					{
						currentState = MultikontoState.Attack;
						break;
                    }
                    if (!playerIsVisible)
                    {
                        GoToPoint = transform.position;
                        currentState = MultikontoState.Return;
                    }
                }
                break;
                case MultikontoState.Return:
                {
                    easyAnimator.SetBooleanTrue("IsWalking");
                    MoveTo(SpawnPoint, EnemyData.MovementSpeed, EnemyData.AttackRadius);

                    if(!updateLogicFrame) break;
                    if(playerIsVisible)
                    {
                        currentState = MultikontoState.Chase;
                        break;
                    }
                    if(Vector3.Distance(transform.position, SpawnPoint) < EnemyData.AttackRadius)
                    {
                        currentState = MultikontoState.Idle;
                    }
                }
                break;
                case MultikontoState.Aggro:
                {
                    easyAnimator.SetBooleanTrue("IsAggroing");

                    if(!updateLogicFrame) break;
                    if(playerIsVisible)
                    {
                        currentState = MultikontoState.Chase;
                    }
                    else
                    {
                        GoToPoint = EnemyData.MainCharacterTransform.position;
                        break;
                    }
                }
                break;
                case MultikontoState.Dying:
                {
                    Die();
                }
                break;
                case MultikontoState.AIOff:
                {
                    return;
                }

                default:
                {
                    Debug.Log("One or more Multikonto is in undefined state");
                    break;
                }
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
                currentState = MultikontoState.AIOff;
            }
            else
            {
                currentState = resumeState;
            }
        }

        public override void SetDamage(int damageAmount, Vector3 damageOriginPoint)
        {
            pushBackEffect.ApplyEffect();
            base.SetDamage(damageAmount, damageOriginPoint);
        }

        public void Damage()
        {
            RHCollider.DamageDealed = false;
        }
    }
}