using jbzd.MainHero;
using UnityEngine;
using Zenject;

namespace jbzd.QuestCreationScripts
{
    public class TeleportOnTrigger : MonoBehaviour
    {
        private PlayerManager _playerManager;
        [SerializeField] private GameObject _gameObjectToTeleportTo;

        [Inject]
        public void Constructor(PlayerManager playerManager)
        {
            _playerManager = playerManager;         
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_gameObjectToTeleportTo == null)
            {
                Debug.LogWarning("Gameobject to teleport player to is not assigned");
                return;
            }
            _playerManager.PlaceAt(_gameObjectToTeleportTo.transform.position);
            _playerManager.WarpFollowersToPlayer();
        }
    }
}
