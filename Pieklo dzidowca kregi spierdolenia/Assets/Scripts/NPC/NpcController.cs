using System;
using System.Collections.Generic;
using System.Linq;
using jbzd.Common;
using jbzd.LegacyDialogues.NodesDatas;
using jbzd.Quests;
using jbzdy.Actions.Interaction;
using jbzdy.DialogueSystem;
//TODO trzeba pozbyc sie referencji do namespace'a gracz. skrpyt npc nie powinien sie tym zajmowac
using jbzdy.Player;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace jbzd.NPC
{
    public struct NpcStringAnimatorParameters
    {
        public static string Walk => "isWalking";
    }
    
    public class NpcController : Interactable
    {
        [SerializeField] private float followDistance = 2;
        [SerializeField] private DialogueContainerSO NPCDialogue;
        [SerializeField] private List<Quest> questsToUpdateWhenInteracted = new();
        [JbzdReadOnly][SerializeField]private bool followPlayer;
        public bool FollowPlayer
        {
            get => followPlayer;
            set
            {
                if (!value) _animator.SetBool(NpcStringAnimatorParameters.Walk, false);

                followPlayer = value;
            }
        }

        private DialogueTalk _dialogueTalk;
        private PlayerController _player;
        private NavMeshAgent _navMeshAgent;
        private Animator _animator;
        private void Start()
        {
            _animator = GetComponentInChildren<Animator>();
            
            if (!TryGetComponent(out _dialogueTalk) ||
                !TryGetComponent(out _navMeshAgent) ||
                _animator == null)
            {
                Debug.LogWarning("Nie znaleziono potrzebnego komponentu!");
                return;
            }
            
            _player = GameManager.Instance.PlayerObject.GetComponent<PlayerController>();

            SceneManager.sceneUnloaded += OnSceneUnloaded;
            
            AnimatorParametersCheck();
            
        }

        private void OnSceneUnloaded(Scene scene)
        {
            if (followPlayer) gameObject.transform.position = _player.transform.position;
        }

        private void AnimatorParametersCheck()
        {
            if(_animator.parameters.Any(x => x.name == NpcStringAnimatorParameters.Walk) == false) 
                Debug.Log("Blad w nazwie parametru chodzenia");
        }

        public override void Interact()
        {
            base.Interact();
            foreach (var quest in questsToUpdateWhenInteracted.Where(quest => quest.IsActive))
            {
                quest.UpdateQuest();
            }
            _dialogueTalk.StartDialogue(NPCDialogue);
            _player.IsUiTurnOn = true;
        }

        public override void StopInteract()
        {
            _player.IsUiTurnOn = false;
        }

        public override void Update()
        {
            base.Update();
            if (FollowPlayer)
            {
                FollowingPlayer();

                _animator.SetBool(NpcStringAnimatorParameters.Walk, !IsDestinationReached());
            }
        }

        public void MoveTo(Vector3 moveTo) => _navMeshAgent.destination = moveTo;

        private void FollowingPlayer()
        {
            var playerPosition = _player.gameObject.transform.position;
            _navMeshAgent.SetDestination
            (
                playerPosition - followDistance * (playerPosition - transform.position).normalized
            );
            
        }
        
        private bool IsDestinationReached()
        {
            if (_navMeshAgent.pathPending) return false;
	        
            if (!(_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)) return false;
	        
            return !_navMeshAgent.hasPath || _navMeshAgent.velocity.sqrMagnitude == 0f;
        }
    }
}
