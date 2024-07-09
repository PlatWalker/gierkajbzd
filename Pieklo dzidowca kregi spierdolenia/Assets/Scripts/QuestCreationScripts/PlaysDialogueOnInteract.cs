using jbzd.Common.Interfaces;
using jbzd.Dialogues.RuntimeData;
using jbzd.UI;
using jbzd.UI.Dialogues;
using UnityEngine;
using Zenject;

namespace jbzd.QuestCreationScripts
{
    public class PlaysDialogueOnInteract: MonoBehaviour, IInteractable
    {
        private DialogueUIController _uiController;
        [SerializeField] private ContainerSO DialogueContainer;
        
        [Inject]
        public void Constructor(UserInterfaceManager uiManager)
        {
            _uiController = uiManager.GetUIController<DialogueUIController>();
        }
        private void Start()
        {
            if (DialogueContainer != null) return;
            
            Debug.LogError($"No dialogue to play in object {name}!");
        }
        public void OnInteract()
        {
            _uiController.StartDialogue(DialogueContainer);
        }
    }
}