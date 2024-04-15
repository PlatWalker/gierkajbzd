using System.Collections;
using System.Collections.Generic;
using jbzd.MainHero;
using MyBox;
using UnityEngine;
using Zenject;

namespace jbzd.Map.Krag1
{

    public class TeleportOnBorder : MonoBehaviour
    {
        
        public float x = 0;
        public float z = 0;

        private PlayerManager _playerManager;

        [Inject]
        public void Construct(PlayerManager playerManager)
        {
            _playerManager = playerManager;
        }

        void OnCollisionEnter(Collision collision) 
        {
            if (x != 0) {
                _playerManager.PlaceAt(new Vector3(
                    x,
                    _playerManager.transform.position.y,
                    _playerManager.transform.position.z
                ));
            }

            if (z != 0) {
                _playerManager.PlaceAt(new Vector3(
                    _playerManager.transform.position.x,
                    _playerManager.transform.position.y,
                    z
                ));
            }
        }
    }
}
