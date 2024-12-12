using System.Collections;
using System.Collections.Generic;
using System.Linq;
using jbzd.Common.Interfaces;
using jbzd.Dialogues.RuntimeData;
using jbzd.QuestSystem;
using jbzd.QuestSystem.QuestStructureElements;
using jbzd.UI;
using jbzd.UI.Dialogues;
using UnityEngine;
using Zenject;

namespace jbzd
{
    public class PlaysDialogueOnInteractOnTasks: MonoBehaviour, IInteractable
    {
        private DialogueUIController _uiController;
        private QuestManager _questManager;
        [SerializeField] private ContainerSO DialogueContainer;
        [SerializeField] private List<TaskSO> TasksOnWhichToTalk =new();
        
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
            if (TasksOnWhichToTalk.Count == 0)
                Debug.LogWarning($"No task assigned to check in object {name}!");
        }
        public void OnInteract()
        {
            foreach (var _ in _questManager.ActiveQuests.Where(quest => TasksOnWhichToTalk.Any(x=>quest.ActiveTask == x)))
            {
                _uiController.StartDialogue(DialogueContainer);
            }
        }
    }
}
