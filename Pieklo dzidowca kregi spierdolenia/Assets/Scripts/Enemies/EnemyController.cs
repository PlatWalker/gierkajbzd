using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Created by Kumdzio.
/// Abstract class with all needed tolls for simple AI.
/// </summary>
namespace jbzdy.Enemies
{
	public abstract class EnemyController : MonoBehaviour, IDamageable
	{
		[SerializeField] protected EnemyDataContainer _enemyData;
        public EnemyDataContainer EnemyData => _enemyData;

        [Header("Artifical Intelligence")]
		[SerializeField] protected bool turnOffAI = false;
		public bool EnemyAlive { get; protected set; }
		protected float MultiUseTimer { get; set; }
		protected int framesCounter;
		protected float distanceToMainChar;
		protected bool playerIsVisible;
		protected bool updateLogicFrame;
		protected string CurrentAnimation; //debug only
		public Vector3 GoToPoint { get; protected set; }
		public NavMeshAgent NavAgent { get; protected set; }
		public int PatrolStepsCounter { get; protected set; }
		public virtual int CurrentHealth { get; protected set; }
		public virtual Vector3 SpawnPoint { get; protected set; }
		public virtual int MaximumHealth => EnemyData.MaxHealth;
		protected  EasyAnimatorController easyAnimator;
		private float animationPlayPreviousSpeed = 0f;


        [Header("Sounds")]
        protected EnemySoundController soundController;

        public delegate void EnemyDied(EnemyController enemy);
        public static event EnemyDied OnDeath;


        protected virtual void Start()
		{
			/*comment below is showing only how to initialize easyAniamtorController
			 * string[] ignoredBooleans = new string[] { "ignoredBooleanName1", "ignoredBooleanName2" };
			 * easyAnimator = new EasyAnimatorController(GetComponent<Animator>(), ignoredBooleans);
			 */
			SpawnPoint = new Vector3(transform.position.x,
									transform.position.y,
									transform.position.z);
			CurrentHealth = EnemyData.MaxHealth;
			EnemyAlive = true;
			GetComponentInChildren<Animator>().SetFloat("IdleSpeedMultiplier", Random.Range(0.900001f, 1.100001f));

			NavAgent = GetComponent<NavMeshAgent>();
			NavAgent.angularSpeed = EnemyData.RotationSpeed;
			NavAgent.acceleration = 100;
            NavAgent.stoppingDistance = 0.2f;

            soundController = GetComponent<EnemySoundController>();
            if (soundController == null)
            {
                Debug.Log("Some enemy does not have Sound Controller - that may cause problems.");
            }
		}
        protected virtual void Update()
        {
            HandleLogicPerformaceBoost();
        }

        /// <summary>
        /// Method to set destination point for enemy.
        /// Just rotate works only if you call this method every frame.
        /// </summary>
        /// <param name="target">Destination point of path</param>
        /// <param name="speed">Speed of travel. 0 = just rotate</param>
        protected virtual void MoveTo(Vector3 target, float speed, float stopDistance)
        {
	        //if (Vector3.Distance(target, NavAgent.destination) < 1f) return;

            if (NavAgent.radius*2 <= stopDistance)
            {
                stopDistance -= NavAgent.radius*2;
            }
            else
            {
                stopDistance = 0f;
            }

            if (NavAgent.stoppingDistance != stopDistance)
            {
                NavAgent.stoppingDistance = stopDistance;
            }


            if (speed > 0)
            {
                NavAgent.speed = speed;
                NavAgent.SetDestination(target);
            }
            else
            {
                Vector3 direction = new Vector3(target.x - transform.position.x,
									0,
									target.z - transform.position.z);
				direction = Vector3.Normalize(direction);
				Vector3 newDirection = Vector3.RotateTowards(gameObject.transform.forward, direction, EnemyData.RotationSpeed /10000, 0.0f);
				transform.rotation = Quaternion.LookRotation(newDirection);
			}
		}

        /// <summary>
        /// Method to handle receiving damage with type of this damage. 
        /// </summary>
        /// <param name="damageAmount"> Amount of received damage.</param>
        /// <param name="damageType"> Type of received damage.</param>
        public virtual void SetDamage(int damageAmount, DamageType damageType)
        {
            //for now damage types are ignored
            CurrentHealth -= damageAmount;
            if(soundController != null) soundController.PlayDamaged();
        }

        /// <summary>
        /// Method to handle receiving damage with type of this damage and handling Crit Ratio.
        /// </summary>
        /// <param name="damageAmount"> Amount of received damage.</param>
        /// <param name="damageType"> Type of received damage.</param>
        /// <param name="criticalMultiplier"> Determines how much the damage is multiplied.</param>
        /// <param name="criticalChance"> What is the chance that critical hit will land. Have to be in range 0f-1f.</param>
        public virtual void SetDamage(int damageAmount, DamageType damageType, float criticalMultiplier, float criticalChance)
        {
            if (Random.Range(0.0f, 1.0f) <= criticalChance)
            {
                damageAmount = (int)(damageAmount * criticalMultiplier);
            }
            this.SetDamage(damageAmount, damageType);
        }
		
