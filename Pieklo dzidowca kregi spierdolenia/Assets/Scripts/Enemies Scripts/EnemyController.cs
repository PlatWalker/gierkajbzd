using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// Created by Kumdzio.
/// Abstract class with all needed tolls for simple AI.
/// </summary>
public abstract class EnemyController : MonoBehaviour, IMove, IFight
{
    [Header("Artifical Intelligence")]
    [SerializeField] protected bool turnOffAI = false;
    public bool Alive { get; protected set; }
    [SerializeField] private float _disappearAfter = 5.0f;
    public float DisappearAfter 
    {
        get
        {
            return _disappearAfter;
        }
        protected set
        {
            _disappearAfter = value;
        }
    }
    protected float MultiUseTimer { get; set; }
    [SerializeField] protected int updateLogicEveryXFrames = 3;
    protected int framesCounter;
    public bool SeesPlayer { get; protected set; }

    [Header("Movement")]
    [SerializeField] private float _movementSpeed = 0.15f;
    public virtual float MovementSpeed
    {
        get
        {
            return _movementSpeed;
        }
        protected set
        {
            _movementSpeed = value;
        }
    }

    public bool ShouldGoToPoint { get; protected set; }

    [SerializeField] private float _rotationSpeed = 0.15f;
    public virtual float RotationSpeed
    {
        get
        {
            return _rotationSpeed;
        }
        protected set
        {
            _rotationSpeed = value;
        }
    }
    [SerializeField] private float _timeBetweenPatrolSteps = 2f;
    public float TimeBetweenPatrolSteps
    {
        get
        {
            return _timeBetweenPatrolSteps;
        }
        protected set
        {
            _timeBetweenPatrolSteps = value;
        }
    }
    [SerializeField] private int _maxPatrolSteps = 5;
    public int MaxPatrolSteps
    {
        get
        {
            return _maxPatrolSteps;
        }
        protected set
        {
            _maxPatrolSteps = value;
        }
    }
    public bool IsPatroling { get; protected set; }

    public NavMeshAgent NavAgent { get; private set; }

    public int PatrolStepsCounter { get; protected set; }

    [SerializeField] private float _patrolMaxDistance = 3f;
    public float PatrolMaxDistance 
    {
        get
        {
            return _patrolMaxDistance;
        } 
        private set
        {
            _patrolMaxDistance = value;
        }
    }

    [Header("Attack target transform")]
    [SerializeField] private Transform _mainCharacterTransform = null;
    public virtual Transform MainCharacterTransform
    {
        get
        {
            return _mainCharacterTransform;
        }
    }
    public Vector3 GoToPoint { get; protected set; }

    [Header("Fight")]
    [SerializeField] private float _aggroRadius = 20.0f;
    public virtual float AggroRadius
    {
        get
        {
            return _aggroRadius;
        }
        protected set
        {
            _aggroRadius = value;
        }
    }

    [SerializeField] private float _aggroByAttackRadius = 50.0f;
    public virtual float AggroByAttackRadius
    {
        get
        {
            return _aggroByAttackRadius;
        }
        protected set
        {
            AggroByAttackRadius = value;
        }
    }

    [SerializeField] private float _attackRadius = 1.5f;
    public virtual float AttackRadius
    {
        get
        {
            return _attackRadius;
        }
        protected set
        {
            _attackRadius = value;
        }
    }

    [SerializeField] private int _maxHealth = 100;
    public virtual int MaxHealth
    {
        get
        {
            return _maxHealth;
        }
        protected set
        {
            _maxHealth = value;
        }
    }
    public virtual int CurrentHealth { get; protected set; }


    public virtual Vector3 SpawnPoint { get; protected set; }
    protected  EasyAnimatorController easyAnimator;

