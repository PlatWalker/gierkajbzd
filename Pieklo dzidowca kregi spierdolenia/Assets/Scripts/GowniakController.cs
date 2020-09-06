using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class GowniakController : MonoBehaviour
{
    [SerializeField] public Transform mainCharacterTransform;

    [SerializeField] private float movementSpeed = 0.1f;

    [SerializeField] private float aggroRadius = 10.0f;

    [SerializeField] private float aggroMaxTimeMs = 5000f;

    [SerializeField] private float attackRadius = 1.5f;

    private Vector3 spawnPoint;

    private Stopwatch _aggroTimer;

    private bool _isInAggroState;

    private bool _isInChaseState;

    private bool _isInAttackState;

    private bool IsInAggroState
    {
        get => _isInAggroState;
        set
        {
            if (_isInAggroState != value)
            {
                Debug.Log($"Aggro {(value ? "Triggered" : "Stopped")}");
                _isInAggroState = value;
            }
        }
    }
    
    private bool IsInChaseState
    {
        get => _isInChaseState;
        set
        {
            if (_isInChaseState != value)
            {
                Debug.Log($"Chase {(value ? "Triggered" : "Stopped")}");
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
                Debug.Log($"Attack {(value ? "Triggered" : "Stopped")}");
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
        spawnPoint = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y,
            this.gameObject.transform.position.z);
        _gowniakAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        float distanceToMainChar =
            Vector3.Distance(this.gameObject.transform.position, mainCharacterTransform.position);

        HandleAggro();

        if (distanceToMainChar < aggroRadius) // checking if main char is visible for enemy
        {
            _aggroTimer = _aggroTimer != null && _aggroTimer.IsRunning ? _aggroTimer : Stopwatch.StartNew();

            UpdateEnemyRotation(new Vector3(mainCharacterTransform.position.x, 0, mainCharacterTransform.position.z));
            
                if (distanceToMainChar < attackRadius) //checking if main char is in attack range
                {
                    _aggroTimer = null; // stop aggroTimer, Attack commenced
                    IsInAttackState = true;
                }
                else if (distanceToMainChar > attackRadius && AggroExpiredCommenceChase())
                {
                    _aggroTimer = null;
                    IsInChaseState = true;
                    IsInAttackState = false;
                    UpdateEnemyPosition(new Vector3(mainCharacterTransform.position.x - this.gameObject.transform.position.x, 0, mainCharacterTransform.position.z - this.gameObject.transform.position.z));
                }
                else
                {
                    IsInAttackState = false;
                    if (IsInChaseState)
                        UpdateEnemyPosition(new Vector3(mainCharacterTransform.position.x - this.gameObject.transform.position.x, 0, mainCharacterTransform.position.z - this.gameObject.transform.position.z));
                }
        }
        else if (Vector3.Distance(this.gameObject.transform.position, spawnPoint) > aggroRadius)
        {
            _aggroTimer = null; // stop aggroTimer, main char outside of aggro radius
            IsInChaseState = false;
            IsInAttackState = false;
             UpdateEnemyRotation(new Vector3(spawnPoint.x,0,spawnPoint.z));
             UpdateEnemyPosition(new Vector3(spawnPoint.x-this.gameObject.transform.position.x,0,spawnPoint.z-this.gameObject.transform.position.z));
        }
    }

    private void UpdateEnemyPosition(Vector3 direction)
    {
        direction = Vector3.Normalize(direction);
        this.gameObject.transform.position += direction * movementSpeed;
    }

    private void UpdateEnemyRotation(Vector3 direction)
    {
        this.gameObject.transform.LookAt(direction);
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
}