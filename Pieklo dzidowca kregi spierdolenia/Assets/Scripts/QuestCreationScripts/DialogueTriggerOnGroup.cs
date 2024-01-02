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
        [SerializeField] private string GroupToCheck;

        private DialogueManager _dialogueManager;

        [Inject]
        public void Constructor(DialogueManager dialogueManager)
        {
            _dialogueManager = dialogueManager;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (dialogueToStart.CurrentGroup != GroupToCheck) return;

            _dialogueManager.StartDialogue(dialogueToStart);

        }
    }
}
