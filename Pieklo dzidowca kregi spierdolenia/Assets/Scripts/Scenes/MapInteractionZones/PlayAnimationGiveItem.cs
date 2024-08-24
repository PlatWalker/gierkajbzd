using jbzd.Common.Interfaces;
using jbzd.Items;
using jbzd.MainHero;
using jbzd.MainHero.PlayerControllers;
using jbzd.MinorSystems.Interact;
using jbzd.QuestSystem;
using UnityEngine;
using Zenject;

namespace jbzd.Scenes.MapInteractionZones
{
    public class PlayAnimationGiveItem : MonoBehaviour, IInteractable
    {
        [SerializeField] private ItemSO itemToGive;
        [SerializeField] private int itemCount;
        
        private Interaction _interactionScript;
        private Animator _animator;
        private InventoryController _playerManager;
        private static readonly int Interacted = Animator.StringToHash("Interacted");

        [Inject]
        public void Construct(QuestManager questManager, PlayerManager playerManager)
        {
            _playerManager = playerManager.GetComponent<InventoryController>();
        }
        
        private void Start()
        {
            _interactionScript = GetComponentInChildren<Interaction>();
            _animator = GetComponentInChildren<Animator>();
        }

        public void OnInteract()
        {
            _animator.SetBool(Interacted, true);
            if (itemToGive != null) _playerManager.PickUpItem(itemToGive, itemCount);
            _interactionScript.IsInteractable = false;
        }
    }
}
