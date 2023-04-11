using jbzd.Dialogues;
using jbzd.Dialogues.RuntimeData;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using Zenject;

namespace jbzd
{
    public class TimelineDialogue : MonoBehaviour
    {
        private DialogueManager _manager;
        [SerializeField] private PlayableDirector _cutscene;
        [SerializeField] private ContainerSO data;
        [Inject]
        public void Construct(DialogueManager manager)
        {
            _manager = manager;
        }
        private void Start()
        {
            _cutscene = GetComponent<PlayableDirector>();
            _manager.OnDialogueEnded += endPause;
            _manager.OnDialogueStarted += startPause;
        }
        public void fireDialogue()
        {
            if (data != null)
            {
                _manager.StartDialogue(data);
            }
            
        }
        private void startPause()
        {
            _cutscene.Pause();
        }
        private void endPause()
        {
            _cutscene.Resume();
        }
        
    }
}
