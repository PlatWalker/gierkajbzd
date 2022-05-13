using jbzdy.Actions.Interaction;
using jbzdy.DialogueSystem;
using jbzdy.DialogueSystem.NodeDatas;
using jbzdy.Player;
using System.Collections.Generic;
using UnityEngine;

namespace jbzdy.NPC.Interaction
{
    public class NPCInteraction : Interactable
    {
        [SerializeField]  private DialogueContainerSO NPCDialogue = default;
        [SerializeField] private List<Quest> QuestsToUpdateWhenInteracted = new();
        
        private DialogueTalk dialogueTalk;
        private PlayerController player;

        private void Start()
        {
            dialogueTalk = GetComponent<DialogueTalk>();
            if(!TryGetComponent(out dialogueTalk))
            {
                dialogueTalk = gameObject.AddComponent<DialogueTalk>();
            }
            player = GameManager.Instance.PlayerObject.GetComponent<PlayerController>();
        }

        public override void Interact()
        {
            base.Interact();
            foreach (Quest quest in QuestsToUpdateWhenInteracted)
            {
                if (quest.isActive)
                {
                    quest.UpdateQuest();
                }
            }
            dialogueTalk.StartDialogue(NPCDialogue);
            player.CanPlayerMove = false;
        }

        public override void StopInteract()
        {
            player.CanPlayerMove = true;
        }
    }
}
