using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private NewDict<QuestSO, ContainerSO, string> CompletedQuestsAndDialogues;
        Dictionary<TaskSO, (ContainerSO, string)> TasksAndDialoguesDict;
        Dictionary<QuestSO, (ContainerSO, string)> CompletedQuestsAndDialoguesDict;

        [Inject]
        public void Constructor(UserInterfaceManager uiManager, QuestManager quest)
        {
            _uiController = uiManager.GetUIController<DialogueUIController>();
            _questManager = quest;
        }

        private void Start()
        {
            CompletedQuestsAndDialoguesDict = CompletedQuestsAndDialogues.ToBiggerDictionary();
            TasksAndDialoguesDict = TasksAndDialogues.ToBiggerDictionary();
        }

        public void OnInteract()
        {
            var availableDialogues = new List<(ContainerSO, string)>();
            foreach (var quest in _questManager.ActiveQuests)
            {
                if (TasksAndDialoguesDict.TryGetValue(quest.ActiveTask, out var value))
                {
                    availableDialogues.Add(value);
                }
            }
            
            foreach (var quest in _questManager.AllQuestsFromLoadedMaps
                         .Where(quest => quest.IsCompleted && CompletedQuestsAndDialoguesDict.ContainsKey(quest.QuestData)))
            {
                if (CompletedQuestsAndDialoguesDict.TryGetValue(quest.QuestData, out var value))
                {
                    availableDialogues.Add(value);
                }
            }

            _uiController.StartDialogue(availableDialogues, InitiateText, NpcImage, PlayerImage);
        }
    }
}
