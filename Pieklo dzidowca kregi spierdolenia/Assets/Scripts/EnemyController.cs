///<summary>
///Created by Kumdzio
///</summary>

using UnityEngine;

public abstract class EnemyController : MonoBehaviour, IMove, IFight
{
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



    // Start is called before the first frame update
    protected virtual void Start()
    {
        ////comment below is showing only how to initialize easyAniamtorController
        //string[] ignoredBooleans = new string[] { "ignoredBooleanName1", "ignoredBooleanName2" };
        //easyAnimator = new EasyAnimatorController(GetComponent<Animator>(), ignoredBooleans);
        SpawnPoint = new Vector3(gameObject.transform.position.x,
                                gameObject.transform.position.y,
                                gameObject.transform.position.z);
        CurrentHealth = MaxHealth;
    }

    //OnValidate is called when script is loaded and everytime when value is changed
    //in the inspector
    protected virtual void OnValidate()
    {
        //here will be code to handle debuging and balance changes in values
        //for example there have to be check if the AggroRadius is bigger that AttackRadius etc.
        if (MovementSpeed < 0)
        {
            Debug.Log("Movement speed cannot be lower than 0");
            MovementSpeed = 0.0f;
        }
        if(MovementSpeed >= 5f)
        {
            Debug.Log("Movement speed cannot be bigger that 5");
            MovementSpeed = 5.0f;
        }
        if (MaxHealth <= 0)
        {
            Debug.Log("MaxHealth cannot be less than 1");
            MaxHealth = 1;
        }
        if (AttackRadius < 0.5f)
        {
            Debug.Log("Attack Radius cannot be lower than 0.5");
            AttackRadius = 0.5f;
        }
        if (AttackRadius >= AggroRadius)
        {
            Debug.Log("Attack radius cannot be bigger that Aggro radius");
            AttackRadius = AggroRadius-0.01f;
        }
    }

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        float distanceToMainChar = Vector3.Distance(gameObject.transform.position, _mainCharacterTransform.position);
        if (distanceToMainChar < _aggroRadius)// checking if main char is visible for enemy
        {
            UpdateEnemyRotation(_mainCharacterTransform.position - gameObject.transform.position);
            if (distanceToMainChar < _attackRadius) //checking if main char is in attack range
            {
                easyAnimator.setBooleanTrue("isAttacking");
            }
            else
            {
                //if main char is not in attack range then get closer
                UpdateEnemyPosition(new Vector3(_mainCharacterTransform.position.x - gameObject.transform.position.x,
                                                0,
                                                _mainCharacterTransform.position.z - gameObject.transform.position.z),
                                                1.0f);
                easyAnimator.setBooleanTrue("isWalking");
            }
        }
        else if (Vector3.Distance(this.gameObject.transform.position, SpawnPoint) > 2.0f) //if main char is not visible and this enemy is far form spawn point then go back to spawn
        {
            UpdateEnemyRotation(new Vector3(SpawnPoint.x, 0, SpawnPoint.z) - gameObject.transform.position);
            UpdateEnemyPosition(new Vector3(SpawnPoint.x - gameObject.transform.position.x,
                                            0,
                                            SpawnPoint.z - gameObject.transform.position.z),
                                            1.0f);
            easyAnimator.setBooleanTrue("isWalking");
        }
        else //when mainchar is not visible and reached spawn point
        {
            easyAnimator.ResetAllBooleans();
        }
        if (CurrentHealth<1)
        {
            easyAnimator.setBooleanTrue("isDead");
        }
    }

    protected virtual void UpdateEnemyPosition(Vector3 direction, float speedScale)
    {
        direction = Vector3.Normalize(direction);
        this.gameObject.transform.position += direction * _movementSpeed * speedScale;
    }


    protected virtual void UpdateEnemyRotation(Vector3 direction)
    {
        Vector3 newDirection = Vector3.RotateTowards(gameObject.transform.forward, direction, _rotationSpeed, 0.0f);
        this.gameObject.transform.rotation = Quaternion.LookRotation(newDirection);
    }

    public virtual float GetHealthPercentage()
    {
        return (float)CurrentHealth / (float)_maxHealth;
    }

    //for now damage types are ignored
    public virtual void SetDamage(int damageAmount, DamageType damageType)
    {
        CurrentHealth -= damageAmount;
    }

    public virtual void SetDamage(int damageAmount, DamageType damageType, int criticalMultiplier, float criticalChance)
    {
        if (Random.Range(0.0f, 1.0f) <= criticalChance)
        {
            damageAmount *= criticalMultiplier;
        }
        this.SetDamage(damageAmount, damageType);
    }
}
