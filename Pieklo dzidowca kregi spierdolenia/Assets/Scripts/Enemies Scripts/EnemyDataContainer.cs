using UnityEngine;
using UnityEngine.AI;

public class EnemyDataContainer : ScriptableObject
{
    [Header("Artifical Intelligence")]
    [SerializeField] private float _disappearAfter = 5.0f;
    public float DisappearAfter
    {
        get
        {
            return _disappearAfter;
        }
    }
    [SerializeField] private int _updateLogicEveryXFrames = 3;
    public int UpdateLogicEveryXFrames
    {
        get
        {
            return _updateLogicEveryXFrames;
        }
    }
    [SerializeField] float _otherEnemiesTriggerRadius = 20f;
    public float OtherEnemiesTriggerRadius
    {
        get
        {
            return _otherEnemiesTriggerRadius;
        }
    }
    [SerializeField] private bool _triggeringNearEnemies = true;
    public bool TriggeringNearEnemies
    {
        get
        {
            return _triggeringNearEnemies;
        }
    }

    [Header("Movement")]
    [SerializeField] private float _movementSpeed = 0.15f;
    public virtual float MovementSpeed
    {
        get
        {
            return _movementSpeed;
        }
        set
        {
            _movementSpeed = value;
        }
    }
    public Vector3 GoToPoint { get; protected set; }
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
    public NavMeshAgent NavAgent { get; protected set; }
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
    protected EasyAnimatorController easyAnimator;
    private float animationPlayPreviousSpeed = 0f;
}
