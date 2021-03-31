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
    [SerializeField] private float _otherEnemiesTriggerRadius = 20f;
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
    public float MovementSpeed
    {
        get
        {
            return _movementSpeed;
        }
    }
    [SerializeField] private float _rotationSpeed = 0.15f;
    public float RotationSpeed
    {
        get
        {
            return _rotationSpeed;
        }
    }
    [SerializeField] private float _timeBetweenPatrolSteps = 2f;
    public float TimeBetweenPatrolSteps
    {
        get
        {
            return _timeBetweenPatrolSteps;
        }
    }
    [SerializeField] private int _maxPatrolSteps = 5;
    public int MaxPatrolSteps
    {
        get
        {
            return _maxPatrolSteps;
        }
    }
    [SerializeField] private float _patrolMaxDistance = 3f;
    public float PatrolMaxDistance
    {
        get
        {
            return _patrolMaxDistance;
        }
    }

    [Header("Attack target transform")]
    [SerializeField] private Transform _mainCharacterTransform = null;
    public virtual Transform MainCharacterTransform
    {
        get
        {
            //here insert instance taken from game manager - no need to store reference
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
    }
    [SerializeField] private float _aggroByAttackRadius = 50.0f;
    public virtual float AggroByAttackRadius
    {
        get
        {
            return _aggroByAttackRadius;
        }
    }
    [SerializeField] private float _attackRadius = 1.5f;
    public virtual float AttackRadius
    {
        get
        {
            return _attackRadius;
        }
    }
    [SerializeField] private int _maxHealth = 100;
    public virtual int MaxHealth
    {
        get
        {
            return _maxHealth;
        }
    }
}
