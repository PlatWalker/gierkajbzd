using jbzd.MainHero;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace jbzd.NPC
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class NpcController : MonoBehaviour
    {
        private PlayerManager _playerManager;
        private NavMeshAgent _agent;
        
        [SerializeField] private bool followsPlayer;

        public bool FollowsPlayer
        {
            get => followsPlayer;
            set => followsPlayer = value;
        }

        [Inject]
        public void Constructor(PlayerManager playerManager)
        {
            _playerManager = playerManager;
        }
        
        public void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }
        
        public void Update()
        {
            if(followsPlayer) _agent.SetDestination(_playerManager.transform.position - Vector3.one);
        }
        
    }
}