    private float animationPlayPreviousSpeed = 0f;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        ////comment below is showing only how to initialize easyAniamtorController
        //string[] ignoredBooleans = new string[] { "ignoredBooleanName1", "ignoredBooleanName2" };
        //easyAnimator = new EasyAnimatorController(GetComponent<Animator>(), ignoredBooleans);
        SpawnPoint = new Vector3(transform.position.x,
                                transform.position.y,
                                transform.position.z);
        CurrentHealth = MaxHealth;
        Alive = true;
        GetComponent<Animator>().SetFloat("IdleSpeedMultiplier", Random.Range(0.900001f, 1.100001f));
        NavAgent = GetComponent<NavMeshAgent>();
        SeesPlayer = false;
    }

    //OnValidate is called when script is loaded and everytime when value is changed
    //in the inspector
    protected virtual void OnValidate()
    {
        //here will be code to handle debuging and balance changes in values
        //for example there have to be check if the AggroRadius is bigger that AttackRadius etc.
        /*if (MovementSpeed < 0)
        {
            Debug.Log(transform.name + ": Movement speed cannot be lower than 0");
        }
        if(MovementSpeed >= 5f)
        {
            Debug.Log(transform.name + ": Movement speed cannot be bigger that 5");
        }
        if (MaxHealth <= 0)
        {
            Debug.Log(transform.name + ": MaxHealth cannot be less than 1");
        }
        if (AttackRadius < 0.5f)
        {
            Debug.Log(transform.name + ": Attack Radius cannot be lower than 0.5");
        }
        if (AttackRadius >= AggroRadius)
        {
            Debug.Log(transform.name+": Attack radius cannot be bigger that Aggro radius");
        }*/
        
        //need to rebuild this
    }

    // Update is called once per frame
    protected abstract void FixedUpdate();
    /// <summary>
    /// Method to set destination point for enemy - shouldn't have been updated every frame.
    /// </summary>
    /// <param name="target">Destination point of path</param>
    /// <param name="speed">Speed of travel. 0 = just rotate</param>
    protected virtual void MoveTo(Vector3 target,float speed, float stopDistance)
    {
        if (Vector3.Distance(target, NavAgent.destination) < 1f) return;
        if (NavAgent.radius <= stopDistance)
        {
            stopDistance = stopDistance - NavAgent.radius;
        }
        else
        {
            stopDistance = 0f;
        }

        if (NavAgent.stoppingDistance != stopDistance)
        {
            NavAgent.stoppingDistance = stopDistance;
        }
        if(speed>0) 
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
            Vector3 newDirection = Vector3.RotateTowards(gameObject.transform.forward, direction, RotationSpeed, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
        }
    }

    /// <summary>
    /// Method created for HealthBars.
    /// </summary>
    /// <returns>Current health in range 0f-1f</returns>
    public virtual float GetHealthPercentage()
    {
        return (float)CurrentHealth / (float)MaxHealth;
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

        SaveAndGoToPoint(MainCharacterTransform.position);
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
            damageAmount =(int)(damageAmount * criticalMultiplier);
        }
        this.SetDamage(damageAmount, damageType);
    }

    /// <summary>
    /// Method to pause enemy AI
    /// </summary>
    public void SwitchAI()
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
        if (Physics.Raycast((transform.position + new Vector3(0f, 1f, 0f)), (MainCharacterTransform.position - transform.position), out hit, AggroRadius, NotEnemiesMask))
        {
            if (hit.transform == MainCharacterTransform)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Method that is called to save current location of player and succesfully go there
    /// Process can be break by changing ShouldGoToPoint to false
    /// </summary>
    protected void SaveAndGoToPoint(Vector3 goToPoint)
    {
        if(Vector3.Distance(transform.position,goToPoint) <= AggroByAttackRadius)
        {
            GoToPoint = goToPoint;
            ShouldGoToPoint = true;
        }
    }

    protected Vector3 ChooseNewPatrollingPoint()
    {
        Vector3 newPoint = Vector3.zero;
        /*bool correctPoint = false;
        RaycastHit hit;

        for (int i = 5; i > 0; i--)
        {
            newPoint = new Vector3(
                Random.Range(transform.position.x - PatrolMaxDistance,
                             transform.position.x + PatrolMaxDistance),

                transform.position.y,

                Random.Range(transform.position.x - PatrolMaxDistance,
                             transform.position.x + PatrolMaxDistance));
            if (Physics.Raycast((transform.position + new Vector3(0f, 1f, 0f)), (newPoint - transform.position), out hit,AggroRadius))
            {
                Debug.Log("New patroling point raycast hit tag: "+hit.collider.transform.tag);
                if(hit.collider.transform.tag == "Terrain")
                {
                    correctPoint = true;
                    break;
                }
            }
            else
            {
                continue;
            }
        }

        if (!correctPoint) newPoint = transform.position;

        Debug.Log("Nowy punkt patrolu: " + newPoint);*/
        newPoint = new Vector3(
                Random.Range(transform.position.x - PatrolMaxDistance,
                             transform.position.x + PatrolMaxDistance),

                transform.position.y,

                Random.Range(transform.position.x - PatrolMaxDistance,
                             transform.position.x + PatrolMaxDistance));
        return newPoint;
    }

    /// <summary>
    /// Method which is destroying collider when enemy died and counting to destroy whole model.
    /// </summary>
    protected void Die()
    {
        Destroy(gameObject.GetComponent<Collider>());
        Destroy(gameObject.GetComponent<Rigidbody>());
        Alive = false;
        MultiUseTimer += Time.deltaTime;
        if (MultiUseTimer >= DisappearAfter) Destroy(this.gameObject);
        easyAnimator.SetBooleanTrue("isDying");
    }
}
