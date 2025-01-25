using System;
using UnityEngine;

namespace jbzd.Enemies.Obsolete
{
    [CreateAssetMenu(fileName = "NewEnemyData", menuName = "EnemyDataContainer", order = 100)]
    /// <summary>
    /// Data container for simple data of every enemy.
    /// Created by Kumdzio
    /// </summary>

    [Obsolete]
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
                
                    GameObject temp = GameObject.FindGameObjectWithTag("Player");
                    if (temp)
                    {
                        _mainCharacterTransform = temp.transform;
                        return _mainCharacterTransform;
                    }
                    else
                    {
                        Debug.Log("Cannot find object with tag \"Player\" and enemy do not know where to go");
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

        [Header("Sounds")]
        [SerializeField] protected AudioClip _damagedAudioClip;
        public AudioClip DamagedAudioClip
        {
            get
            {
                return _damagedAudioClip;
            }
        }
        [SerializeField] protected AudioClip _dyingAudioClip;
        public AudioClip DyingAudioClip
        {
            get
            {
                return _dyingAudioClip;
            }
        }
        [SerializeField] protected AudioClip _ambientAudioClip;
        public AudioClip AmbientAudioClip
        {
            get
            {
                return _ambientAudioClip;
            }
        }
        [SerializeField] protected AudioClip _aggroAudioClip;
        public AudioClip AggroAudioClip
        {
            get
            {
                return _aggroAudioClip;
            }
        }
        [SerializeField] protected AudioClip _attackAudioClip;
        public AudioClip AttackAudioClip
        {
            get
            {
                return _attackAudioClip;
            }
        }

        private void OnEnable()
        {
            //To be removed when can get instance from game manager
            GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
            if (gameObject)
            {
                _mainCharacterTransform = gameObject.transform;
            }
        }
    }
}
