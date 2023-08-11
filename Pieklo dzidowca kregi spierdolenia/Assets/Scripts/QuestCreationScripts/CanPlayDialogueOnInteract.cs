using jbzd.Common.Interfaces;
using jbzd.Dialogues;
using jbzd.Dialogues.RuntimeData;
using jbzd.QuestSystem.QuestStructureElements;
using UnityEngine;
using Zenject;

namespace jbzd.QuestCreationScripts
{
    public class CanPlayDialogueOnInteract : MonoBehaviour, IInteractable
    {
        private DialogueManager _dialogueManager;
        [SerializeField] private Quest QuestToCheck;
        [SerializeField] private ContainerSO DialogueContainer;
        [SerializeField] private TaskSO TaskOnWhichToTalk;
        
        [Inject]
        public void Contructor(DialogueManager dialogue)
        {
            _dialogueManager = dialogue;
        }        

        public void OnInteract()
        {
            
            if (DialogueContainer != null)
            {
                if (TaskOnWhichToTalk != null && TaskOnWhichToTalk == QuestToCheck.ActiveTask)
                {

                    _dialogueManager.StartDialogue(DialogueContainer);

                }
            }
        }
    }
}
