using jbzd.Common;
using jbzd.Dialogues;
using jbzd.Dialogues.RuntimeData;
using jbzd.QuestSystem.QuestStructureElements;
using UnityEngine;
using Zenject;

namespace jbzd.QuestCreationScripts
{
    public class DialogueTriggerOnTask : MonoBehaviour
    {

        [SerializeField] private ContainerSO dialogueToPlay;
        private DialogueManager _dialogueManager;
        [SerializeField] private Quest questToCheck;
        [SerializeField] private TaskSO taskToCheck;
        [SerializeField][JbzdReadOnly] private bool wasTriggered;

        [Inject]
        public void Construct(DialogueManager dialogue)
        {
            _dialogueManager = dialogue;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (questToCheck.ActiveTask == taskToCheck && !wasTriggered)
            {
                _dialogueManager.StartDialogue(dialogueToPlay);
                wasTriggered = true;
            }   
        }
    }
}
