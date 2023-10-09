using System.Collections.Generic;
using jbzd.Dialogues;
using jbzd.Dialogues.RuntimeData;
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
        
        [Inject]
        public void Construct(DialogueManager manager)
        {
            _manager = manager;
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
                _manager.StartDialogue(data);
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
        
    }
}
