using jbzd.Dialogues;
using jbzd.Dialogues.RuntimeData;
using UnityEngine;
using Zenject;

namespace jbzd.Scenes.Triggers
{
    [RequireComponent(typeof(Collider))]
    public class DialogueTrigger : MonoBehaviour
    {
        public ContainerSO dialogueToStart;
        public bool wasTriggered;
        
        private DialogueManager _dialogueManager;
        
        [Inject]
        public void Constructor(DialogueManager dialogueManager)
        {
            _dialogueManager = dialogueManager;
        }
        
        public void OnTriggerEnter(Collider other)
        {
            if (wasTriggered) return;
            
            wasTriggered = true;
            _dialogueManager.StartDialogue(dialogueToStart);

        }
    }
}
