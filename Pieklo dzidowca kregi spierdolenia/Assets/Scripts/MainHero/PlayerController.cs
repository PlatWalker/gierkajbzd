using System;
using System.Collections.Generic;
using jbzdy.CharacterStats;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;
using jbzd.Common.InputSystem;
using jbzd.Common.InputSystem.Inputs;
using jbzd.Common.Interfaces;


namespace jbzd.MainHero
{
    public struct StringAnimatorParameters
    {
        public static string AttackParam => "Attack";
        public static string AttackInProgressParam => "Attacking animation in progress";
        public static string RunParam => "Run";
        public static string RunSpeed => "Running speed";
    }

    public class PlayerController : MonoBehaviour, IDamageable
    {
        #region Serialized fields
        [field:SerializeField]
        public int MaximumHealth { get; private set; }
        [field:SerializeField]
        public int CurrentHealth { get; private set; }
        [field:SerializeField]
        public bool CanPlayerMove { get; set; } //TODO nie ma blokady myszki
        [field:SerializeField]
        public float PlayerSpeed { get; private set; }
        [field:SerializeField]
        public float DashAttackMovePower { get; private set; }
        #endregion

        #region Public variables
        public List<Collider> listOfEnemiesColliders = new List<Collider>();
        public bool NextFrameDash { get; set; }
        public bool IsUiTurnOn { get; set; }
        #endregion

        #region Private variables
        private PlayerInput _inputController;
        private PlayerStats _playerStats;
        private Animator _characterAnimator;
        private Rigidbody _rb;
        private Vector3 _movementVector;
        private Vector3 _newPositionVector;
        #endregion

        #region Player states
        private enum PlayerState
        {
            Idle,
            Attack,
            Move
        }
        private PlayerState _playerState;
        #endregion
        
        [Inject]
        public void Construct(InputManager inputManager)
        {
            _inputController = inputManager.GetInput<PlayerInput>();
        }

        private void Start()
        {
            _characterAnimator = GetComponentInChildren<Animator>();
            if (_characterAnimator == null) Debug.Log("Nie znaleziono animatora w postaci gracza!");

            var _behaviours = _characterAnimator.GetBehaviours<MainHeroBehaviour>();
            var _firstBehaviour = _behaviours.First();

            _firstBehaviour.stateControl += OnStateEnterJbzd;
            _firstBehaviour.stateControl += OnStateExitJbzd;

            AnimatorParametersCheck();
            
            _rb = GetComponent<Rigidbody>();
            _playerStats = GetComponent<PlayerStats>();
        }

