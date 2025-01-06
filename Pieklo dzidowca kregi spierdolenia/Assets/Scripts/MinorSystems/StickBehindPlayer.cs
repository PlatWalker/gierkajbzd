using jbzd.MainHero;
using UnityEngine;
using Zenject;

namespace jbzd
{
    public class StickBehindPlayer : MonoBehaviour
    {
        private Transform _playerTransform;
        [field: SerializeField] Transform target;
        [field: SerializeField] private Vector3 offset;
        [field: SerializeField] private Vector3 targetScale;
        [field: SerializeField] private float forceStrength = 10f;
        [field: SerializeField] private float smoothSpeed = 5f;
        [field: SerializeField] private float drag = 0.95f;
        private Rigidbody _rigidbody;
         
        [Inject]
        public void Constructor(PlayerManager playerManager)
        {
            _playerTransform = playerManager.transform;
        }
        
        void Start()
        {
            _rigidbody = target.GetComponent<Rigidbody>();
            if (_rigidbody == null)
            {
                _rigidbody = target.gameObject.AddComponent<Rigidbody>();
            }
        
            _rigidbody.useGravity = false; 
            _rigidbody.drag = 0;          
        }
        
        private void FixedUpdate()
        {
            Vector3 desiredPosition = _playerTransform.position + offset;

            Vector3 forceDirection = (desiredPosition - target.position).normalized;
            float distance = Vector3.Distance(target.position, desiredPosition);
            _rigidbody.AddForce(forceDirection * (forceStrength * distance));

            _rigidbody.velocity *= drag;
            
            target.rotation = Quaternion.Lerp(target.rotation, _playerTransform.rotation, smoothSpeed * Time.deltaTime);
            target.localScale = Vector3.Lerp(target.localScale, targetScale, smoothSpeed * Time.deltaTime);
        }
    }
}
