using jbzd.Dialogues;
using jbzd.Dialogues.RuntimeData;
using jbzd.UI;
using jbzd.UI.Dialogues;
using UnityEngine;
using UnityEngine.Playables;
using Zenject;

namespace jbzd.QuestCreationScripts
{
    public class TimelineDialogue : MonoBehaviour
    {
        [SerializeField] private PlayableDirector _cutscene;
        [SerializeField] private ContainerSO data;
        
        private DialogueManager _manager;
        private DialogueUIController _controller;
        
        [Inject]
        public void Construct(DialogueManager manager, UserInterfaceManager uiManager)
        {
            _manager = manager;
            _controller = uiManager.GetUIController<DialogueUIController>();
        }
        private void Start()
        {
            _cutscene = GetComponent<PlayableDirector>();
            _manager.OnDialogueEnded += EndPause;
            _manager.OnDialogueStarted += StartPause;
        }
        public void FireDialogue()
        {
            if (data != null)
            {
                _controller.StartDialogue(data);
            }
            
        }
        private void StartPause()
        {
            _cutscene.Pause();
        }
        private void EndPause()
        {
            _cutscene.Resume();
        }

        private void OnDisable()
        {
            _manager.OnDialogueStarted -= StartPause;
            _manager.OnDialogueEnded -= EndPause;
        }
    }
}
