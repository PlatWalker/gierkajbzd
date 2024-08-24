using jbzd.MainHero;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace jbzd.MinorSystems.Barks
{
    public class PlayerThoughtBarkController : MonoBehaviour
    {
        [SerializeField] private TMP_Text thoughtTmpTextComponent;
        [SerializeField] private GameObject thoughtCloudGameObject;
        [SerializeField][TextArea(5,20)] private string thoughtText;
        
        private bool _shouldImageFollowPlayer;
        private PlayerManager _playerManager;
        private Camera _camera;
        private Collider _playerCollider;
        private RectTransform _thoughtCloudRectTransform;
        
        [Inject]
        public void Constructor(PlayerManager playerManager)
        {
            _playerManager = playerManager;
        }

        private void Awake()
        {
            _camera = Camera.main;
            _playerCollider = _playerManager.GetComponent<Collider>();
            _thoughtCloudRectTransform = thoughtCloudGameObject.GetComponent<RectTransform>();
            
            Debug.Assert(_camera, "Missing main camera");
            Debug.Assert(_thoughtCloudRectTransform, "Missing game object for thought cloud");
            Debug.Assert(_playerCollider, "Missing player collider");
        }

        private void Update()
        {
            if(!_shouldImageFollowPlayer) return;
            
            const float offsetTextCloudStartPointToReferencePointXaxis = 40f;
            
            var centerPosition = _playerCollider.bounds.center;
            var halfHeight = _playerCollider.bounds.extents.y;
            var topPoint = new Vector3(centerPosition.x, centerPosition.y + halfHeight, centerPosition.z);
                
            var result = _camera.WorldToScreenPoint(topPoint);
            var sizeDelta = _thoughtCloudRectTransform.sizeDelta;
            _thoughtCloudRectTransform.position = result - new Vector3(sizeDelta.x * 0.5f - offsetTextCloudStartPointToReferencePointXaxis, -sizeDelta.y * 0.5f, 0);
        }
        
        /// <summary>
        /// Shows thought bark over player, text for bark are taken from <see cref="thoughtText"/>.
        /// </summary>
        public void ShowPlayerThoughtBark()
        {
            _shouldImageFollowPlayer = true;
            thoughtCloudGameObject.SetActive(true);
            thoughtTmpTextComponent.text = thoughtText;
        }
        
        /// <summary>
        /// Hides player thought bark, started by <see cref="ShowPlayerThoughtBark"/>.
        /// </summary>
        public void HidePlayerThoughtBark()
        {
            _shouldImageFollowPlayer = false;
            thoughtCloudGameObject.SetActive(false);
            thoughtTmpTextComponent.text = "";
        }
    }
}