using UnityEngine;
using Zenject;

namespace jbzd.MainHero
{
    public class PlayerStubController:MonoBehaviour
    {
        private PlayerManager _playerManager;
        
        [Inject]
        public void Constructor(PlayerManager playerManager)
        {
            _playerManager = playerManager;
        }

        public void Update()
        {
            gameObject.transform.position = _playerManager.transform.position;
        }
    }
}