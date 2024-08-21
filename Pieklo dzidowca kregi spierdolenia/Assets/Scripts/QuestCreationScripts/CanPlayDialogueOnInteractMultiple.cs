using System.Collections.Generic;
using jbzd.Common.Interfaces;
using jbzd.Common.Random;
using jbzd.Dialogues.RuntimeData;
using jbzd.QuestSystem;
using jbzd.QuestSystem.QuestStructureElements;
using jbzd.UI;
using jbzd.UI.Dialogues;
using UnityEngine;
using Zenject;

namespace jbzd.QuestCreationScripts
{
    public class CanPlayDialogueOnInteractMultiple : MonoBehaviour, IInteractable
    {
        private DialogueUIController _uiController;
        private QuestManager _questManager;
        [SerializeField] private string InitiateText;
        [SerializeField] private Sprite PlayerImage;
        [SerializeField] private Sprite NpcImage;
        [SerializeField] private NewDict<TaskSO, ContainerSO, string> TasksAndDialogues;
        Dictionary<TaskSO, (ContainerSO, string)> TasksAndDialoguesDict;

        [Inject]
        public void Constructor(UserInterfaceManager uiManager, QuestManager quest)
        {
            _uiController = uiManager.GetUIController<DialogueUIController>();
            _questManager = quest;
        }

        private void Start()
        {
            TasksAndDialoguesDict = TasksAndDialogues.ToBiggerDictionary();
        }

        public void OnInteract()
        {
            var availableDialogues = new List<(ContainerSO, string)>();
            foreach (var quest in _questManager.ActiveQuests)
            {
                if (TasksAndDialoguesDict.ContainsKey(quest.ActiveTask))
                {
                    availableDialogues.Add(TasksAndDialoguesDict[quest.ActiveTask]);
                }
            }

            _uiController.StartDialogue(availableDialogues, InitiateText, NpcImage, PlayerImage);
        }
    }
}
