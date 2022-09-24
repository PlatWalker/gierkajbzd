using System.Collections.Generic;
using jbzd;
using jbzdy.Player;
using UnityEngine;
using Zenject;

namespace UI
{
    public class PointerController : MonoBehaviour
    {
        private IEnumerable<GameObject> _allNpc;
        private PlayerController _playerController;
        
        [Inject]
        public void Construct(PlayerController playerController)
        {
            _playerController = playerController;
        }

        private void Awake()
        {
            _allNpc = GameObject.FindGameObjectsWithTag("Npc");
        }
        
        private void Update()
        {
            GameObject nearestNPC = null;
            var distanceToNearestNPC = float.PositiveInfinity;
        
            foreach (var NPC in _allNpc)
            {
                var distanceToNPC = Vector3.Distance(NPC.transform.position, _playerController.transform.position);

                if (!(distanceToNPC < distanceToNearestNPC)) continue;
            
                nearestNPC = NPC;
                distanceToNearestNPC = distanceToNPC;
            }
        
            var vectorToNearestNPC = nearestNPC.transform.position - _playerController.transform.position;
            vectorToNearestNPC.Normalize();
            var angleBetweenVectors = Vector3.Angle(vectorToNearestNPC, Vector3.right);
            if (Vector3.Cross(vectorToNearestNPC, Vector3.right).y < 0) angleBetweenVectors = -angleBetweenVectors;
        
            var eulerAngles = transform.eulerAngles;
            eulerAngles = new Vector3(
                eulerAngles.x,
                eulerAngles.y,
                angleBetweenVectors - 180
            );
            transform.eulerAngles = eulerAngles;
        }
    }
}
