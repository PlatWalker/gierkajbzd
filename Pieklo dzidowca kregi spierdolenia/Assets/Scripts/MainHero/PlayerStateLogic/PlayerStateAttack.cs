using System.Collections;
using System.Drawing.Drawing2D;
using System.Linq;
using jbzd.MinorSystems.InputSystem.Inputs;
using UnityEngine;

namespace jbzd.MainHero.PlayerStateLogic
{
    public class PlayerStateAttack : IPlayerStateLogic
    {
        public delegate void PlayerStateChanged(PlayerState playerState);
        public event PlayerStateChanged OnPlayerStateChanged;
        
        private readonly PlayerInput _playerInput;
        private readonly PlayerManager _playerManager;

        private bool _isAttacking;
        private bool _isCapturingClickAttackForCombo;

        private int _backfieldVariable = 1;
        private int NextPartOfComboAttack
        {
            get => _backfieldVariable;
            set
            {
                if (value is 0 or < 0)
                {
                    Debug.LogError($"Value of {nameof(_backfieldVariable)} can't be less or equal to 0");
                }

                _backfieldVariable = value;
            }
        }

        private ClickAttackCaptureInfo _clickAttackCaptureInfo;
        
        private struct ClickAttackCaptureInfo
        {
            public Vector3 DashVector;
            public Vector3 LookDirection;
        }
        
        public PlayerStateAttack(
            PlayerManager playerManager,
            PlayerInput playerInput,
            MainHeroBehaviour behaviour)
        {
            _playerManager = playerManager;
            _playerInput = playerInput;

            behaviour.OnStateEnterPassed += OnAttackStateEnter;
            behaviour.OnStateExitPassed += OnAttackStateExit;
        }

        public PlayerState InitPlayerState => PlayerState.Attack;

        #region Unity update events

        public PlayerState StateLogicForFixedUpdate()
        {
            return PlayerState.Attack;
        }

        public PlayerState StateLogicForUpdate()
        {
            if (_isAttacking) return PlayerState.Attack;

            if (_playerManager.CanPlayerMove is false) return PlayerState.Idle;

            if (_isCapturingClickAttackForCombo) return PlayerState.Attack;
            
            _clickAttackCaptureInfo = new ClickAttackCaptureInfo
            {
                DashVector = DashAttackMoveVector(),
                LookDirection = _playerInput.mousePositionFlat - _playerManager.transform.position
            };

            SetAnimTrigger();

            return PlayerState.Attack;
        }

        public PlayerState StateLogicForLateUpdate()
        {
            return PlayerState.Attack;
        }
        
        #endregion

        #region Animation events
        
        private void OnAttackStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if(!PlayerStateNameHelper.IsAttacking(stateInfo)) return;
            
            _isAttacking = true;
            _playerManager.AttackInputFreeze = true;
            
            Dash();
            
            _playerManager.StartCoroutine(WaitForStartOfCapturingAttackInput());
            
            return;
            
            void Dash()
            {
                UpdateCharacterRotation(_clickAttackCaptureInfo.LookDirection);
                
                const float percentageOfClipLength = 0.05f;
                var dashEnd = stateInfo.length - stateInfo.length * percentageOfClipLength;
                _playerManager.StartCoroutine(MoveOverSeconds(
                    _playerManager.Rb,
                    _clickAttackCaptureInfo.DashVector,
                    dashEnd));
                
                return;

                void UpdateCharacterRotation(Vector3 lookDirection)
                {
                    if (lookDirection.magnitude == 0) return;
                
                    var rotation = Quaternion.LookRotation(lookDirection);
            
                    _playerManager.transform.rotation = rotation;
                }
                
                IEnumerator MoveOverSeconds(Rigidbody rigidbodyToMove, Vector3 end, float seconds)
                {
                    float elapsedTime = 0;
                    var startingPos = rigidbodyToMove.position;
                    var col = _playerManager.CharacterCollider;
                    // we want a little smaller radius, it will let us dash even next to ex. wall
                    var colRadius = (float)(col.radius*0.9);
                    
                    while (elapsedTime < seconds)
                    {
                        var resultColliders = new Collider[10];
                        
                        //https://roundwide.com/physics-overlap-capsule/ quick maths
                        var direction = new Vector3 {[col.direction] = 1};
                        var offset = col.height / 2 - colRadius;
                        var localPoint0 = col.center - direction * offset;
                        var localPoint1 = col.center + direction * offset;
                        var point0 = _playerManager.transform.TransformPoint(localPoint0);
                        var point1 = _playerManager.transform.TransformPoint(localPoint1);
                        var r = _playerManager. transform.TransformVector(colRadius, colRadius, colRadius);
                        var radius = Enumerable.Range(0, 3).Select(xyz => xyz == col.direction ? 0 : r[xyz])
                            .Select(Mathf.Abs).Max();
                        
                        // layers that are NOT blocking dash
                        var ignoreLayers =~ (LayerMask.GetMask("Player") |
                                             LayerMask.GetMask("Enemies") |
                                             LayerMask.GetMask("NPC") |
                                             LayerMask.GetMask("Interaction"));

                        Physics.OverlapCapsuleNonAlloc(point0, point1, radius, resultColliders, ignoreLayers);

                        if (!resultColliders.ToList().All(collider => collider is null)) yield break;

                        rigidbodyToMove.position = Vector3.Lerp(startingPos, end, elapsedTime / seconds);
                        elapsedTime += Time.deltaTime;
                        yield return null;
                    }
                    
                    rigidbodyToMove.position = end;
                }
            }
            
