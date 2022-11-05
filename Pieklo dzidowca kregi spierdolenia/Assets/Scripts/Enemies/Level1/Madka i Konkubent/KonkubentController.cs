using jbzd.Enemies;
using UnityEngine;
using UnityEngine.AI;
namespace jbzdy.Enemies
{

    public class KonkubentController : EnemyController
    {
        [SerializeField] private DamageController _baseballCollider;
        [SerializeField] private float _chargeSpeedMultiplier = 1.0f;
        [SerializeField] private float _chargeDamageMultiplier = 1.0f;
        private enum KonkubentState
        {
            Idle,
            Attack,
            Chase,
            Charge,
            PrepareCharge,
            EndCharge,
            Return,
            Aggro,
            Dying,
            AIOff
        }

        #region Initialize private variables

        private KonkubentState currentState;
        private KonkubentState resumeState;
        private EnemyDamagedEffect pushBackEffect;
        private float timer;
        private float timeToCharge = 8.0f;
        private float timeOfCharge = 3.0f;
        private bool chargeDone;
        private KonkubentAnimationEvents animationEvents;
        private float idleToAggroWalkUpSpeed = 0.3f;
        private float idleToAggroWalkUpDistance = 2.0f;
        #endregion

        override protected void Start()
        {
            base.Start();
            easyAnimator = new EasyAnimatorController(GetComponent<Animator>(), new string[] { });
            NavAgent = GetComponent<NavMeshAgent>();
            _baseballCollider.SetUp(EnemyData.Damage);
            pushBackEffect = GetComponent<EnemyDamagedEffect>();
            animationEvents = GetComponentInChildren<KonkubentAnimationEvents>();
        }
        override protected void Update()
        {
            timer += Time.deltaTime;

            if (CurrentHealth <= 0 && EnemyAlive)
            {
                currentState = KonkubentState.Dying;
            }

            if (timer > timeToCharge && animationEvents.aggroAnimDone && currentState != KonkubentState.Attack)
            {
                timer = 0.0f;
                currentState = KonkubentState.PrepareCharge;
            }

            HandleLogicPerformaceBoost();

            switch (currentState)
            {
                case KonkubentState.Idle:
                {
                    easyAnimator.SetBooleanTrue("IsIdling");

                    if(!updateLogicFrame) break;
                    if(playerIsVisible)
                    {
                        currentState = KonkubentState.Aggro;
                    }
                }
                break;
                case KonkubentState.Attack:
                {
                    easyAnimator.SetBooleanTrue("IsAttacking");

                    MoveTo(EnemyData.MainCharacterTransform.position, 0.0f, EnemyData.AttackRadius);

                    if(!updateLogicFrame) break;

                    if(animationEvents.baseballHit)
                    {
                        if (chargeDone)
                        {
                            _baseballCollider.SetUp((int)(EnemyData.Damage * _chargeDamageMultiplier));
                            chargeDone = false;
                        }
                        else
                        {
                            _baseballCollider.SetUp(EnemyData.Damage);
                        }
                        _baseballCollider.DamageDealed = false;
                    }

                    if(distanceToMainChar > EnemyData.AttackRadius)
                    {
                        currentState = KonkubentState.Chase;
                        break;
                    }
                }
                break;
                case KonkubentState.PrepareCharge:
                {
                    easyAnimator.SetBooleanTrue("PreparingCharge");
                    MoveTo(EnemyData.MainCharacterTransform.position, 0.0f, 0.0f);

                    if(animationEvents.chargeLoop)
                    {
                        currentState = KonkubentState.Charge;
                    }
                }
                break;
                case KonkubentState.Charge:
                {
                    easyAnimator.SetBooleanTrue("IsCharging");

                    MoveTo(EnemyData.MainCharacterTransform.position, EnemyData.MovementSpeed*_chargeSpeedMultiplier, EnemyData.AttackRadius);

                    if(!updateLogicFrame) break;
                    if(timer > timeOfCharge || distanceToMainChar <= EnemyData.AttackRadius)
                    {
                        timer = 0.0f;
                        currentState = KonkubentState.EndCharge;
                        break;
                    }  
                }
                break;
                case KonkubentState.EndCharge:
                {
                    easyAnimator.SetBooleanTrue("EndCharge");

                    MoveTo(EnemyData.MainCharacterTransform.position, EnemyData.MovementSpeed*_chargeSpeedMultiplier, EnemyData.AttackRadius);

                    chargeDone = true;

                    if(!updateLogicFrame) break;
                    if(distanceToMainChar <= EnemyData.AttackRadius)
					{
						currentState = KonkubentState.Attack;
						break;
                    }
                    if(distanceToMainChar > EnemyData.AttackRadius)
                    {
                        currentState = KonkubentState.Chase;
                        break;
                    }
                }
                break;
                case KonkubentState.Chase:
                {
					easyAnimator.SetBooleanTrue("IsChasing");

                    MoveTo(EnemyData.MainCharacterTransform.position, EnemyData.MovementSpeed, EnemyData.AttackRadius);

                    if (!updateLogicFrame) break;
                    if (distanceToMainChar <= EnemyData.AttackRadius)
					{
						currentState = KonkubentState.Attack;
						break;
                    }
                    if (!playerIsVisible)
                    {
                        GoToPoint = transform.position;
                        currentState = KonkubentState.Return;
                    }
                }
                break;
                case KonkubentState.Return:
                {
                    easyAnimator.SetBooleanTrue("IsReturning");

                    MoveTo(SpawnPoint, EnemyData.MovementSpeed, EnemyData.AttackRadius);

                    if(!updateLogicFrame) break;
                    if(playerIsVisible)
                    {
                        currentState = KonkubentState.Chase;
                        break;
                    }
                    if(Vector3.Distance(transform.position, SpawnPoint) < EnemyData.AttackRadius)
                    {
                        currentState = KonkubentState.Idle;
                    }
                }
                break;
                case KonkubentState.Aggro:
                {
                    easyAnimator.SetBooleanTrue("IsAggroing");
                    
                    if(!updateLogicFrame) break;
                    if(animationEvents.moveForward) 
                    {
                        MoveTo(EnemyData.MainCharacterTransform.position, idleToAggroWalkUpSpeed, idleToAggroWalkUpDistance);
                        animationEvents.moveForward = false;
                    }
                    if(playerIsVisible && animationEvents.aggroAnimDone)
                    {
                        currentState = KonkubentState.Chase;
                    }
                    else
                    {
                        GoToPoint = EnemyData.MainCharacterTransform.position;
                    }
                }
                break;
                case KonkubentState.Dying:
                {
                    Die();
                }
                break;
                case KonkubentState.AIOff:
                {
                    return;
                }

                default:
                {
                    Debug.Log("One or more Konkubent is in undefined state");
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
                currentState = KonkubentState.AIOff;
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
    }
}
