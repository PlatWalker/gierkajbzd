using jbzd.MainHero;
using UnityEngine;
using Zenject;

namespace jbzd.Dialogues.Barks
{
    public class FollowPlayer : MonoBehaviour
    {
        private PlayerManager _player;

        [Inject]
        public void Constructor(PlayerManager manager)
        {
            _player = manager;
        }
        
        void Update()
        {
            transform.position = _player.gameObject.transform.position;
        }
    }
}
