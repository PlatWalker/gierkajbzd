using jbzd.Enemies;
using UnityEngine;
using UnityEngine.AI;
namespace jbzdy.Enemies
{
    public class MadkaController : EnemyController
    {
        [SerializeField] private float _runAwayRadius;
        [Header("Projectile")]
        [SerializeField] private GameObject[] _projectileObject;
        [SerializeField] private Transform _projectileSpawnPoint;
        [SerializeField] private float _throwPower;
        [SerializeField] private float _throwTargetHeight;

        private enum MadkaState
        {
            Idle,
            Attack,
            Chase,
            Move,
            MoveAway,
            Return,
            Dying,
            AIOff
        }

        #region Initialize private variables
        private MadkaState _currentState;
        private MadkaState _resumeState;
        private int _randomInt;
        private EnemyDamagedEffect _pushBackEffect;
        private MadkaAnimationEvents animationEvents;
        #endregion
        
        override protected void Start()
        {
            base.Start();
            easyAnimator = new EasyAnimatorController(GetComponent<Animator>(), new string[] { });
            NavAgent = GetComponent<NavMeshAgent>();
            _pushBackEffect = GetComponent<EnemyDamagedEffect>();
            animationEvents = GetComponentInChildren<MadkaAnimationEvents>();
        }
        override protected void Update()
        {
            if (CurrentHealth <= 0 && EnemyAlive)
            {
                _currentState = MadkaState.Dying;
            }

            HandleLogicPerformaceBoost();

            switch (_currentState)
            {
                case MadkaState.Idle:
                {
                    easyAnimator.SetBooleanTrue("IsIdling");

                    if(!updateLogicFrame) break;
                    if(playerIsVisible)
                    {
                        _currentState = MadkaState.Attack;
                    }
                }
                break;
                case MadkaState.Chase:
                {
                    MoveTo(EnemyData.MainCharacterTransform.position, EnemyData.MovementSpeed, (EnemyData.AttackRadius));
					easyAnimator.SetBooleanTrue("IsMoving");

                    if (!updateLogicFrame) break;

					if (distanceToMainChar <= EnemyData.AttackRadius)
                    {
                        _currentState = MadkaState.Attack;
                    }
                }
                break;
                case MadkaState.Attack:
                {
                    easyAnimator.SetBooleanTrue("IsAttacking");

                    MoveTo(EnemyData.MainCharacterTransform.position, 0.0f, (EnemyData.AttackRadius));
                    HandleThrowing();

                    if(!updateLogicFrame) break;

                    if(animationEvents.throwAnimationFinished)
                    {
                        animationEvents.throwAnimationFinished = false;
                        if(distanceToMainChar > EnemyData.AttackRadius)
                        {
                            _currentState = MadkaState.Move;
                        }
                        if (distanceToMainChar < _runAwayRadius)
                        {
                            _currentState = MadkaState.MoveAway;
                        }
                    }
                }
                break;
                case MadkaState.Move:
                {
					easyAnimator.SetBooleanTrue("IsMoving");
                    MoveTo(EnemyData.MainCharacterTransform.position, EnemyData.MovementSpeed, EnemyData.AttackRadius);

                    if(!updateLogicFrame) break;
                    if(distanceToMainChar < EnemyData.AttackRadius)
                    {
                        _currentState = MadkaState.Attack;
                    }
                }
                break;
                case MadkaState.MoveAway:
                {
					easyAnimator.SetBooleanTrue("IsMoving");

                    var distanceFromPlayer =  transform.position - EnemyData.MainCharacterTransform.position;
                    var howFarToRunAway = _runAwayRadius / distanceFromPlayer.magnitude;
                    // Calculating destination vector. I multiply by at least 1.3, because otherwise the enemy runs in place, I don't know why yet.
                    var runAwayDestination = transform.position + distanceFromPlayer*howFarToRunAway*1.3f;

                    MoveTo(runAwayDestination, EnemyData.MovementSpeed, EnemyData.AttackRadius);

                    if(!updateLogicFrame) break;
                    if(distanceToMainChar > _runAwayRadius)
                    {
                        _currentState = MadkaState.Attack;
                    }
                }
                break;
                case MadkaState.Return:
                {
                    easyAnimator.SetBooleanTrue("IsReturning");

                    MoveTo(SpawnPoint, EnemyData.MovementSpeed, EnemyData.AttackRadius);

                    if(!updateLogicFrame) break;
                    if(playerIsVisible)
                    {
                        _currentState = MadkaState.Move;
                        break;
                    }
                    if(Vector3.Distance(transform.position, SpawnPoint) < EnemyData.AttackRadius)
                    {
                        _currentState = MadkaState.Idle;
                    }
                }
                break;
                case MadkaState.Dying:
                {
                    Die();
                }
                break;
                case MadkaState.AIOff:
                {
                    return;
                }

                default:
                {
                    Debug.Log("One or more Madka is in an undefined state");
                    break;
                }
            }
        }
        protected override bool HandleTriggerByEnemy(Vector3 target)
        {
            return false;
        }

        private void HandleThrowing()
        {
            if (easyAnimator.GetBoolean("spawnProjectile"))
            {
                _randomInt = Random.Range(0, _projectileObject.Length);
				GameObject projectile = Instantiate(_projectileObject[_randomInt], _projectileSpawnPoint.position, _projectileSpawnPoint.rotation);
				Rigidbody rbProjectile = projectile.GetComponent<Rigidbody>();
				Vector3 throwDirection = (EnemyData.MainCharacterTransform.position - projectile.transform.position);
				projectile.AddComponent<DamageController>();
				projectile.GetComponent<DamageController>().SetUp(EnemyData.Damage,DamageType.Dystansowa);
                throwDirection.y += _throwTargetHeight;
                throwDirection *= _throwPower;
                rbProjectile.AddForce(throwDirection, ForceMode.Impulse);
                easyAnimator.SetBooleanDirectly("spawnProjectile", false);
            }
        }

        override public void SwitchAI()
        {
            base.SwitchAI();

            if (turnOffAI)
            {
                _resumeState = _currentState;
                _currentState = MadkaState.AIOff;
            }
            else
            {
                _currentState = _resumeState;
            }
        }

        public override void SetDamage(int damageAmount, DamageType damageType)
        {
            _pushBackEffect.ApplyEffect();
            base.SetDamage(damageAmount, damageType);
        }
    }
}
