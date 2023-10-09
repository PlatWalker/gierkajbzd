using jbzd.Common.Interfaces;
using jbzd.Dialogues;
using jbzd.Dialogues.RuntimeData;
using jbzd.QuestSystem;
using jbzd.QuestSystem.QuestStructureElements;
using UnityEngine;
using Zenject;

namespace jbzd.QuestCreationScripts
{
    public class CanPlayDialogueOnInteract : MonoBehaviour, IInteractable
    {
        private DialogueManager _dialogueManager;
        private QuestManager _questManager;
        [SerializeField] private Quest QuestToCheck;
        [SerializeField] private ContainerSO DialogueContainer;
        [SerializeField] private TaskSO TaskOnWhichToTalk;
        
        [Inject]
        public void Contructor(DialogueManager dialogue, QuestManager quest)
        {
            _dialogueManager = dialogue;
            _questManager = quest;
        }
        private void Start()
        {
            if (DialogueContainer == null)
            {
                Debug.LogWarning($"No dialogue to play in object {name}!");
                return;
            }
            if (TaskOnWhichToTalk == null)
                Debug.LogWarning($"No task assigned to check in object {name}!");
        }
        public void OnInteract()
        {

            
            foreach (var quest in _questManager.ActiveQuests)
            {
                if (TaskOnWhichToTalk == quest.ActiveTask)
                    _dialogueManager.StartDialogue(DialogueContainer);
            }
        }
    }
}
