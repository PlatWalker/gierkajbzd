using jbzd.MainHero;
using UnityEngine;
using Zenject;

namespace jbzd.Common
{
    public class CameraController : MonoBehaviour
    {
        [field:SerializeField]
        public Vector3 CameraOffset { get; set; }
        public float smoothSpeed = 0.05f;
        private GameObject player;
        private Vector3 velocity = Vector3.zero;
        
        private PlayerManager _playerController;
        
        [Inject]
        public void Construct(PlayerManager playerController)
        {
            _playerController = playerController;
        }
    
        private void Start()
        {
            player = _playerController.gameObject;
        }

        private void FixedUpdate()
        {
            Vector3 desiredPosition = player.transform.position + CameraOffset;
            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}