        /// <summary>
        /// Method return if destination is reached by navAgent. It's upgrading a property NavAgent.remainingDistance
        /// which not working 100 % properly. 
        /// </summary>
        /// <returns></returns>
        protected bool IsDestinationReached()
        {
	        if (NavAgent.pathPending) return false;
	        
	        if (!(NavAgent.remainingDistance <= NavAgent.stoppingDistance)) return false;
	        
	        return !NavAgent.hasPath || NavAgent.velocity.sqrMagnitude == 0f;
        }
        
        /// <summary>
		/// Method to pause enemy AI
		/// </summary>
		public virtual void SwitchAI()
		{
			turnOffAI = !turnOffAI;
			float temp = animationPlayPreviousSpeed;
			animationPlayPreviousSpeed = GetComponent<Animator>().speed;
			GetComponent<Animator>().speed = temp;
		}

		/// <summary>
		/// Method that is checking if player is visible in the aggro radius for enemy who is calling this method.
		/// </summary>
		/// <returns>True if player is visible otherwise false</returns>
		protected bool IsPlayerVisible()
		{
			RaycastHit hit;
			LayerMask NotEnemiesMask = ~LayerMask.GetMask("Enemies");

			if (Physics.Raycast((transform.position + new Vector3(0f, 1f, 0f)), (EnemyData.MainCharacterTransform.position - transform.position), out hit, EnemyData.AggroRadius, NotEnemiesMask))
			{
				if (hit.transform == EnemyData.MainCharacterTransform)
                {
                    return true;
                }
            }
            return false;
        }

        protected Vector3 ChooseNewPatrollingPoint()
        {
            Vector3 newPoint = Vector3.zero;
            bool correctPoint = false;
            RaycastHit hit;

            for (int i = 5; i > 0; i--)

            {
                newPoint = new Vector3( Random.Range(transform.position.x - EnemyData.PatrolMaxDistance,transform.position.x + EnemyData.PatrolMaxDistance),
										transform.position.y,
										Random.Range(transform.position.z - EnemyData.PatrolMaxDistance,transform.position.z + EnemyData.PatrolMaxDistance));

				if (Physics.Raycast((transform.position + new Vector3(0f, 1f, 0f)), (newPoint - transform.position), out hit, EnemyData.AggroRadius))
                {
                    if (hit.collider.transform.tag == "Terrain")
                    {
                        correctPoint = true;
                        break;
                    }
                }
                else
                {
                    //Dont know why but collinding with terrain dont return its tag
                    correctPoint = true;
                    break;
                }
            }
            if (!correctPoint) newPoint = transform.position;
            return newPoint;
        }
		/// <summary>
		/// Method to trigger near enemies
		/// </summary>
		/// <returns>Number of enemies triggered</returns>
		protected int TriggerNearEnemies(Vector3 target)
		{
			if (!EnemyData.TriggeringNearEnemies) return 0;
			int numberOfEnemiesTriggered = 0;
			GameObject [] FoundEnemyObjects = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (GameObject enemyObject in FoundEnemyObjects)
            {
                if (Vector3.Distance(transform.position, enemyObject.transform.position) > EnemyData.OtherEnemiesTriggerRadius) continue;
				if (enemyObject.transform == transform) continue;

                EnemyController enemyController;

                if (enemyObject.TryGetComponent<EnemyController>(out enemyController))
                {
                    if (enemyController.HandleTriggerByEnemy(target))
                    {
                        numberOfEnemiesTriggered++;
                    }
                }
            }
            return numberOfEnemiesTriggered;
        }

        protected abstract bool HandleTriggerByEnemy(Vector3 target);

        /// <summary>
        /// Method which is destroying collider when enemy died and counting to destroy whole model.
        /// </summary>
        protected void Die()
        {
            if (EnemyAlive)
            {
                if(soundController) soundController.PlayDying();
                
	            Destroy(gameObject.GetComponent<Collider>());
				Destroy(gameObject.GetComponent<Rigidbody>());
				EnemyAlive = false;
				GoToPoint = transform.position;
				MultiUseTimer = 0f;
				MoveTo(transform.position, EnemyData.MovementSpeed, 1);
                OnDeath?.Invoke(this);
            }

			MultiUseTimer += Time.deltaTime;
			if (MultiUseTimer >= EnemyData.DisappearAfter) Destroy(this.gameObject);
			easyAnimator.SetBooleanTrue("isDying");

            
        }

		/// <summary>
		/// Method which have to be called once per every frame update in every enemy controller which want to use the performace boost.
		/// If not using this method you have to calculate distanceToMainChar and playerIsVisible manually instead
		/// </summary>
		protected void HandleLogicPerformaceBoost()
		{
			//Beta version of performance booster
			updateLogicFrame = false;

			if (framesCounter == EnemyData.UpdateLogicEveryXFrames)
			{
				framesCounter = 0;
				updateLogicFrame = true;
			}
			else
			{
				framesCounter++;
            }

            //code below is strongly undebuggable -you have to remember that distance to main char is 
            //updating/counted again only every (see: updateLogicEveryXFrames) frames
            if (updateLogicFrame)
            {
                distanceToMainChar = Vector3.Distance(transform.position, EnemyData.MainCharacterTransform.position);
				playerIsVisible = IsPlayerVisible();
            }
        }
    }
}
