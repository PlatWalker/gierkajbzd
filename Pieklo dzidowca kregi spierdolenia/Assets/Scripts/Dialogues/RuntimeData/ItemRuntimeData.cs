using System;
using jbzd.Common.RunnerThing;
using jbzd.Items;
using jbzd.MainHero;
using jbzd.MainHero.PlayerControllers;
using UnityEngine;

namespace jbzd.Dialogues.RuntimeData
{
    public class ItemRuntimeData : NodeRuntimeData
    {
        [field: SerializeField] public ItemSO Item { get; set; }
        [field: SerializeField] public int ItemsNumber { get; set; }
        [field: SerializeField] public bool IsItemAdded { get; set; }

        [RunMethod]
        public void Run(PlayerManager playerManager, DialogueManager dialogueManager)
        {
            var controller = playerManager.GetPlayerController<InventoryController>();

            Func<ItemSO, int, bool> itemAction;

            if (IsItemAdded)
                itemAction = controller.PickUpItem;
            else 
                itemAction = controller.RemoveItem;
            
            
            var choice = 1;
            if(itemAction(Item, ItemsNumber)) choice = 0;
                
            dialogueManager.RunNode(Choices[choice].NextDialogue);
        }
    }
}
