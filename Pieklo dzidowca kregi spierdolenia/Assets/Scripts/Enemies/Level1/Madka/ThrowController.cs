using jbzd.Common;
using jbzd.MainHero;
using UnityEngine;
using Zenject;

namespace jbzd.Enemies.Level1.Madka
{
    public class ThrowController: MonoBehaviour
    {
        [SerializeField]
        private GameObject projectileSpawnPoint;
        [SerializeField]
        private GameObject projectilePrefab;
        [SerializeField]
        private float throwPower;
        [SerializeField]
        private float throwHeight;
        
        private PlayerManager _playerManager;
        private AnimationEventHandler _animationEventHandler;
        
        [Inject]
        private void Constructor(PlayerManager playerManager)
        {
            _playerManager = playerManager;
        }
        
        public void Awake()
        {
            Debug.Assert(projectileSpawnPoint, $"Missing {nameof(projectileSpawnPoint)} in {name}");
            Debug.Assert(projectilePrefab, $"Missing {nameof(projectilePrefab)} in {name}");
        }

        public void Start()
        {
            _animationEventHandler = GetComponentInChildren<AnimationEventHandler>();

            if (_animationEventHandler is null)
            {
                Debug.LogError($"Missing {nameof(AnimationEventHandler)} in children object");
                return;
            }
            
            _animationEventHandler.OnEventFired += HandleEvent;
        }

        private void HandleEvent(AnimationEvent animationEvent)
        {
            var projectile = Instantiate(projectilePrefab, projectileSpawnPoint.transform.position, Quaternion.identity);

            var throwDirection = (_playerManager.transform.position - projectile.transform.position).normalized;
            throwDirection *= throwPower;
            throwDirection.y = throwHeight;
            
            var rb = projectile.GetComponent<Rigidbody>();
            rb.AddForce(throwDirection, ForceMode.Impulse);
        }

        private void OnDestroy()
        {
            _animationEventHandler.OnEventFired -= HandleEvent;
        }
    }
}