using jbzd.Common.Interfaces;
using jbzd.InteractSystem;
using jbzd.Items;
using jbzd.MainHero;
using jbzd.MainHero.PlayerControllers;
using jbzd.QuestSystem;
using jbzd.QuestSystem.QuestStructureElements;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace jbzd.MapInteractionZones
{
    public class PlayAnimationGiveItem : MonoBehaviour, IInteractable
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Interaction _interactionScript;
        [SerializeField] private GoalSO goalToAct;
        [SerializeField] private ItemSO itemToGive;
        [SerializeField] private int itemCount;
        private QuestManager _questManager;
        private InventoryController _playerManager;

        [Inject]
        public void Construct(QuestManager questManager, PlayerManager playerManager)
        {
            _questManager = questManager;
            _playerManager = playerManager.GetComponent<InventoryController>();
        }

        public void OnInteract()
        {
            if (goalToAct != null) _questManager.MakeActorPlay(goalToAct);
            if (itemToGive != null) _playerManager.PickUpItem(itemToGive, itemCount);
            _animator.SetBool("Interacted", true);
            Destroy(_interactionScript.gameObject);
        }
    }
}
