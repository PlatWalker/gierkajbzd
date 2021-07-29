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
        private int interactionCounter = 0;
        
        private DialogueTalk dialogueTalk;
        private PlayerController player;

        private new void Awake()
        {
            dialogueTalk = GetComponent<DialogueTalk>();
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }

        public override void Interact()
        {
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
