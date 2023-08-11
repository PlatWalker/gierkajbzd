using jbzd.Dialogues;
using jbzd.Dialogues.RuntimeData;
using UnityEngine;
using Zenject;

namespace jbzd.QuestCreationScripts
{
    [RequireComponent(typeof(Collider))]
    public class DialogueTriggerOnGroup : MonoBehaviour
    {
        public ContainerSO dialogueToStart;
        public bool wasTriggered;
        [SerializeField] private string GroupToCheck;

        private DialogueManager _dialogueManager;

        [Inject]
        public void Constructor(DialogueManager dialogueManager)
        {
            _dialogueManager = dialogueManager;
        }

        public void OnTriggerEnter(Collider other)
        {
            if(wasTriggered && dialogueToStart.CurrentGroup == GroupToCheck)
                wasTriggered = false;

            if (wasTriggered) return;

            wasTriggered = true;
            _dialogueManager.StartDialogue(dialogueToStart);

        }
    }
}
