using UnityEngine;
using jbzdy.DialogueSystem.managers;
using jbzdy.DialogueSystem.DataContainers;

namespace jbzdy.NPC.interaction
{
    public class NPCInteraction : Interactables
    {
        [SerializeField] private DialogueContainer NPCDialogue;
        [SerializeField] private DialogueManager dialogueManager;

        public override void Interact()
        {
            base.Interact();
            NPCInterract();
        }

        private void NPCInterract()
        {
            dialogueManager.StartDialogue(NPCDialogue);
        }
    }
}
