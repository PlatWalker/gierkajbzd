///<summary>
/// Created by Szwagier
///Edited by Kumdzio
///</summary>


using System.Diagnostics;
using UnityEngine;

public class GowniakController : MonoBehaviour, IMove, IFight
{
    [SerializeField] private Transform mainCharacterTransform;
    public Transform MainCharacterTransform
    {
        get
        {
            return mainCharacterTransform;
        }
    }

    [SerializeField] private float _movementSpeed = 0.1f;
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

    [SerializeField] private float _aggroRadius = 10.0f;
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

    [SerializeField] private float aggroMaxTimeMs = 5000f;

    [SerializeField] private float _attackRadius = 1.5f;
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

    private Vector3 spawnPoint;

    private Stopwatch _aggroTimer;

    private bool _isInChaseState;

    private bool _isInAttackState;

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


    private bool IsInAggroState { get; set; }

    private bool IsInChaseState
    {
        get => _isInChaseState;
        set
        {
            if (_isInChaseState != value)
            {
                _isInChaseState = value;
                _gowniakAnimator.SetBool("Move", value);
            }
        }
    }

    private bool IsInAttackState
    {
        get => _isInAttackState;
        set
        {
            if (_isInAttackState != value)
            {
                _isInAttackState = value;
                _gowniakAnimator.SetBool("Move", !value);
                _gowniakAnimator.SetBool("Attack", value);
            }
        }
    }

    private bool AggroExpiredCommenceChase() => _aggroTimer != null && _aggroTimer.IsRunning &&
                                                 _aggroTimer.ElapsedMilliseconds >= aggroMaxTimeMs;

    private Animator _gowniakAnimator;

    // Start is called before the first frame update
    private void Start()
    {
        spawnPoint = new Vector3(
                            gameObject.transform.position.x,
                            gameObject.transform.position.y,
                            gameObject.transform.position.z);
        _gowniakAnimator = GetComponent<Animator>();
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
    private void FixedUpdate()
    {
        float distanceToMainChar =
            Vector3.Distance(gameObject.transform.position, mainCharacterTransform.position);

        HandleAggro();

        if (distanceToMainChar < _aggroRadius) // checking if main char is visible for enemy
        {
            _aggroTimer = _aggroTimer != null && _aggroTimer.IsRunning ? _aggroTimer : Stopwatch.StartNew();

            UpdateEnemyRotation(new Vector3(mainCharacterTransform.position.x, 0, mainCharacterTransform.position.z));

            if (distanceToMainChar < _attackRadius) //checking if main char is in attack range
            {
                _aggroTimer = null; // stop aggroTimer, Attack commenced
                IsInAttackState = true;
            }
            else if (distanceToMainChar > _attackRadius && AggroExpiredCommenceChase())
            {
                _aggroTimer = null;
                IsInChaseState = true;
                IsInAttackState = false;
                UpdateEnemyPosition(new Vector3(mainCharacterTransform.position.x - gameObject.transform.position.x,
                                                0,
                                                mainCharacterTransform.position.z - gameObject.transform.position.z));
            }
            else
            {
                IsInAttackState = false;
                if (IsInChaseState)
                    UpdateEnemyPosition(new Vector3(mainCharacterTransform.position.x - gameObject.transform.position.x,
                                                    0,
                                                    mainCharacterTransform.position.z - gameObject.transform.position.z));
            }
        }
        else if (Vector3.Distance(gameObject.transform.position, spawnPoint) > _aggroRadius)
        {
            _aggroTimer = null; // stop aggroTimer, main char outside of aggro radius
            IsInChaseState = false;
            IsInAttackState = false;
            UpdateEnemyRotation(new Vector3(spawnPoint.x, 0, spawnPoint.z));
            UpdateEnemyPosition(new Vector3(spawnPoint.x - gameObject.transform.position.x,
                                            0, 
                                            spawnPoint.z - gameObject.transform.position.z));
        }
    }

    private void UpdateEnemyPosition(Vector3 direction)
    {
        direction = Vector3.Normalize(direction);
        this.gameObject.transform.position += direction * _movementSpeed;
    }

    private void UpdateEnemyRotation(Vector3 direction)
    {
        gameObject.transform.Rotate(0.0f, -90.0f, 0.0f); //reApply Bug of gizmos
        Vector3 targetDirection = direction - gameObject.transform.position;
        Vector3 newDirection = Vector3.RotateTowards(gameObject.transform.forward, targetDirection, _rotationSpeed, 0.0f);
        gameObject.transform.rotation = Quaternion.LookRotation(newDirection);
        gameObject.transform.Rotate(0.0f, 90.0f, 0.0f); // removing Bug of gizmos
    }

    private void HandleAggro()
    {
        const string aggroRadiusTriggerName = "InAggroRadius";

        if (_aggroTimer != null && _aggroTimer.IsRunning)
        {
            if (_aggroTimer.ElapsedMilliseconds <= aggroMaxTimeMs)
            {
                IsInAggroState = true;
                _gowniakAnimator.SetBool(aggroRadiusTriggerName, true);
            }
            else
            {
                _gowniakAnimator.SetBool(aggroRadiusTriggerName, false);
                IsInAggroState = false;
            }
        }

        else
        {
            _gowniakAnimator.SetBool(aggroRadiusTriggerName, false);
            IsInAggroState = false;
        }
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
