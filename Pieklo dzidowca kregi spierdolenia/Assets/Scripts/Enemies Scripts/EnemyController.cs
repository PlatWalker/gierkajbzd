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
    protected float DisappearTimer { get; set; }
    public bool TriggeredByAttack { get; protected set; }

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

    [Header("Attact target transform")]
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
        //DisappearTimer = 0f;
        GetComponent<Animator>().SetFloat("IdleSpeedMultiplier", Random.Range(0.900001f, 1.100001f));
        //TriggeredByAttack = false;
    }

    //OnValidate is called when script is loaded and everytime when value is changed
    //in the inspector
    protected virtual void OnValidate()
    {
        //here will be code to handle debuging and balance changes in values
        //for example there have to be check if the AggroRadius is bigger that AttackRadius etc.
        if (MovementSpeed < 0)
        {
            Debug.Log(transform.name + ": Movement speed cannot be lower than 0");
            MovementSpeed = 0.0f;
        }
        if(MovementSpeed >= 5f)
        {
            Debug.Log(transform.name + ": Movement speed cannot be bigger that 5");
            MovementSpeed = 5.0f;
        }
        if (MaxHealth <= 0)
        {
            Debug.Log(transform.name + ": MaxHealth cannot be less than 1");
            MaxHealth = 1;
        }
        if (AttackRadius < 0.5f)
        {
            Debug.Log(transform.name + ": Attack Radius cannot be lower than 0.5");
            AttackRadius = 0.5f;
        }
        if (AttackRadius >= AggroRadius)
        {
            Debug.Log(transform.name+": Attack radius cannot be bigger that Aggro radius");
            AttackRadius = AggroRadius-0.01f;
        }
    }

    // Update is called once per frame
    protected abstract void FixedUpdate();
    /// <summary>
    /// Method to move enemy. Can be used towards target, run away from target or just simply rotate 
    /// enemy when speedModifier=0f
    /// </summary>
    /// <param name="shouldRunAway"> Decides if enemy should run away from target.</param>
    /// <param name="speedModifier"> Modifies speed of enemy. 1.0f is normal speed. 0f is just rotating.</param>
    /// <param name="target"> Target position in game world.</param>
    protected virtual void MoveTo(bool shouldRunAway, float speedModifier, Vector3 target)
    {
        /*
        Vector3 direction = new Vector3(target.x - transform.position.x,
                                0,
                                target.z - transform.position.z);

        if (shouldRunAway)
        {
            direction = new Vector3(transform.position.x - target.x,
                                    0,
                                    transform.position.z - target.z);
        }

        direction = Vector3.Normalize(direction);
        transform.position += direction * MovementSpeed * speedModifier;

        Vector3 newDirection = Vector3.RotateTowards(gameObject.transform.forward, direction, RotationSpeed, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
        */
        if(speedModifier>0) 
        {
            GetComponent<NavMeshAgent>().destination = target;
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

        TriggerByAttack();
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
    /// Method that is checking if player is visible for enemy who is calling this method.
    /// </summary>
    /// <returns>True if player is visible otherwise false</returns>
    protected bool PlayerVisible()
    {
        //Debug.DrawRay(transform.position, (MainCharacterTransform.position - transform.position), Color.cyan, 0.5f);
        RaycastHit hit;
        LayerMask NotEnemiesMask = ~LayerMask.GetMask("Enemies");
        if (Physics.Raycast((transform.position + new Vector3(0f, 1f, 0f)), ((MainCharacterTransform.position + new Vector3(0f,0f,0f)) - transform.position), out hit, AggroRadius, NotEnemiesMask))
        {
            Debug.DrawRay((transform.position + new Vector3(0f, 1f, 0f)), ((hit.transform.position + new Vector3(0f, 0f, 0f)) - transform.position), Color.cyan, 0.0f);
            if (hit.transform == MainCharacterTransform)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Method that is called to handle aggro when attacked from distance greater than aggro radius
    /// </summary>
    protected void TriggerByAttack()
    {
        if(Vector3.Distance(transform.position,MainCharacterTransform.position) <= AggroByAttackRadius)
        {
            TriggeredByAttack = true;
            GoToPoint = MainCharacterTransform.position;
        }
    }

    /// <summary>
    /// Method which is dedstroying collider when enemy died and counting to destroy whole model.
    /// </summary>
    protected void HandleDying()
    {
        if (CurrentHealth <= 0)
        {
            Destroy(gameObject.GetComponent<Collider>());
            Destroy(gameObject.GetComponent<Rigidbody>());
            Alive = false;
            DisappearTimer += Time.deltaTime;
            if (DisappearTimer >= DisappearAfter) Destroy(this.gameObject);
            easyAnimator.SetBooleanTrue("isDying");
        }
    }
}
