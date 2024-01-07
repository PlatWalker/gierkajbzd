using System.Linq;
using jbzd.Common.Interfaces;
using jbzd.Dialogues;
using jbzd.Dialogues.RuntimeData;
using jbzd.QuestSystem;
using jbzd.QuestSystem.QuestStructureElements;
using jbzd.UI;
using UnityEngine;
using Zenject;

namespace jbzd.QuestCreationScripts
{
    //TODO name of class need to be renamed, it plays dialogue on interact and task!
    public class CanPlayDialogueOnInteract : MonoBehaviour, IInteractable
    {
        private DialogueUIController _uiController;
        private QuestManager _questManager;
        [SerializeField] private ContainerSO DialogueContainer;
        [SerializeField] private TaskSO TaskOnWhichToTalk;
        
        [Inject]
        public void Constructor(UserInterfaceManager uiManager, QuestManager quest)
        {
            _uiController = uiManager.GetUIController<DialogueUIController>();
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
            foreach (var quest in _questManager.ActiveQuests.Where(quest => TaskOnWhichToTalk == quest.ActiveTask))
            {
                _uiController.StartDialogue(DialogueContainer);
            }
        }
    }
}
