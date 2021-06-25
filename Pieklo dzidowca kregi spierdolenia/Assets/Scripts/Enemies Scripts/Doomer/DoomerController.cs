///<summary>
///Created by Kumdzio
///</summary>

using UnityEngine;
namespace jbzdy.Enemies
{
    public class DoomerController : EnemyController
    {
        [Header("Special Attack")]
        [SerializeField] private float movementRushSpeed = 1.05f;
        [SerializeField] private float jumpSpeed = 1.08f;
        [SerializeField] private float firstAttackRadius = 4.0f;
		
    [SerializeField] private float chargeDamageModifier = 2.0f;
    [SerializeField] DamageController LHCollider = null;
    [SerializeField] DamageController RHCollider = null;
        private enum DoomerState
        {
            Idle,
			Attack,
			ChargedAttack,
			Chase,
			Return,
			Patrol,
			Aggro,
			Dying,
			HaveSeenPlayer,
			AIOff
		}
		DoomerState currentState;
		DoomerState resumeState;
		private Vector3 jumpDirection = Vector3.zero;
		public static bool HasDoneAggro { get; set; }
		private bool hasDoneSpecialAttack;

		override protected void Start()
		{
			base.Start();
			easyAnimator = new EasyAnimatorController(GetComponent<Animator>(), new string[] {"shouldUseSecondAttack"});
			hasDoneSpecialAttack = false;
			currentState = DoomerState.Idle;
		}

		void OnValidate()
		{
			if(NavAgent) NavAgent.angularSpeed = EnemyData.RotationSpeed; // without this rotation speed change apply only on start
			//there should go some code to check if values inserted in editor are correct
		}



