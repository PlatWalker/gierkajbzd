using jbzd.MainHero;
using jbzd.MinorSystems.Barks;
using jbzd.NPC;
using UnityEngine;
using Zenject;

namespace jbzd.QuestCreationScripts
{
    public class ActionsAfterTime : MonoBehaviour
    {
        [SerializeField] private float targetTime = 60.0f;
        private Collider collider;
        private bool triggered = false;
        private bool timerEnded = false;
        [SerializeField] private NpcController npcController;
        [SerializeField] private BarkController barkController;
        [SerializeField] private GameObject objectToDisable;

        private PlayerManager _playerManager;
        [SerializeField] private GameObject _gameObjectToTeleportTo;

        [Inject]
        public void Constructor(PlayerManager playerManager)
        {
            _playerManager = playerManager;         
        }
        
        private void Awake()
        {
            collider = GetComponent<Collider>();    
        }

        private void Update(){

            targetTime -= Time.deltaTime;

            if (!timerEnded && targetTime <= 0.0f)
            {
                collider.enabled = true;
                timerEnded = true;
                npcController.NpcState = NpcController.NpcStates.RunAwayFromPlayer;
                objectToDisable.SetActive(false);
            }

        }

        private void TriggeredActions()
        {
            triggered = true;
            if (_gameObjectToTeleportTo == null)
            {
                Debug.LogWarning("Gameobject to teleport player to is not assigned");
                return;
            }

            barkController.ShowNpcBark("Wypierdalaj");
            _playerManager.PlaceAt(_gameObjectToTeleportTo.transform.position);
            _playerManager.WarpFollowersToPlayer();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (timerEnded)
                TriggeredActions();
        }

        private void OnTriggerStay(Collider other)
        {
            if (timerEnded)
                TriggeredActions();
        }
    }
}
