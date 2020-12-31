///<summary>
///Created By Kumdzio
///</summary>

using UnityEngine;

public class MonkeyController : MonoBehaviour, IMove, IFight
{
    [SerializeField] private Transform _mainCharacterTransform;
    public Transform MainCharacterTransform
    {
        get
        {
            return _mainCharacterTransform;
        }
    }

    [SerializeField] private float _movementSpeed = 0.14f;
    public float MovementSpeed
    {
        get
        {
            return _movementSpeed;
        }
        private set
        {
            _movementSpeed = value;
        }
    }

    [SerializeField] private float _aggroRadius = 20.0f;
    public float AggroRadius
    {
        get
        {
            return _aggroRadius;
        }
        private set
        {
            _aggroRadius = value;
        }
    }

    [SerializeField] private float _attackRadius = 8.5f;
    public float AttackRadius
    {
        get
        {
            return _attackRadius;
        }
        set
        {
            _attackRadius = value;
        }
    }

    [SerializeField] private float runAwayRadius = 5.0f;

    [SerializeField] private float runSpeedDebuff = 0.7f;

    [SerializeField] private float _rotationSpeed = 0.15f;
    public float RotationSpeed
    {
        get
        {
            return _rotationSpeed;
        }
        private set
        {
            _rotationSpeed = value;
        }
    }

    [SerializeField] private GameObject projectileObject;

    [SerializeField] private Transform projectileSpawnPoint;

    [SerializeField] private float throwPower=1.0f;

    [SerializeField] private int _maxHealth = 100;
    public int MaxHealth
    {
        get
        {
            return _maxHealth;
        }
        private set
        {
            _maxHealth = value;
        }
    }
    public int CurrentHealth { get; private set; }


    private Vector3 spawnPoint;
    private Animator monkeyAnimator;
    EasyAnimatorController easyMonkeyAnimator;
   


    // Start is called before the first frame update
    void Start()
    {
        monkeyAnimator = GetComponent<Animator>();
        string[] ignoredBooleans = new string[] { "spawnProjectile" };
        easyMonkeyAnimator = new EasyAnimatorController(GetComponent<Animator>(),ignoredBooleans);
        spawnPoint = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
        CurrentHealth = _maxHealth;
    }

    //OnValidate is called when script is loaded and everytime when value is changed
    //in the inspector
    void OnValidate()
    {
        //here will be code to handle debuging and balance changes in values
        //for example there have to be check if the AggroRadius is bigger that AttackRadius etc.
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool isWalking = false, isAttacking = false, isDead = false;
        float distanceToMainChar = Vector3.Distance(this.gameObject.transform.position, _mainCharacterTransform.position);
        if (distanceToMainChar < _aggroRadius)// checking if main char is visible for enemy
        {
            if (distanceToMainChar < _attackRadius) //checking if main char is in attack range
            {
                if (distanceToMainChar < runAwayRadius)
                {
                    //move enemy in opposite direction than main char and with speed debuff
                    UpdateEnemyPosition(new Vector3(this.gameObject.transform.position.x - _mainCharacterTransform.position.x, 0, this.gameObject.transform.position.z - _mainCharacterTransform.position.z),runSpeedDebuff);
                    UpdateEnemyRotation(this.transform.position-_mainCharacterTransform.position);
                    isWalking = true;
                }
                else
                {
                    UpdateEnemyRotation(_mainCharacterTransform.position - this.transform.position );
                    isAttacking = true;
                    if (monkeyAnimator.GetBool("spawnProjectile"))
                    {
                        //tutaj tworze pocisk
                        GameObject projectile = Instantiate(projectileObject, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
                        Rigidbody rigidbody = projectile.GetComponent<Rigidbody>();
                        Vector3 throwDirection = (_mainCharacterTransform.position - projectile.transform.position);
                        throwDirection.y += 2;
                        throwDirection *= throwPower;
                        rigidbody.AddForce(throwDirection,ForceMode.Impulse);
                        monkeyAnimator.SetBool("spawnProjectile", false);
                        
                        //tutaj dodać rzut bananem - żeby leciał i się kręcił.
                    }
                    //attacking code goes here
                }
            }
            else
            {
                //if main char is not in attack range then get closer
                UpdateEnemyRotation(_mainCharacterTransform.position - this.transform.position);
                UpdateEnemyPosition(new Vector3(_mainCharacterTransform.position.x - this.gameObject.transform.position.x, 0, _mainCharacterTransform.position.z - this.gameObject.transform.position.z));
                isWalking = true;
            }
        }
        else if (Vector3.Distance(this.gameObject.transform.position, spawnPoint) > 2.0f) //if main char is not visible and this enemy is far form spawn point then go back to spawn
        {
            UpdateEnemyRotation(new Vector3(spawnPoint.x, 0, spawnPoint.z)-this.transform.position);
            UpdateEnemyPosition(new Vector3(spawnPoint.x - this.gameObject.transform.position.x, 0, spawnPoint.z - this.gameObject.transform.position.z));
            isWalking = true;
        }

        if (isAttacking)
        {
            //monkeyAnimator.SetBool("isAttacking", true);
            //monkeyAnimator.SetBool("isWalking", false);
            easyMonkeyAnimator.setBooleanTrue("isAttacking");
        }
        else if(isWalking)
        {
            //monkeyAnimator.SetBool("isAttacking", false);
            //monkeyAnimator.SetBool("isWalking", true);
            easyMonkeyAnimator.setBooleanTrue("isWalking");
        }
        else
        {
            easyMonkeyAnimator.ResetAllBooleans();
            //monkeyAnimator.SetBool("isAttacking", false);
            //monkeyAnimator.SetBool("isWalking", false);
        }

        if (isDead)
        {
            //monkeyAnimator.SetBool("isDead", isDead);
            easyMonkeyAnimator.setBooleanTrue("isDead");
        }
    }

    void UpdateEnemyPosition(Vector3 direction,float speedDebuff)
    {
        direction = Vector3.Normalize(direction);
        this.gameObject.transform.position += direction * _movementSpeed * speedDebuff;
    }
    void UpdateEnemyPosition(Vector3 direction)
    {
        direction = Vector3.Normalize(direction);
        this.gameObject.transform.position += direction * _movementSpeed;
    }

    void UpdateEnemyRotation(Vector3 direction)
    {
        Vector3 newDirection = Vector3.RotateTowards(this.gameObject.transform.forward, direction, _rotationSpeed, 0.0f);
        this.gameObject.transform.rotation = Quaternion.LookRotation(newDirection);
    }

    public float GetHealthPercentage()
    {
        return (float)CurrentHealth / (float)_maxHealth;
    }

    //for now damage types are ignored
    public void SetDamage(int damageAmount, DamageType damageType)
    {
        CurrentHealth -= damageAmount;
    }

    public void SetDamage(int damageAmount, DamageType damageType, int criticalMultiplier, float criticalChance)
    {
        if (Random.Range(0.0f, 1.0f) <= criticalChance)
        {
            damageAmount *= criticalMultiplier;
        }
        this.SetDamage(damageAmount, damageType);
    }
}