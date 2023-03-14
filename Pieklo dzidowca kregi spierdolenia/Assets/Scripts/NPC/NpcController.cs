using System;
using jbzd.MainHero;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
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
            } 
        }

        [field:SerializeField]
        [Tooltip("If Npc need to follow a predefined path in some situation, add waypoints from scene for him to follow" +
                 "them one by one.")]
        public Transform[] Waypoints { get; set; }
        public NavMeshAgent NpcAgent { get; private set; }

        private PlayerManager _playerManager;
        private int _currentWaypointIndex;

        [Inject]
        public void Constructor(PlayerManager playerManager)
        {
            _playerManager = playerManager;
        }
        
        public void Awake()
        {
            NpcAgent = GetComponent<NavMeshAgent>();
        }
        
        public void Update()
        {
            switch (NpcState)
            {
                case NpcStates.Idle:
                    break;
                case NpcStates.FollowPlayer:
                    NpcAgent.SetDestination(_playerManager.transform.position - Vector3.one);
                    break;
                case NpcStates.RunAwayFromPlayer:
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
        
    }
}
