using System;
using jbzd.Cutscenes;
using jbzd.MainHero;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace jbzd.NPC
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class NpcController : MonoBehaviour
    {
        public enum NpcStates
        {
            Undefined,
            Idle,
            FollowPlayer,
            RunAwayFromPlayer
        }

        [SerializeField] private NpcStates npcState = NpcStates.Idle;
        
        public NpcStates NpcState
        {
            get => npcState;
            set
            {
                if (npcState is NpcStates.RunAwayFromPlayer && value is not NpcStates.RunAwayFromPlayer)
                {
                    _currentWaypointIndex = 0;
                }

                if (value is NpcStates.FollowPlayer)
                {
                    _playerManager.ListOfFollowers.Add(this);
                }

                if (npcState is NpcStates.FollowPlayer && value is not NpcStates.FollowPlayer)
                {
                    _playerManager.ListOfFollowers.Remove(this);
                }
                
                npcState = value;
            } 
        }

        [field:SerializeField]
        [Tooltip("If Npc need to follow a predefined path in some situation, add waypoints from scene for him to follow" +
                 "them one by one.")]
        public Transform[] Waypoints { get; set; }
        public NavMeshAgent NpcAgent { get; private set; }

        private PlayerManager _playerManager;
        private CutscenesManager _cutscenesManager;
        private int _currentWaypointIndex;
        private Animator _animator;
        
        private static readonly int IsWalking = Animator.StringToHash("isWalking");


        [Inject]
        public void Constructor(PlayerManager playerManager, CutscenesManager cutscenesManager)
        {
            _playerManager = playerManager;
            _cutscenesManager = cutscenesManager;
        }

        public void Awake()
        {
            NpcAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponentInChildren<Animator>();
        }

        public void Update()
        {
            switch (NpcState)
            {
                case NpcStates.Idle:
                    _animator.SetBool(IsWalking, false);
                    break;
                case NpcStates.FollowPlayer:
                    NpcAgent.SetDestination(_playerManager.transform.position - Vector3.one);
                    if (NpcAgent.remainingDistance > 1f)
                    {
                        _animator.SetBool(IsWalking, true);
                    }
                    else
                    {
                        _animator.SetBool(IsWalking, false);
                    }
                    break;
                case NpcStates.RunAwayFromPlayer:
                    _animator.SetBool(IsWalking, true);
                    if (!(NpcAgent.remainingDistance < 0.01f)) return;
                    if (_currentWaypointIndex > Waypoints.Length - 1) return;

                    NpcAgent.SetDestination(Waypoints[_currentWaypointIndex].position);
                    _currentWaypointIndex += 1;
                    break;
                case NpcStates.Undefined:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Start()
        {
            _cutscenesManager.OnCutsceneStarted += OnCutsceneStarted;
            _cutscenesManager.OnCutsceneEnded += OnCutsceneEnded;
        }
        
        public void OnDestroy()
        {
            _cutscenesManager.OnCutsceneStarted -= OnCutsceneStarted;
            _cutscenesManager.OnCutsceneEnded -= OnCutsceneEnded;
        }
        
        private void OnCutsceneStarted()
        {
            if (NpcStates.FollowPlayer == NpcState)
            {
                NpcAgent.gameObject.transform.position = _playerManager.transform.position;
                NpcAgent.gameObject.SetActive(false);
            }
        }

        private void OnCutsceneEnded()
        {
            if (NpcStates.FollowPlayer == NpcState)
            {
                NpcAgent.gameObject.SetActive(true);
            }
        }
    }
}
