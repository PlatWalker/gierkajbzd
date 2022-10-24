using System.Collections.Generic;
using jbzdy.CharacterStats;
using System.Linq;
using UnityEngine;
using Zenject;
using jbzd.Common.InputSystem;
using jbzd.Common.InputSystem.Inputs;
using jbzd.Common.Interfaces;
using jbzd.MainHero.PlayerStateLogic;

namespace jbzd.MainHero
{
    public struct PlayerStringAnimParam
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
        [SerializeField]
        private PlayerState playerState = PlayerState.Idle;
        #endregion

        #region Public variables
        public List<Collider> ListOfEnemiesColliders { get; } = new();
        public bool NextFrameDash { get; set; }
        public Animator CharacterAnimator { get; private set; }
        public Rigidbody Rb { get; private set; }
        #endregion

        #region Private variables
        private PlayerInput _playerInput;
        private PlayerStats _playerStats;
        private List<IPlayerStateLogic> _stateLogicObjects;
        #endregion

        [Inject]
        public void Construct(InputManager inputManager)
        {
            _playerInput = inputManager.GetInput<PlayerInput>();
        }

        private void Start()
        {
            RegisterStatesLogic();
            
            CharacterAnimator = GetComponentInChildren<Animator>();
            if (CharacterAnimator == null) Debug.Log("Nie znaleziono animatora w postaci gracza!");
            
            var behaviour = CharacterAnimator.GetBehaviours<MainHeroBehaviour>().First();

            behaviour.OnStateEnterPassed += OnStateEnterJbzd;
            behaviour.OnStateExitPassed += OnStateExitJbzd;

            AnimatorParametersCheck();
            
            Rb = GetComponent<Rigidbody>();
            _playerStats = GetComponent<PlayerStats>();
        }
        
        private void RegisterStatesLogic()
        {
            _stateLogicObjects = new List<IPlayerStateLogic>
            {
                new PlayerStateIdleUpdate(this, _playerInput),
                new PlayerStateAttackUpdate(this, _playerInput),
                new PlayerStateMoveFixedUpdate(this, _playerInput)
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

        private void AnimatorParametersCheck()
        {
            if(CharacterAnimator.parameters.Any(x => x.name == PlayerStringAnimParam.AttackParam) == false) 
                Debug.Log("Blad w nazwie parametru atakowania");
            if(CharacterAnimator.parameters.Any(x => x.name == PlayerStringAnimParam.AttackInProgressParam) == false)
                Debug.Log("Blad w nazwie parametru progresu animacji atakowania");
            if(CharacterAnimator.parameters.Any(x => x.name == PlayerStringAnimParam.RunParam) == false)
                Debug.Log("Blad w nazwie parametru biegania");
            if (CharacterAnimator.parameters.Any(x => x.name == PlayerStringAnimParam.RunSpeed) == false)
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
                animator.SetBool(PlayerStringAnimParam.AttackInProgressParam, true);
            }

            animator.SetBool(PlayerStringAnimParam.AttackParam, false);
        }

        private void OnStateExitJbzd(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!isTransitionStateJbzd(stateInfo))
                animator.SetBool(PlayerStringAnimParam.AttackParam, false);

            if (isAttackAnimationPlayingJbzd(stateInfo))
                animator.SetBool(PlayerStringAnimParam.AttackInProgressParam, false);            
        }
    } 
}