        override protected void Update()
        {
            CurrentAnimation = easyAnimator.GetCurrentTrueBoolean();//for debug purposes only (showing active animation in editor)

            if (CurrentHealth <= 0 && EnemyAlive)
            {
                currentState = DoomerState.Dying;
            }

            HandleLogicPerformaceBoost();

            switch (currentState)
            {
                case DoomerState.Idle:

                    {

                        easyAnimator.SetBooleanTrue("isIdling");

                        if (!updateLogicFrame) break;

                        if (playerIsVisible)
                        {
                            if (HasDoneAggro)
							{
								currentState = DoomerState.Chase;
								TriggerNearEnemies(EnemyData.MainCharacterTransform.position);
							}
							else
							{
								TriggerNearEnemies(transform.position);
								currentState = DoomerState.Aggro;
							}
						}
						 
					}
					break;

				case DoomerState.Attack:
					{
						MoveTo(EnemyData.MainCharacterTransform.position, 0.0f, EnemyData.AttackRadius);
						easyAnimator.SetBooleanTrue("isAttacking");

						AnimatorStateInfo animatorStateInfo = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
						if (animatorStateInfo.IsName("Armature|Atk_2")) //short attack animation
						{
							if (animatorStateInfo.normalizedTime % 1 > 0.9)
							{
								RHCollider.DamageDealed = false;
							}
							if (animatorStateInfo.normalizedTime % 1 > 0.2 && animatorStateInfo.normalizedTime % 1 < 0.3)
							{
								LHCollider.DamageDealed = false;
							}
						}
						else if (animatorStateInfo.IsName("Armature|Atk_1"))//long attack animation
						{

							if (animatorStateInfo.normalizedTime < 0.1)
							{
								RHCollider.DamageDealed = false;
							}
							else if ((animatorStateInfo.normalizedTime > 0.3 && animatorStateInfo.normalizedTime < 0.5) || (animatorStateInfo.normalizedTime > 0.75 && animatorStateInfo.normalizedTime < 0.8))
							{
								LHCollider.DamageDealed = false;
								RHCollider.DamageDealed = false;
							}
							else if (animatorStateInfo.normalizedTime > 0.8 && animatorStateInfo.normalizedTime < 0.9)
							{
								LHCollider.DamageDealed = false;
								RHCollider.DamageDealed = false;
							}

						}
						else //before first attack and after charged attack
						{
							if (animatorStateInfo.normalizedTime < 0.9 && animatorStateInfo.normalizedTime > 0.8)
							{
								LHCollider.DamageDealed = false;
								RHCollider.DamageDealed = false;
							}
						}
						
                        if (!updateLogicFrame) break;

                        if (distanceToMainChar > EnemyData.AttackRadius)
                        {
                            currentState = DoomerState.Chase;
                        }
                    }
                    break;

                case DoomerState.ChargedAttack:
                    {
                        if (jumpDirection == Vector3.zero)
                        {
                            jumpDirection = EnemyData.MainCharacterTransform.position; //saving jump direction so Doomer cannot change direction in air
						}

						if (Vector3.Distance(jumpDirection, gameObject.transform.position) > EnemyData.AttackRadius)
						{
							MoveTo(jumpDirection, jumpSpeed, EnemyData.AttackRadius);
							easyAnimator.SetBooleanTrue("isJumping");
						}
						else
						{
							hasDoneSpecialAttack = true;
						}

                        //here should check if player have been reached and deal damage

						if (!updateLogicFrame) break; 

						if (hasDoneSpecialAttack)
						{
							RHCollider.SetUp(EnemyData.Damage);
							LHCollider.SetUp(EnemyData.Damage);
							RHCollider.DamageDealed = false;
							LHCollider.DamageDealed = false;
							
							if (distanceToMainChar <= EnemyData.AttackRadius)
							{ 
                                currentState = DoomerState.Attack;
                            }
                            else
                            {
                                currentState = DoomerState.Chase;
                            }
                        }
                    }
                    break;

				case DoomerState.Chase:
					{
						if (hasDoneSpecialAttack)
						{
							MoveTo(EnemyData.MainCharacterTransform.position, EnemyData.MovementSpeed, EnemyData.AttackRadius);
							easyAnimator.SetBooleanTrue("isWalking");
						}
						else
						{
							MoveTo(EnemyData.MainCharacterTransform.position, movementRushSpeed, EnemyData.AttackRadius);
							easyAnimator.SetBooleanTrue("isRunning");
						}

                        if (!updateLogicFrame) break;

                        if (!playerIsVisible)
						{
							GoToPoint = EnemyData.MainCharacterTransform.position;
							currentState = DoomerState.HaveSeenPlayer;
							break;
                        }

                        if (hasDoneSpecialAttack)
                        {
                            if (distanceToMainChar <= EnemyData.AttackRadius)
                            {
                                currentState = DoomerState.Attack;
                            }
                        }
                        else
                        {
                            if (distanceToMainChar <= firstAttackRadius)
                            {
                                RHCollider.SetUp((int)(EnemyData.Damage * chargeDamageModifier));
								LHCollider.SetUp((int)(EnemyData.Damage * chargeDamageModifier));
								currentState = DoomerState.ChargedAttack;
                            }
							
                        }
                    }
                    break;

                case DoomerState.Return:
                    {
                        easyAnimator.SetBooleanTrue("isWalking");
                        MoveTo(SpawnPoint, EnemyData.MovementSpeed, EnemyData.AttackRadius);

                        if (!updateLogicFrame) break;

                        if (playerIsVisible)
						{
							currentState = DoomerState.Chase;
							TriggerNearEnemies(EnemyData.MainCharacterTransform.position);
							break;
						}
						if (Vector3.Distance(transform.position, SpawnPoint) < EnemyData.AttackRadius)
						{
							currentState = DoomerState.Idle;
						}
					}
					break;

				case DoomerState.Patrol:
					{
						MultiUseTimer += Time.deltaTime;
						MoveTo(GoToPoint, EnemyData.MovementSpeed, EnemyData.AttackRadius);

						if (Vector3.Distance(transform.position, GoToPoint) <= EnemyData.AttackRadius)
						{
							easyAnimator.SetBooleanTrue("isIdling");
						}
						else
                        {
                            easyAnimator.SetBooleanTrue("isWalking");
                        }

                        if (!updateLogicFrame) break;

                        if (playerIsVisible)
                        {
                            PatrolStepsCounter = 0;
							MultiUseTimer = 0f;
							currentState = DoomerState.Chase;
							TriggerNearEnemies(EnemyData.MainCharacterTransform.position);
							break;
                        }

                        if (MultiUseTimer >= EnemyData.TimeBetweenPatrolSteps)
                        {
                            PatrolStepsCounter++;
                            MultiUseTimer = 0f;
                            GoToPoint = ChooseNewPatrollingPoint();
                        }

                        if (PatrolStepsCounter >= EnemyData.MaxPatrolSteps)
						{
							PatrolStepsCounter = 0;
							MultiUseTimer = 0f;
							currentState = DoomerState.Return;
							break;
						}
					}
					break;

				case DoomerState.Aggro:
					{
						easyAnimator.SetBooleanTrue("isAggroing");
						MoveTo(EnemyData.MainCharacterTransform.position, 0f, EnemyData.AttackRadius);

                        if (!updateLogicFrame) break;

                        if (!playerIsVisible)
						{
							GoToPoint = EnemyData.MainCharacterTransform.position;
							currentState = DoomerState.HaveSeenPlayer;
							break;
                        }
                        if (HasDoneAggro)
                        {
                            currentState = DoomerState.Chase;
							TriggerNearEnemies(EnemyData.MainCharacterTransform.position);
                        }
                    }
                    break;

                case DoomerState.Dying:
                    if (EnemyAlive) TriggerNearEnemies(transform.position);
                    Die();
                    break;

                case DoomerState.HaveSeenPlayer:
					{
						if (hasDoneSpecialAttack)
						{
							MoveTo(GoToPoint, EnemyData.MovementSpeed, EnemyData.AttackRadius);
							easyAnimator.SetBooleanTrue("isWalking");
						}
						else
						{
							MoveTo(GoToPoint, movementRushSpeed, EnemyData.AttackRadius);
							easyAnimator.SetBooleanTrue("isRunning");
						}

                        if (!updateLogicFrame) break;

                        if (playerIsVisible)
                        {
                            currentState = DoomerState.Chase;
							TriggerNearEnemies(EnemyData.MainCharacterTransform.position);
							break;
						}

						if (Vector3.Distance(transform.position, GoToPoint) <= EnemyData.AttackRadius)
						{
							currentState = DoomerState.Patrol;
						}
					}
					break;

                case DoomerState.AIOff:
                    return;

                default:
                    Debug.Log("Some doomer is in werid and unrecognized state");
                    break;
            }
        }

        override public void SwitchAI()
        {
            base.SwitchAI();

            if (turnOffAI)
            {
                resumeState = currentState;
                currentState = DoomerState.AIOff;
            }
            else
            {
                currentState = resumeState;
            }
        }

        protected override bool HandleTriggerByEnemy(Vector3 target)
        {
            if (currentState == DoomerState.Idle || currentState == DoomerState.Patrol || currentState == DoomerState.Return)
            {
                GoToPoint = target;
                currentState = DoomerState.HaveSeenPlayer;
                return true;
            }
            return false;
        }


        override public void SetDamage(int damageAmount, DamageType damageType)
        {
            if (!HasDoneAggro) HasDoneAggro = true;

            if (currentState == DoomerState.Idle || currentState == DoomerState.Patrol || currentState == DoomerState.Return || currentState == DoomerState.HaveSeenPlayer)
            {
                GoToPoint = EnemyData.MainCharacterTransform.position;
				currentState = DoomerState.HaveSeenPlayer;
            }

            base.SetDamage(damageAmount, damageType);
        }


    }
}