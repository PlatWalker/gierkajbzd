using jbzd.Common;
using jbzd.Dialogues;
using jbzd.Dialogues.RuntimeData;
using jbzd.QuestSystem;
using jbzd.QuestSystem.QuestStructureElements;
using jbzd.UI;
using jbzd.UI.Dialogues;
using UnityEngine;
using Zenject;

namespace jbzd.QuestCreationScripts
{
    public class DialogueTriggerOnTask : MonoBehaviour
    {

        [SerializeField] private ContainerSO dialogueToPlay;
        private DialogueUIController _dialogueController;
        private QuestManager _questManager;
        [SerializeField] private TaskSO taskToCheck;
        [SerializeField][JbzdReadOnly] private bool wasTriggered;

        [Inject]
        public void Construct(UserInterfaceManager uiManager, QuestManager questManager)
        {

            _dialogueController = uiManager.GetUIController<DialogueUIController>();
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
                _dialogueController.StartDialogue(dialogueToPlay);
                wasTriggered = true;
            }
        }
    }
}
