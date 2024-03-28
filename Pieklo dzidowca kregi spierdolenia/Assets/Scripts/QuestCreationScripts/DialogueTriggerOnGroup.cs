using jbzd.Dialogues;
using jbzd.Dialogues.RuntimeData;
using jbzd.UI;
using jbzd.UI.Dialogues;
using UnityEngine;
using Zenject;

namespace jbzd.QuestCreationScripts
{
    [RequireComponent(typeof(Collider))]
    public class DialogueTriggerOnGroup : MonoBehaviour
    {
        public ContainerSO dialogueToStart;
        [SerializeField] private string GroupToCheck;

        private DialogueUIController _dialogueController;

        [Inject]
        public void Constructor(UserInterfaceManager uiManager)
        {
            _dialogueController = uiManager.GetUIController<DialogueUIController>();
        }

        public void OnTriggerEnter(Collider other)
        {
            if (dialogueToStart.CurrentGroup != GroupToCheck) return;

            _dialogueController.StartDialogue(dialogueToStart);

        }
    }
}
