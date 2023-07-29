using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using jbzd.Common.Enums;
using jbzd.Common.InputSystem.Inputs;
using UnityEngine;

namespace jbzd.MainHero.PlayerStateLogic
{
    public class PlayerStateAttackFixedUpdate : IPlayerStateLogic
    {
        public delegate void PlayerStateChanged(PlayerState playerState);
        public event PlayerStateChanged OnPlayerStateChanged;
        
        private readonly PlayerInput _playerInput;
        private readonly PlayerManager _playerController;

        private (bool, Vector3) _dash;
        
        public PlayerStateAttackFixedUpdate(
            PlayerManager playerController,
            PlayerInput playerInput,
            MainHeroBehaviour behaviour)
        {
            _playerController = playerController;
            _playerInput = playerInput;
            
            behaviour.OnStateEnterPassed += OnAttackStateEnter;
            behaviour.OnStateExitPassed += OnAttackStateExit;
        }
        
        public MonoBehaviourMethod MonoBehaviourMethodInWhichInvoked() => MonoBehaviourMethod.FixedUpdate;

        public PlayerState PlayerStateInWhichInvoked() => PlayerState.Attack;

        public PlayerState StateLogic()
        {
            var playerState = PlayerStateInWhichInvoked();

            if (IsAttacking()) return PlayerState.Attack;

            if (_playerController.CanPlayerMove)
            {
                LookAtMousePosition();
                Attack();
            }
            else
            {
                return PlayerState.Idle;
            }

            return playerState;
            
            //----------- local functions -----------//
            void LookAtMousePosition()
            {
                var flatVector = _playerInput.mousePositionFlat;
                flatVector.y = _playerController.transform.position.y;
                _playerController.transform.LookAt(flatVector);
            }
            
            bool IsAttacking() =>
                _playerController.CharacterAnimator.GetBool(PlayerStringAnimParam.AttackInProgressParam);

            void Attack()
            {
                _playerController.CharacterAnimator.SetTrigger(PlayerStringAnimParam.AttackParam);
                DashAttackMove();
            }
            
            void DashAttackMove()
            {
                var dashVector = _playerInput.mousePositionFlat - _playerController.transform.position;
            
                UpdateCharacterRotation(dashVector.normalized);
                
                var positionToDashTo = new Vector3
                {
                    x = Math.Abs(dashVector.x) > 2 ? dashVector.x > 0 ? 2 : -2 : dashVector.x,
                    z = Math.Abs(dashVector.z) > 2 ? dashVector.z > 0 ? 2 : -2 : dashVector.z
                };

                positionToDashTo += _playerController.transform.position;
                positionToDashTo.y = 0;

                _dash = (true, positionToDashTo);
            }
            
            void UpdateCharacterRotation(Vector3 lookDirection)
            {
                if (lookDirection.magnitude == 0) return;
            
                var rotation = Quaternion.LookRotation(lookDirection);
            
                _playerController.transform.rotation = rotation;
            }
        }

        private bool IsAttackAnimation(AnimatorStateInfo stateInfo) =>
            stateInfo.IsName("Atk1") || stateInfo.IsName("Atk2") || stateInfo.IsName("Atk3") ||
            stateInfo.IsName("Atk4");
        
        private void OnAttackStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if(!IsAttackAnimation(stateInfo)) return;

            animator.SetBool(PlayerStringAnimParam.AttackInProgressParam, true);
            _playerController.AttackInputFreeze = false;
            
            if(!_dash.Item1) return;
            
            _playerController.StartCoroutine(MoveOverSeconds(
                _playerController.Rb,
                _dash.Item2,
                stateInfo.length));

            _dash = (false, Vector3.zero);
        }

        private void OnAttackStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if(!IsAttackAnimation(stateInfo)) return;

            animator.SetBool(PlayerStringAnimParam.AttackInProgressParam, false);
            _playerController.AttackInputFreeze = true;
            
            if(IsMoving())
            {
                OnPlayerStateChanged?.Invoke(PlayerState.Move);
            }

            if(IsNotMoving())
            {
                OnPlayerStateChanged?.Invoke(PlayerState.Idle);
            }
            
            //----------- local functions -----------//
            
            bool IsNotMoving() => _playerInput.movementInputStatus is
                {Down: false, Up: false, Left: false, Right: false};
        
            bool IsMoving() => _playerInput.movementInputStatus.Down ||
                               _playerInput.movementInputStatus.Up ||
                               _playerInput.movementInputStatus.Left ||
                               _playerInput.movementInputStatus.Right;
        }
        
        private IEnumerator MoveOverSeconds(Rigidbody rigidbodyToMove, Vector3 end, float seconds)
        {
            float elapsedTime = 0;
            var startingPos = rigidbodyToMove.position;
            var col = _playerController.CharacterCollider;
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
                var point0 = _playerController.transform.TransformPoint(localPoint0);
                var point1 = _playerController.transform.TransformPoint(localPoint1);
                var r = _playerController. transform.TransformVector(colRadius, colRadius, colRadius);
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
}