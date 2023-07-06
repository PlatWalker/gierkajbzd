using jbzd.Common.Interfaces;
using jbzd.Dialogues.RuntimeData;
using jbzd.Dialogues;
using UnityEngine;
using Zenject;
using jbzd.QuestSystem;
using jbzd.QuestSystem.QuestStructureElements;

namespace jbzd
{
    public class DialogueInteraction : MonoBehaviour, IInteractable
    {
        private DialogueManager _manager;
        private QuestManager _questManager;
        [field: SerializeField] private ContainerSO data;
        public GoalSO goalToAct;

        [Inject]
        public void Construct(DialogueManager manager, QuestManager questManager)
        {
            _manager = manager;
            _questManager = questManager;
        }

        public void OnInteract()
        {
            if (data != null)
            {
                _manager.StartDialogue(data);
            }
        }
    }
}