            IEnumerator WaitForStartOfCapturingAttackInput()
            {
                var percent = _playerManager.ComboClickPercentage/100f;
                
                while (true)
                {
                    var currentState = _playerManager.CharacterAnimator.GetCurrentAnimatorStateInfo(1);
                    
                    if (currentState.normalizedTime > percent)
                    {
                        _playerManager.StartCoroutine(StartCapturingClickAttackForCombo());
                        yield break;
                    }
                    
                    yield return new WaitForEndOfFrame();
                }
                
                IEnumerator StartCapturingClickAttackForCombo()
                {
                    _isCapturingClickAttackForCombo = true;
                    float timer = 0;
                    var oldStateName = PlayerStateNameHelper.GetStateName(stateInfo);
                    
                    while (timer < _playerManager.ComboClickThreshold)
                    {
                        var currentState = _playerManager.CharacterAnimator.GetCurrentAnimatorStateInfo(1);
                        
                        if (_playerInput.attackInputStatus.Basic)
                        {
                            _clickAttackCaptureInfo = new ClickAttackCaptureInfo
                            {
                                DashVector = DashAttackMoveVector(),
                                LookDirection = _playerInput.mousePositionFlat - _playerManager.transform.position
                            };
                            
                            SetAnimTrigger();
                            
                            _isCapturingClickAttackForCombo = false;
                            yield break;
                        }
                        
                        if (!IsOldAnimationKeepPerforming(currentState))
                        {
                            timer += Time.deltaTime;
                        }
                        
                        yield return new WaitForEndOfFrame();
                    }
                    
                    ClearCombo();
                    _isCapturingClickAttackForCombo = false;
                    yield break;

                    bool IsOldAnimationKeepPerforming(AnimatorStateInfo animatorStateInfo) => 
                        animatorStateInfo.normalizedTime <= 1.0f && animatorStateInfo.IsName(oldStateName);
                }
            }
        }
        
        private void OnAttackStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if(!PlayerStateNameHelper.IsAttacking(stateInfo)) return;
            
            _isAttacking = false;
            _playerManager.AttackInputFreeze = false;
            
            if(IsMoving())
            {
                OnPlayerStateChanged?.Invoke(PlayerState.Move);
            }

            if(IsNotMoving())
            {
                OnPlayerStateChanged?.Invoke(PlayerState.Idle);
            }
            
            return;

            //----------- local functions -----------//
            
            bool IsNotMoving() => _playerInput.movementInputStatus is
                {Down: false, Up: false, Left: false, Right: false};
        
            bool IsMoving() => _playerInput.movementInputStatus.Down ||
                               _playerInput.movementInputStatus.Up ||
                               _playerInput.movementInputStatus.Left ||
                               _playerInput.movementInputStatus.Right;
        }

        #endregion
        
        private Vector3 DashAttackMoveVector()
        {
            var distanceVector = _playerInput.mousePositionFlat - _playerManager.transform.position;

            var scalingFactor = _playerManager.dashLength/distanceVector.magnitude;
            var dashVector = new Vector3
            {
                x =  scalingFactor * distanceVector.x,
                y = 0,
                z = scalingFactor * distanceVector.z
            };
                    
            dashVector += _playerManager.transform.position;
            dashVector.y = 0;

            return dashVector;
        }
        
        private void SetAnimTrigger()
        {
            if (_playerManager.CharacterAnimator.GetBool(PlayerStringAnimParam.AttackParam)||
                _playerManager.CharacterAnimator.GetBool(PlayerStringAnimParam.SecondAttack)||
                _playerManager.CharacterAnimator.GetBool(PlayerStringAnimParam.ThirdAttack)) return;
            
            var animParam = NextPartOfComboAttack switch
            {
                1 => PlayerStringAnimParam.AttackParam,
                2 => PlayerStringAnimParam.SecondAttack,
                3 => PlayerStringAnimParam.ThirdAttack,
                _ => "error"
            };
            
            if (animParam == "error")
            {
                // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
                Debug.LogError($"Not handled part of combo: {NextPartOfComboAttack.ToString()}");
            }
            
            _playerManager.CharacterAnimator.SetTrigger(animParam);
            
            NextPartOfComboAttack += 1;
            
            if (NextPartOfComboAttack > _playerManager.CountOfComboAttackParts)
            {
                ClearCombo();
            }
        }

        private void ClearCombo()
        {
            NextPartOfComboAttack = 1;
        }
    }
}