using System.Collections;
using System.Collections.Generic;
using jbzd.Enemies;
using jbzd.Enemies.Obsolete;
using UnityEngine;
using UnityEngine.AI;

namespace jbzdy.Enemies
{
    public class OsaController : EnemyController
    {

        private OsaState _currentState;
        [SerializeField] private DamageController _damageController;

        private enum OsaState
        {
            Idle,
            Chase,
            Attack,
        }

        protected override void Start()
        {
            base.Start();
            easyAnimator = new EasyAnimatorController(GetComponent<Animator>(), new string[] {  });
            NavAgent = GetComponent<NavMeshAgent>();
            _damageController = GetComponent<DamageController>();
            _damageController.SetUp(EnemyData.Damage);

        }

        protected override void Update()
        {
            if (CurrentHealth <= 0 && EnemyAlive)
            {
                Die();
            }

            HandleLogicPerformaceBoost();

            switch (_currentState)
            {
                case OsaState.Attack:
                    {

                        MultiUseTimer += Time.deltaTime;
                        var explosion = GetComponent<ParticleSystem>();
                        explosion.Play();

                        if (!updateLogicFrame) break;

                        double estheticDelay = 0.1;

                        if (MultiUseTimer > explosion.main.duration + estheticDelay)
                        {
                            MultiUseTimer = 0;
                            Die();
                        }
                       
                    }
                    break;
                case OsaState.Chase:
                    {

                        MoveTo(EnemyData.MainCharacterTransform.position, EnemyData.MovementSpeed, EnemyData.AttackRadius);

                        if (!updateLogicFrame) break;

                        if (distanceToMainChar <= EnemyData.AttackRadius)
                        {
                            _currentState = OsaState.Attack;
                            break;
                        }
                        if (!playerIsVisible)
                        {
                            GoToPoint = transform.position;
                            _currentState = OsaState.Idle;
                        }
                    }
                    break;
                case OsaState.Idle: 
                    {

                        MoveTo(ChooseNewPatrollingPoint(), EnemyData.MovementSpeed, EnemyData.AttackRadius);

                        if (!updateLogicFrame) break;
                  
                        if (playerIsVisible)
                        {
                            _currentState = OsaState.Chase;
                        }
                    }
                    break;
                default:
                    UnityEngine.Debug.Log("Some Osa is in strange and unrecognized state");
                    break;
            }
        }

        protected override bool HandleTriggerByEnemy(Vector3 target)
        {
            return false;
        }

    }
}
