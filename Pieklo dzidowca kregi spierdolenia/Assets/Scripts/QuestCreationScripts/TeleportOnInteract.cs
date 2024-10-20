using jbzd.Common.Interfaces;
using jbzd.MainHero;
using UnityEngine;
using Zenject;

namespace jbzd.QuestCreationScripts
{
    public class TeleportOnInteract : MonoBehaviour, IInteractable
    {
        private PlayerManager _playerManager;
        [SerializeField] private GameObject _gameObjectToTeleportTo;

        [Inject]
        public void Constructor(PlayerManager playerManager)
        {
            _playerManager = playerManager;         
        }

        public void OnInteract()
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
