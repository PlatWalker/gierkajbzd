using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using jbzd.Common;
using UnityEngine;
using Zenject;
using jbzd.Common.InputSystem;
using jbzd.Common.InputSystem.Inputs;
using jbzd.Cutscenes;
using jbzd.MainHero.PlayerControllers;
using jbzd.MainHero.PlayerStateLogic;
using jbzd.NPC;
using jbzd.SavingSystem;
using UnityEngine.AI;

namespace jbzd.MainHero
{
    public struct PlayerStringAnimParam
    {
        public static string AttackParam => "Attack";
        public static string SecondAttack => "Second attack";
        public static string RunParam => "Run";
        public static string RunSpeed => "Running speed";
        
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public static void AnimatorParametersCheck(Animator characterAnimator)
        {
            if(characterAnimator.parameters.Any(x => x.name == AttackParam) == false) 
                Debug.Log("Blad w nazwie parametru atakowania");
            if (characterAnimator.parameters.Any(x => x.name == SecondAttack) == false)
                Debug.Log("Blad w nazwie parametru drugiego ataku");
            if(characterAnimator.parameters.Any(x => x.name == RunParam) == false)
                Debug.Log("Blad w nazwie parametru biegania");
            if (characterAnimator.parameters.Any(x => x.name == RunSpeed) == false)
                Debug.Log("Blad w nazwie parametru szybkosci biegania");
        }
    }

    public class PlayerManager : MonoBehaviour, ISaveable
    {
        #region Inspector Fields

        [SerializeField]
        [JbzdReadOnly] 
        private bool canPlayerMove = true;

        [field: SerializeField]
        [field: Range(1f, 10f)]
        public float PlayerSpeed { get; private set; } = 5;

        [field: Tooltip(
            "From what point of animation (in percentage) of attack register mouse click for character to perform next attack from combo." +
            "for ex. 30% will result in ability to click after 30 % animation has passed")]
        [field: SerializeField]
        [field: Range(1, 100)]
        public int ComboClickPercentage { get; set; } = 30;

        [field: Tooltip("How much time in seconds after attack, player can click for combo continuation")]
        [field: SerializeField]
        [field: Range(0.01f, 1)]
        public float ComboClickThreshold { get; set; } = 0.01f;
        
        [SerializeField] 
        private PlayerState playerState = PlayerState.Idle;

        [Range(1,5)]
        public float dashLength = 2;

        [JbzdReadOnly]
        public List<NpcController> ListOfFollowers = new();
        
        #endregion

        #region Public variables

        private int _countOfComboAttackParts = 2;
        
        /// <summary>
        /// Number of parts of combo for actual equipped weapon
        /// </summary>
        public int CountOfComboAttackParts
        {
            get => _countOfComboAttackParts;
            set
            {
                if (value is 0 or < 0)
                {
                    Debug.LogError($"{nameof(_countOfComboAttackParts)} can't be set to 0 or less");
                }
                
                _countOfComboAttackParts = value;
            }
        }
        
        public bool CanPlayerMove
        {
            get => canPlayerMove;
            set
            {
                if (value)
                {
                    _canAttackUnfreeze = true;
                    canPlayerMove = true;
                }
                else
                {
                    _canAttackUnfreeze = false;
                    canPlayerMove = false;
                }
            }
        }

        private bool _canAttackUnfreeze = true;

        /// <summary>
        /// Player freezing, can be used only for freezing player when attacking
        /// </summary>
        public bool AttackInputFreeze
        {
            set
            {
                if (!_canAttackUnfreeze) return;
                
                canPlayerMove = !value;
            }
        }
        
        public Animator CharacterAnimator { get; private set; }
        public Rigidbody Rb { get; private set; }
        public CapsuleCollider CharacterCollider { get; private set; }

        #endregion

        #region Private variables
        
        private PlayerInput _playerInput;
        private List<IPlayerStateLogic> _stateLogicObjects;
        private List<IPlayerController> _playerControllers;
        private Vector3 _newPositionVector;

        #endregion

