using System;
using System.Collections;
using jbzd.Dialogues;
using jbzd.MainHero;
using jbzd.Cutscenes;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Zenject;

namespace jbzd.Cutscenes
{
    [RequireComponent(typeof(PlayableDirector))]
    public class CutsceneDirector : MonoBehaviour
    {
        private CutscenesManager _cutsceneManager;
        private PlayerManager _playerManager;
        private DialogueManager _dialogueManager;
        
        private PlayableDirector _playableDirector;

        [Inject]
        public void Constructor(
            CutscenesManager cutscenesManager,
            PlayerManager playerManager,
            DialogueManager dialogueManager)
        {
            _cutsceneManager = cutscenesManager;
            _playerManager = playerManager;
            _dialogueManager = dialogueManager;
        }

        public void Awake()
        {
            _playableDirector = GetComponent<PlayableDirector>();
            _cutsceneManager.OnCutscenePlayDemand += timelineAsset =>
            {
                if (timelineAsset == _playableDirector.playableAsset)
                {
                    Debug.Log($"Cutscene {transform.parent.name} starts playing");
                    _playableDirector.Play();
                    _cutsceneManager.StartCutscene();
                }
            };
            
            _playableDirector.stopped += _ =>
            {
                _playerManager.CanPlayerMove = true;
                Debug.Log($"{transform.parent.name} unfreeze player");
                _dialogueEnded = false;
                _cutsceneManager.EndCutscene();

            };
            
            _playableDirector.played += _ =>
            {
                if (_playableDirector.time > _playableDirector.duration - 0.5 ) return;

                if (_dialogueStarted)
                {
                    StartCoroutine(WaitForDialogue());
                }
                else
                {
                    Debug.Log($"{transform.parent.name} freeze player");
                    _playerManager.CanPlayerMove = false;
                    _dialogueEnded = false;
                }
            };
            
            _dialogueManager.OnDialogueEnded += () => _dialogueEnded = true;
            _dialogueManager.OnDialogueStarted += () => _dialogueStarted = true;
        }

        //this variables helps to control order of freezing and unfreezing player. Cutscene should freeze player,
        //after dialogue ends. If it would be reversed than on ended dialogue it would unfreeze player and cutscene
        // wont freeze it because it done it BEFORE ended  dialogue.
        private bool _dialogueEnded;
        private bool _dialogueStarted;

        private IEnumerator WaitForDialogue()
        {
            yield return new WaitUntil(() => _dialogueEnded);

            Debug.Log($"{transform.parent.name} freeze player");
            _playerManager.CanPlayerMove = false;
            _dialogueEnded = false;
        }
        
    }
}