using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using jbzd.Common;
using jbzd.Common.Enums;
using UnityEngine;
using Zenject;
using jbzd.Common.InputSystem;
using jbzd.Common.InputSystem.Inputs;
using jbzd.MainHero.LegacyHeroThings.Stats;
using jbzd.MainHero.PlayerControllers;
using jbzd.MainHero.PlayerStateLogic;
using jbzd.NPC;
using UnityEngine.AI;

namespace jbzd.MainHero
{
    public struct PlayerStringAnimParam
    {
        public static string AttackParam => "Attack";
        public static string AttackInProgressParam => "Attacking animation in progress";
        public static string RunParam => "Run";
        public static string RunSpeed => "Running speed";
        
        public static void AnimatorParametersCheck(Animator characterAnimator)
        {
            if(characterAnimator.parameters.Any(x => x.name == AttackParam) == false) 
                Debug.Log("Blad w nazwie parametru atakowania");
            if(characterAnimator.parameters.Any(x => x.name == AttackInProgressParam) == false)
                Debug.Log("Blad w nazwie parametru progresu animacji atakowania");
            if(characterAnimator.parameters.Any(x => x.name == RunParam) == false)
                Debug.Log("Blad w nazwie parametru biegania");
            if (characterAnimator.parameters.Any(x => x.name == RunSpeed) == false)
                Debug.Log("Blad w nazwie parametru szybkosci biegania");
        }
    }

    public class PlayerManager : MonoBehaviour
    {
        #region Serialized fields

        [SerializeField][JbzdReadOnly] private bool canPlayerMove = true;
        public bool CanPlayerMove
        {
            get => canPlayerMove;
            set => canPlayerMove = value;
        }

        [field:SerializeField]
        [field: Range(1f, 10f)]
        public float PlayerSpeed { get; private set; }
        [field:SerializeField] 
        [field: Range(200f, 1500f)]
        public float DashAttackMovePower { get; private set; }
        [SerializeField] private PlayerState playerState = PlayerState.Idle;
        
        #endregion

        #region Public variables
        
        public List<NpcController> ListOfFollowers = new();
        public List<Collider> ListOfEnemiesColliders { get; } = new();

        public Animator CharacterAnimator { get; private set; }
        public Rigidbody Rb { get; private set; }
        public CapsuleCollider CharacterCollider { get; private set; }

        #endregion

        #region Private variables
        
        private PlayerInput _playerInput;
        private PlayerStats _playerStats;
        private List<IPlayerStateLogic> _stateLogicObjects;
        private List<IPlayerController> _playerControllers;
        private Vector3 _newPositionVector;

        #endregion

        [Inject]
        public void Construct(InputManager inputManager)
        {
            _playerInput = inputManager.GetInput<PlayerInput>();
        }

        public void Awake()
        {
            _playerControllers = GetComponents<IPlayerController>().ToList();
        }

        private void Start()
        {
            CharacterCollider = GetComponent<CapsuleCollider>();
            CharacterAnimator = GetComponentInChildren<Animator>();
            Rb = GetComponent<Rigidbody>();

            if (CharacterAnimator is null ||
                CharacterCollider is null||
                Rb is null)
            {
                Debug.LogError("Nie znaleziono wymaganego komponentu na graczu albo w jego dzieciach!");
            }
            
            RegisterStatesLogic();
            ((PlayerStateAttackFixedUpdate) _stateLogicObjects.First(stateLogicObject => stateLogicObject is PlayerStateAttackFixedUpdate))
                .OnPlayerStateChanged += state => playerState = state;

            PlayerStringAnimParam.AnimatorParametersCheck(CharacterAnimator);

            canPlayerMove = true;
        }

        private void RegisterStatesLogic()
        {
            var behaviour = CharacterAnimator.GetBehaviours<MainHeroBehaviour>().First();
            
            _stateLogicObjects = new List<IPlayerStateLogic>
            {
                new PlayerStateIdleUpdate(this, _playerInput, behaviour),
                new PlayerStateAttackFixedUpdate(this, _playerInput, behaviour),
                new PlayerStateMoveFixedUpdate(this, _playerInput, behaviour),
                new PlayerStateMoveLateUpdate(this, _playerInput)
            };

            var temp = new List<IPlayerStateLogic>();
            
            foreach (var stateLogicObject in _stateLogicObjects)
            {
                if (temp.Any(x =>
                        x.MonoBehaviourMethodInWhichInvoked() == stateLogicObject.MonoBehaviourMethodInWhichInvoked() &&
                        x.PlayerStateInWhichInvoked() == stateLogicObject.PlayerStateInWhichInvoked()))
                {
                    Debug.LogError("Nie moze być dwoch stateLogicObject przypisanych do tego samego playerstate'u!");
                }
                
                temp.Add(stateLogicObject);
            }
        }
        
        private void FixedUpdate()
        {
            StickPlayerToGround();

            var playerStateLogic = _stateLogicObjects.FirstOrDefault(x =>
                x.MonoBehaviourMethodInWhichInvoked() == MonoBehaviourMethod.FixedUpdate &&
                x.PlayerStateInWhichInvoked() == playerState);

            if (playerStateLogic != null) playerState = playerStateLogic.StateLogic();
        }

        private void Update()
        {
            
            var playerStateLogic = _stateLogicObjects.FirstOrDefault(x =>
                x.MonoBehaviourMethodInWhichInvoked() == MonoBehaviourMethod.Update &&
                x.PlayerStateInWhichInvoked() == playerState);

            if (playerStateLogic != null) playerState = playerStateLogic.StateLogic();

            //Synchronising running animation with character speed
            CharacterAnimator.SetFloat(PlayerStringAnimParam.RunSpeed, Rb.velocity.magnitude);
        }

        private void LateUpdate()
        {
            var playerStateLogic = _stateLogicObjects.FirstOrDefault(x =>
                x.MonoBehaviourMethodInWhichInvoked() == MonoBehaviourMethod.LateUpdate &&
                x.PlayerStateInWhichInvoked() == playerState);

            if (playerStateLogic != null) playerState = playerStateLogic.StateLogic();
        }

        public T GetPlayerController<T>() where T : IPlayerController
        {
            var playerController = (T) _playerControllers.Find(playerController => playerController.GetType() == typeof(T));
            
            if (playerController != null) return playerController;

            Debug.LogWarning("There is no such player controller class!");
            return default;
        }

        public void PlaceAt(Vector3 placement) => transform.position = placement;

        private void StickPlayerToGround()
        {
            if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out RaycastHit hit) 
                && hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                _newPositionVector.x = Rb.position.x;
                _newPositionVector.y = hit.point.y;
                _newPositionVector.z = Rb.position.z;

                Rb.MovePosition(_newPositionVector);
            }
        }

        public void WarpFollowersToPlayer()
        {
            foreach (var npcController in ListOfFollowers)
            {
                npcController.GetComponent<NavMeshAgent>().Warp(transform.position);
            }
        }
    } 
}