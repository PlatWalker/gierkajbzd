using jbzdy.Actions.Interaction;
using jbzdy.DialogueSystem.Actions;
using UnityEngine;

namespace jbzdy.NPC.Interaction
{
    public class NPCInteraction : Interactable
    {
        private DialogueTalk dialogueTalk;

        private new void Awake()
        {
            dialogueTalk = GetComponent<DialogueTalk>();
        }

        public override void Interact()
        {
            dialogueTalk.StartDialogue();
        }

        public override void StopInteract()
        {
            dialogueTalk.EndDialogue();
        }
    }
}
