using jbzd.MinorSystems.Barks;
using UnityEngine;

namespace jbzd.QuestCreationScripts
{
    public class ShowBarkOnColliderEnter : MonoBehaviour
    {
        private PlayerThoughtBarkController _barkController;
        
        private void Awake()
        {
            _barkController = GetComponentInChildren<PlayerThoughtBarkController>();
            Debug.Assert(_barkController, "Missing player thought bark controller");
        }

        public void OnTriggerEnter(Collider other)
        {
            _barkController.ShowPlayerThoughtBark();
        }

        public void OnTriggerExit(Collider other)
        {
            _barkController.HidePlayerThoughtBark();
        }
    }
}