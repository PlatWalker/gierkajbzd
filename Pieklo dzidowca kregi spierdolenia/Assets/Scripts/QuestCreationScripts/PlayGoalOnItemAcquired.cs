using jbzd.Items;
using jbzd.MainHero;
using jbzd.MainHero.PlayerControllers;
using jbzd.QuestSystem;
using jbzd.QuestSystem.QuestStructureElements;
using UnityEngine;
using Zenject;

namespace jbzd.QuestCreationScripts
{
    public class PlayGoalOnItemAcquired: MonoBehaviour
    {
        [SerializeField] private ItemSO item;
        [SerializeField] private GoalSO goalToPlay;
        
        private InventoryController _inventoryController;
        private QuestManager _questManager;
        
        [Inject]
        public void Constructor(PlayerManager playerManager, QuestManager questManager)
        {
            _inventoryController = playerManager.GetPlayerController<InventoryController>();
            _questManager = questManager;
        }

        private void Start()
        {
            _inventoryController.OnItemAcquiring += OnItemAcquired;
        }

        private void OnItemAcquired(ItemSO itemAcquired, int numberOfItems)
        {
            if (item == itemAcquired)
            {
                _questManager.MakeActorPlay(goalToPlay);
            }
        }

        private void OnDestroy()
        {
            _inventoryController.OnItemAcquiring -= OnItemAcquired;
        }
    }
}