        private void FixedUpdate()
        {
            switch (_playerState)
            {
                case PlayerState.Idle:
                    //in update
                    break;
                case PlayerState.Move:

                    if (NextFrameDash)
                    {
                        DashAttackMove();
                        NextFrameDash = false;
                        return;
                    }  

                    if (CanPlayerMove)
                    {
                        UpdateCharacterPosition();
                        UpdateCharacterRotation(_movementVector);
                        UpdateCharacterAnimation();
                    }

                    if(_inputController.attackInputStatus.Basic) _playerState = PlayerState.Attack;

                    if(!_inputController.movementInputStatus.Down && !_inputController.movementInputStatus.Up && !_inputController.movementInputStatus.Left && !_inputController.movementInputStatus.Right)
                    {
                        _playerState = PlayerState.Idle;
                    }

                    break;
                case PlayerState.Attack:
                    //in update
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Update()
        {
            switch (_playerState)
            {
                case PlayerState.Idle:

                    if(_inputController.attackInputStatus.Basic) _playerState = PlayerState.Attack;

                    if(_inputController.movementInputStatus.Down || _inputController.movementInputStatus.Up || _inputController.movementInputStatus.Left || _inputController.movementInputStatus.Right)
                    {
                        _playerState = PlayerState.Move;
                    } 
                    
                    break;
                case PlayerState.Attack:

                    Vector3 flatVector = _inputController.mousePositionFlat;
                    flatVector.y = transform.position.y;
                    transform.LookAt(flatVector);
                
                    CanPlayerMove = false;
                    _characterAnimator.SetBool(StringAnimatorParameters.AttackParam, true);
                    
                    if(_inputController.movementInputStatus.Down || _inputController.movementInputStatus.Up || _inputController.movementInputStatus.Left || _inputController.movementInputStatus.Right)
                    {
                        CanPlayerMove = true;
                        _playerState = PlayerState.Move;
                    }

                    if(!_inputController.movementInputStatus.Down && !_inputController.movementInputStatus.Up && !_inputController.movementInputStatus.Left && !_inputController.movementInputStatus.Right)
                    {
                        CanPlayerMove = true;
                        _playerState = PlayerState.Idle;
                    }

                    break;
                case PlayerState.Move:
                    // in FixedUpdate

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            //Synchronising running animation with character speed
            _characterAnimator.SetFloat(StringAnimatorParameters.RunSpeed, _rb.velocity.magnitude);
        }

        private void AnimatorParametersCheck()
        {
            if(_characterAnimator.parameters.Any(x => x.name == StringAnimatorParameters.AttackParam) == false) 
                Debug.Log("Blad w nazwie parametru atakowania");
            if(_characterAnimator.parameters.Any(x => x.name == StringAnimatorParameters.AttackInProgressParam) == false)
                Debug.Log("Blad w nazwie parametru progresu animacji atakowania");
            if(_characterAnimator.parameters.Any(x => x.name == StringAnimatorParameters.RunParam) == false)
                Debug.Log("Blad w nazwie parametru biegania");
            if (_characterAnimator.parameters.Any(x => x.name == StringAnimatorParameters.RunSpeed) == false)
                Debug.Log("Blad w nazwie parametru szybkosci biegania");
        }

        public void SetDamage(int damageAmount, DamageType damageType)
        {
            _playerStats.Health.BaseValue -= damageAmount;
        }

        public void SetDamage(int damageAmount, DamageType damageType, float criticalMultiplier, float criticalChance)
        {
            if (UnityEngine.Random.Range(0.0f, 1.0f) <= criticalChance)
            {
                damageAmount = (int)(damageAmount * criticalMultiplier);
            }

            SetDamage(damageAmount, damageType);
        }

        public void PlaceAt(Vector3 placement) => transform.position = placement;

        private void UpdateCharacterPosition()
        {
            _movementVector = Vector3.zero;
            _movementVector += Vector3.forward * Convert.ToInt32(_inputController.movementInputStatus.Up);
            _movementVector += Vector3.back * Convert.ToInt32(_inputController.movementInputStatus.Down);
            _movementVector += Vector3.left * Convert.ToInt32(_inputController.movementInputStatus.Left);
            _movementVector += Vector3.right * Convert.ToInt32(_inputController.movementInputStatus.Right);
            
            StickPlayerToGround();

            _rb.AddForce(_movementVector.normalized * PlayerSpeed, ForceMode.VelocityChange);
        }

        private void UpdateCharacterRotation(Vector3 lookDirection)
        {
            if (lookDirection.magnitude == 0) return;
            
            var rotation = Quaternion.LookRotation(lookDirection);
            
            transform.rotation = rotation;
        }

        private void UpdateCharacterAnimation()
        {
            if (_movementVector.z != 0 || _movementVector.x != 0)
            {
                _characterAnimator.SetBool(StringAnimatorParameters.RunParam, true);
            }
            else
            {
                _characterAnimator.SetBool(StringAnimatorParameters.RunParam, false);
            }
        }
        
        private void StickPlayerToGround()
        {
            if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out RaycastHit hit) 
                && hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    _newPositionVector.x = _rb.position.x;
                    _newPositionVector.y = hit.point.y;
                    _newPositionVector.z = _rb.position.z;

                    _rb.MovePosition(_newPositionVector);
                }
        }

        private void DashAttackMove()
        {
            Vector3 dashVector = _inputController.mousePositionFlat - transform.position;
            
            UpdateCharacterRotation(dashVector.normalized);

            _rb.AddForce(dashVector.normalized * DashAttackMovePower , ForceMode.Impulse);
            
            
            
            StickPlayerToGround();
        }

        bool isAttackAnimationPlayingJbzd(AnimatorStateInfo stateInfo) =>
            stateInfo.IsName("Atk1") || stateInfo.IsName("Atk2") || stateInfo.IsName("Atk3") ||
            stateInfo.IsName("Atk4");

        bool isTransitionStateJbzd(AnimatorStateInfo stateInfo) => stateInfo.IsName("Transition state");

        private void OnStateEnterJbzd(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (isTransitionStateJbzd(stateInfo))
            {
                CanPlayerMove = true;
            }
            
            if (isAttackAnimationPlayingJbzd(stateInfo))
            {
                NextFrameDash = true;
                animator.SetBool(StringAnimatorParameters.AttackInProgressParam, true);
            }

            animator.SetBool(StringAnimatorParameters.AttackParam, false);
        }

        private void OnStateExitJbzd(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!isTransitionStateJbzd(stateInfo))
                animator.SetBool(StringAnimatorParameters.AttackParam, false);

            if (isAttackAnimationPlayingJbzd(stateInfo))
                animator.SetBool(StringAnimatorParameters.AttackInProgressParam, false);            
        }
    } 
}