using UnityEngine;
using UnityEngine.AI;
[CreateAssetMenu(fileName = "NewEnemyData", menuName = "EnemyDataContainer", order = 100)]
/// <summary>
/// Data container for simple data of every enemy.
/// Created by Kumdzio
/// </summary>
public class EnemyDataContainer : ScriptableObject
{
    [Header("Artifical Intelligence")]
    [SerializeField] private float _disappearAfter=0;
    public float DisappearAfter
    {
        get
        {
            return _disappearAfter;
        }
    }
    [SerializeField] private int _updateLogicEveryXFrames=0;
    public int UpdateLogicEveryXFrames
    {
        get
        {
            return _updateLogicEveryXFrames;
        }
    }
    [SerializeField] private float _otherEnemiesTriggerRadius=0;
    public float OtherEnemiesTriggerRadius
    {
        get
        {
            return _otherEnemiesTriggerRadius;
        }
    }
    [SerializeField] private bool _triggeringNearEnemies=false;
    public bool TriggeringNearEnemies
    {
        get
        {
            return _triggeringNearEnemies;
        }
    }

    [Header("Movement")]
    [SerializeField] private float _movementSpeed=0;
    public float MovementSpeed
    {
        get
        {
            return _movementSpeed;
        }
    }
    [SerializeField] private float _rotationSpeed=0;
    public float RotationSpeed
    {
        get
        {
            return _rotationSpeed;
        }
    }
    [SerializeField] private float _timeBetweenPatrolSteps=0;
    public float TimeBetweenPatrolSteps
    {
        get
        {
            return _timeBetweenPatrolSteps;
        }
    }
    [SerializeField] private int _maxPatrolSteps=0;
    public int MaxPatrolSteps
    {
        get
        {
            return _maxPatrolSteps;
        }
    }
    [SerializeField] private float _patrolMaxDistance=0;
    public float PatrolMaxDistance
    {
        get
        {
            return _patrolMaxDistance;
        }
    }

    [Header("Attack target transform")]
    [SerializeField] private Transform _mainCharacterTransform=null;
    public virtual Transform MainCharacterTransform
    {
        get
        {
            //here insert instance taken from game manager - no need to store reference
            if (_mainCharacterTransform)
            {
                return _mainCharacterTransform;
            }
            else
            {
                _mainCharacterTransform = GameObject.Find("Malpa (1)").transform;
                if (_mainCharacterTransform)
                {
                    return _mainCharacterTransform;
                }
                else
                {
                    Debug.Log("Cannot find istance of \"MainChar\" and enemy do not know where to go");
                    return null;
                }
            }
        }
    }

    [Header("Fight")]
    [SerializeField] private float _aggroRadius=0;
    public float AggroRadius
    {
        get
        {
            return _aggroRadius;
        }
    }
    [SerializeField] private float _aggroByAttackRadius=0;
    public float AggroByAttackRadius
    {
        get
        {
            return _aggroByAttackRadius;
        }
    }
    [SerializeField] private float _attackRadius=0;
    public float AttackRadius
    {
        get
        {
            return _attackRadius;
        }
    }
    [SerializeField] private int _maxHealth=0;
    public int MaxHealth
    {
        get
        {
            return _maxHealth;
        }
    }
    [SerializeField] private int _damage = 0;
    public int Damage
    {
        get
        {
            return _damage;
        }
    }

    private void OnEnable()
    {
        _mainCharacterTransform = GameObject.Find("Malpa (1)").transform;
    }
}
