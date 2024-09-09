using jbzd.Dialogues.RuntimeData;
using jbzd.UI;
using jbzd.UI.Dialogues;
using MyBox;
using UnityEngine;
using Zenject;

namespace jbzd.QuestCreationScripts
{
    [RequireLayer("Player Triggers")]
    [RequireComponent(typeof(Collider))]
    public class PlaysDialogueOnCollision: MonoBehaviour
    {
        private DialogueUIController _uiController;
        
        [SerializeField] private ContainerSO dialogueContainer;
        
        [Inject]
        public void Constructor(UserInterfaceManager uiManager)
        {
            _uiController = uiManager.GetUIController<DialogueUIController>();
        }
        
        private void Start()
        {
            Debug.Assert(dialogueContainer,$"No dialogue to play in object {name}!");
        }

        public void OnTriggerEnter(Collider other)
        {
            _uiController.StartDialogue(dialogueContainer);
        }
    }
}