using System.Collections.Generic;
using jbzdy.Actions.Interaction;
using jbzdy.DialogueSystem.Actions;
using jbzdy.DialogueSystem.SO;
using UnityEngine;

namespace jbzdy.NPC.Interaction
{
    public class NPCInteraction : Interactable
    {
        [SerializeField] private List<DialogueContainerSO> NPCDialogues = new List<DialogueContainerSO>();
        private int interactionCounter = 0;
        
        private DialogueTalk dialogueTalk;

        private new void Awake()
        {
            dialogueTalk = GetComponent<DialogueTalk>();
        }

        public override void Interact()
        {
            dialogueTalk.StartDialogue(NPCDialogues[interactionCounter]);
            interactionCounter++;
            FreezePlayer(true);
        }

        public override void StopInteract()
        {
            Debug.Log("done talking with " + gameObject.name);
            FreezePlayer(false);
        }
        
        private void FreezePlayer(bool freeze)
        {
            {
                //TU WSADZIĆ FUNKCJE DO FREEZOWANIA
            
                //JA TO WIDZE TAK
                //SAMA FUNKCJA NIE POWINNA SIE TU ZNAJDOWAC
                //RACZEJ POWINNO BYC ODWOLANIE DO SKRYPTU Z MOVEMENTEM GRACZA, A TAM WLASCIWY KOD DO FREEZOWANIA
                //ZEBY NIE ROZBIJAC POSZCZEGOLNYCH CZESCI MOVEMENTU NA ROZNE SKRYPTY
            }
        }
    }
}