        [Inject]
        public void Construct(InputManager inputManager, CutscenesManager cutscenesManager)
        {
            _playerInput = inputManager.GetInput<PlayerInput>();
            cutscenesManager.OnCutsceneStarted += Dissapear;
            cutscenesManager.OnCutsceneEnded += Reappear;
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
            ((PlayerStateAttack) _stateLogicObjects.First(stateLogicObject => stateLogicObject is PlayerStateAttack))
                .OnPlayerStateChanged += state => playerState = state;

            PlayerStringAnimParam.AnimatorParametersCheck(CharacterAnimator);

            canPlayerMove = true;
        }

        private void RegisterStatesLogic()
        {
            var behaviour = CharacterAnimator.GetBehaviours<MainHeroBehaviour>().First();
            
            _stateLogicObjects = new List<IPlayerStateLogic>
            {
                new PlayerStateAttack(this, _playerInput, behaviour),
                new PlayerStateMove(this, _playerInput),
                new PlayerStateIdle(_playerInput)
            };

            var consistsDuplicates = _stateLogicObjects
                .GroupBy(x => x.InitPlayerState)
                .Any(x => x.Count() > 1);
            
            if (consistsDuplicates)
            {
                Debug.LogError("Nie moze byc dwoch stateObjectow z tym samym inicjalnym player statem");
            }
        }
        
        private void FixedUpdate()
        {
            StickPlayerToGround();

            playerState = GetStateLogicObjectForCurrentPlayerState().StateLogicForFixedUpdate();
            
            //----------- local functions -----------//
            
            void StickPlayerToGround()
            {
                if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out RaycastHit hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Ground")))
                {
                    _newPositionVector.x = Rb.position.x;
                    _newPositionVector.y = hit.point.y;
                    _newPositionVector.z = Rb.position.z;

                    Rb.MovePosition(_newPositionVector);
                }
            }
        }

        private void Update()
        {
            playerState = GetStateLogicObjectForCurrentPlayerState().StateLogicForUpdate();

            //Synchronising running animation with character speed
            var runningSpeed = new Vector3(Rb.velocity.x, 0f, Rb.velocity.z).magnitude;
            CharacterAnimator.SetFloat(PlayerStringAnimParam.RunSpeed, runningSpeed);
        }

        private void LateUpdate()
        {
            playerState = GetStateLogicObjectForCurrentPlayerState().StateLogicForLateUpdate();
        }
        
        private IPlayerStateLogic GetStateLogicObjectForCurrentPlayerState()
        {
            var logic = _stateLogicObjects.FirstOrDefault(x => x.InitPlayerState == playerState);

            if (logic is not null) return logic;
            
            Debug.LogError($"missing logic for state {playerState.ToString()}");
            return new ErrorPlayerState();

        }

        public void LoadData(GameData gameData)
        {
            gameObject.transform.position = gameData.playerPosition;
        }

        public void SaveData(ref GameData gameData)
        {
            gameData.playerPosition = gameObject.transform.position;
        }
        
        public T GetPlayerController<T>() where T : IPlayerController
        {
            var playerController = (T) _playerControllers.Find(playerController => playerController.GetType() == typeof(T));
            
            if (playerController != null) return playerController;

            Debug.LogWarning("There is no such player controller class!");
            return default;
        }

        public void PlaceAt(Vector3 placement) => transform.position = placement;

        public void WarpFollowersToPlayer()
        {
            foreach (var npcController in ListOfFollowers)
            {
                npcController.GetComponent<NavMeshAgent>().Warp(transform.position);
            }
        }

        public void Dissapear(){
            MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer meshRenderer in meshRenderers)
            {
                meshRenderer.enabled = false;
            }            
            SkinnedMeshRenderer[] skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (SkinnedMeshRenderer skinnedMeshRenderer in skinnedMeshRenderers)
            {
                skinnedMeshRenderer.enabled = false;
            }
        }

        public void Reappear(){
            MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer meshRenderer in meshRenderers)
            {
                meshRenderer.enabled = true;
            }
            SkinnedMeshRenderer[] skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (SkinnedMeshRenderer skinnedMeshRenderer in skinnedMeshRenderers)
            {
                skinnedMeshRenderer.enabled = true;
            }
        }

    } 
}