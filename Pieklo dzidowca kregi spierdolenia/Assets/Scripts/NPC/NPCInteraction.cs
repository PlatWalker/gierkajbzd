using UnityEngine;
using jbzdy.Actions.Interaction;
using jbzdy.DialogueSystem.Actions;

namespace jbzdy.NPC.Interaction
{
    public class NPCInteraction : Interactables
    {
        public override void Interact()
        {
            NPCInterract();
        }

        private void NPCInterract()
        {
            GetComponent<DialogueTalk>().StartDialogue();
        }
    }
}
