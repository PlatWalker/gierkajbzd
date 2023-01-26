using jbzd.MainHero;
using UnityEngine;
using Zenject;

namespace jbzd.Common
{
    public class CameraController : MonoBehaviour
    {
        public float smoothSpeed = 0.05f;
        private Vector3 offset;
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
            offset = new Vector3(0, 11, -6);
            player = _playerController.gameObject;
        }

        private void FixedUpdate()
        {
            Vector3 desiredPosition = player.transform.position + offset;
            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}
