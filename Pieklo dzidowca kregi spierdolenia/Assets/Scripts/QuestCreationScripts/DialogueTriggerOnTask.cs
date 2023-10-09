using jbzd.Common;
using jbzd.Dialogues;
using jbzd.Dialogues.RuntimeData;
using jbzd.QuestSystem;
using jbzd.QuestSystem.QuestStructureElements;
using UnityEngine;
using Zenject;

namespace jbzd.QuestCreationScripts
{
    public class DialogueTriggerOnTask : MonoBehaviour
    {

        [SerializeField] private ContainerSO dialogueToPlay;
        private DialogueManager _dialogueManager;
        private QuestManager _questManager;
        [SerializeField] private TaskSO taskToCheck;
        [SerializeField][JbzdReadOnly] private bool wasTriggered;

        [Inject]
        public void Construct(DialogueManager dialogue, QuestManager questManager)
        {

            _dialogueManager = dialogue;
            _questManager = questManager;
        }

        private void OnTriggerEnter(Collider other)
        {
            foreach (var quest in _questManager.ActiveQuests)
            {
                if(quest.ActiveTask != taskToCheck || wasTriggered)
                {
                    continue;
                }
                _dialogueManager.StartDialogue(dialogueToPlay);
                wasTriggered = true;
            }
        }
    }
}
