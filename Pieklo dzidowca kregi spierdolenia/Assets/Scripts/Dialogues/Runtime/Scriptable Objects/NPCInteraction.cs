using System.Collections.Generic;
using System.Linq;
using jbzdy.Actions.Interaction;
using jbzdy.DialogueSystem.Actions;
using jbzdy.DialogueSystem.SO;
using jbzdy.Player;
using UnityEngine;

namespace jbzdy.NPC.Interaction
{
    public class NPCInteraction : Interactable
    {
        [SerializeField] private List<DialogueContainerSO> NPCDialogues = new List<DialogueContainerSO>();
        [SerializeField] private List<Quest> NPCQuests = new List<Quest>();
        private int interactionCounter = 0;
        
        private DialogueTalk dialogueTalk;
        private PlayerController player;

        private void Start()
        {
            dialogueTalk = GetComponent<DialogueTalk>();
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }

        public override void Interact()
        {
            base.Interact();
            foreach (Quest quest in NPCQuests)
            {
                if (quest.isActive)
                {
                    quest.UpdateQuest();
                }
            }

            if (interactionCounter >= NPCDialogues.Count)
            {
                dialogueTalk.StartDialogue(NPCDialogues.Last());
            }
            else
            {
                dialogueTalk.StartDialogue(NPCDialogues[interactionCounter]);
                interactionCounter++;
            }
            
            player.CanPlayerMove = false;
        }

        public override void StopInteract()
        {
            Debug.Log("Zakończyłem rozmowę z " + gameObject.name);
            player.CanPlayerMove = true;
        }
    }
}
