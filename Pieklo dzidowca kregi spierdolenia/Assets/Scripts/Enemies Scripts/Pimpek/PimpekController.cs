using System;
using System.Collections;
using System.Numerics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Vector3 = UnityEngine.Vector3;

namespace jbzdy.Enemies
{
    public class PimpekController : EnemyController
    {
        protected override bool HandleTriggerByEnemy(Vector3 target)
        {
            return false;
        }

        private enum PimpekState
        {
            Idle,
            Chase,
            Attack,
            Search,
            Dying,
            Return,
            AIOff
        }

        #region Variables in inspector
        
        [Header("Game design variables")]
        
        [SerializeField]
        private float criticalChance;
        [SerializeField]
        private float criticalMultiplier;
        [SerializeField]
        private float jumpDistance = 1;
        [SerializeField]
        private int maxRunAxisDistance = 5;
        [SerializeField][Range(0,10)]
        private int distanceToLandBehindPlayer = 1;
        [SerializeField][Range(0,5)]
        private float waitAfterAttack = 1;

        #endregion

        #region Variables with gameobjects

        private EnemyDamagedEffect pushBackEffect;
        private Collider enemyCollider;
        private PimpekAnimationEvents animationEvents;

        #endregion
        
        [SerializeField]
        private PimpekState currentState;
        
        protected override void Start()
        {
            base.Start();
            string[] ignoredBooleans = { };
            easyAnimator = new EasyAnimatorController(GetComponentInChildren<Animator>(), ignoredBooleans);
            currentState = PimpekState.Idle;
            enemyCollider = GetComponent<Collider>();
            animationEvents = GetComponentInChildren<PimpekAnimationEvents>();
            pushBackEffect = GetComponent<EnemyDamagedEffect>();
        }

        protected override void Update()
        {
            base.Update();
            
            if (CurrentHealth <= 0 && EnemyAlive) currentState = PimpekState.Dying;
            switch (currentState)
            {
                case PimpekState.Idle:
                    easyAnimator.SetBooleanTrue("Idle");
                    
                    if (!updateLogicFrame) break;
                    
                    if (IsPlayerVisible())
                    {
                        currentState = PimpekState.Chase;
                    }
                    break;
                case PimpekState.Chase:
                    MoveTo(EnemyData.MainCharacterTransform.position, EnemyData.MovementSpeed, (EnemyData.AttackRadius - 0.5f));
                    easyAnimator.SetBooleanTrue("isWalking");

                    if (!updateLogicFrame) break;

                    if (!IsPlayerVisible())
                    {
                        currentState = PimpekState.Search;
                        break;
                    }

                    if (distanceToMainChar <= EnemyData.AttackRadius)
                    {
                        currentState = PimpekState.Attack;
                        NavAgent.isStopped = true;
                    }
                    
                    break;
                case PimpekState.Attack:
                    easyAnimator.SetBooleanTrue("isJumpAttacking");
                    if (animationEvents.startJumping)
                    {
                        NavAgent.isStopped = false;
                        animationEvents.startJumping = false;
                        enemyCollider.isTrigger = true;
                        var playerPosition = EnemyData.MainCharacterTransform.position;
                        var direction = (playerPosition - transform.position).normalized;
                        var cameraDestination = playerPosition + (direction * distanceToLandBehindPlayer);
                        MoveTo(cameraDestination, jumpDistance, 3);
                    }

                    if (IsDestinationReached())
                    {
                        MultiUseTimer += Time.deltaTime;
                        enemyCollider.isTrigger = false;
                    }

                    if (MultiUseTimer > waitAfterAttack)
                    {
                        currentState = PimpekState.Chase;
                        MultiUseTimer = 0;
                    }
                    
                    break;
                case PimpekState.Search:
                     MultiUseTimer += Time.deltaTime;
                     MoveTo(GoToPoint, 0, 0);

                     easyAnimator.SetBooleanTrue(MultiUseTimer < 0.2 ? "isWalking" : "Idle");

                     if (MultiUseTimer >= EnemyData.TimeBetweenPatrolSteps)
                     {
                         GoToPoint = new Vector3(
                             transform.position.x + UnityEngine.Random.Range(-maxRunAxisDistance, maxRunAxisDistance),
                             transform.position.y,
                             transform.position.z + UnityEngine.Random.Range(-maxRunAxisDistance, maxRunAxisDistance));
                         MultiUseTimer = 0;
                         PatrolStepsCounter++;
                     }

                     if (!updateLogicFrame) break;
                     
                     if (playerIsVisible)
                     {
                         MultiUseTimer = 0;
                         PatrolStepsCounter = 0;
                         currentState = PimpekState.Chase;
                         break;
                     }

                     if (PatrolStepsCounter > EnemyData.MaxPatrolSteps)
                     {
                         if (Vector3.Distance(transform.position, SpawnPoint) > 1)
                         {
                             currentState = PimpekState.Return;
                             MultiUseTimer = 0;
                             PatrolStepsCounter = 0;
                         }
                         else
                         {
                             currentState = PimpekState.Idle;
                             MultiUseTimer = 0;
                             PatrolStepsCounter = 0;
                         }
                     }
                     break;
                case PimpekState.Dying:
                    Die();
                    break;
                case PimpekState.Return:
                    MoveTo(SpawnPoint, EnemyData.MovementSpeed, 1);
                    easyAnimator.SetBooleanTrue("isWalking");

                    if (!updateLogicFrame) break;

                    if (Vector3.Distance(transform.position, SpawnPoint) < 1)
                    {
                        currentState = PimpekState.Idle;
                        break;
                    }

                    if (playerIsVisible)
                    {
                        currentState = PimpekState.Chase;
                    }
                    break;
                case PimpekState.AIOff:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.TryGetComponent<IDamageable>(out var hitObjectScript)) return;
            if (other.transform.CompareTag("Enemy")) return;
            //Debug.Log("hit made by: " + transform.name + " to: " + collision.gameObject.name);
            hitObjectScript.SetDamage(EnemyData.Damage, DamageType.CloseCombat, criticalMultiplier , criticalChance);
        }
    }
}
