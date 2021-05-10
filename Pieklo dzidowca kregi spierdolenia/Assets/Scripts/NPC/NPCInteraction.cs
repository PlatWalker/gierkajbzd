using jbzdy.Actions.Interaction;
using jbzdy.DialogueSystem.Actions;

namespace jbzdy.NPC.Interaction
{
    public class NPCInteraction : Interactables
    {
        public override void Interact()
        {
            GetComponent<DialogueTalk>().StartDialogue();
        }

        public override void StopInteract()
        {
            GetComponent<DialogueTalk>().EndDialogue();
        }
    }
}
