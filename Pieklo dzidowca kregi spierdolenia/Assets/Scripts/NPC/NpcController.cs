using System;
using System.Linq;
using jbzd.Common.Interfaces;
using jbzd.Dialogues;
using jbzd.Dialogues.RuntimeData;
using jbzd.MainHero;
using jbzd.QuestSystem;
using jbzd.QuestSystem.QuestStructureElements;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Zenject;

namespace jbzd.NPC
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class NpcController : MonoBehaviour , IInteractable
    {
        public enum NpcStates
        {
            Undefined,
            Idle,
            FollowPlayer,
            RunAwayFromPlayer
        }

        [SerializeField] private NpcStates npcState = NpcStates.Idle;
        
        public NpcStates NpcState
        {
            get => npcState;
            set
            {
                if (npcState is NpcStates.RunAwayFromPlayer && value is not NpcStates.RunAwayFromPlayer)
                {
                    _currentWaypointIndex = 0;
                }

                if (value is NpcStates.FollowPlayer)
                {
                    _playerManager.ListOfFollowers.Add(this);
                }

                if (npcState is NpcStates.FollowPlayer && value is not NpcStates.FollowPlayer)
                {
                    _playerManager.ListOfFollowers.Remove(this);
                }
                
                npcState = value;
            } 
        }

        [field:SerializeField]
        public ContainerSO DialogueContainer { get; set; }
        [field:SerializeField]
        public TaskSO TaskOnWhichToTalk { get; set; }
        
        [field:SerializeField]
        [Tooltip("If Npc need to follow a predefined path in some situation, add waypoints from scene for him to follow" +
                 "them one by one.")]
        public Transform[] Waypoints { get; set; }
        public NavMeshAgent NpcAgent { get; private set; }

        private PlayerManager _playerManager;
        private DialogueManager _dialogueManager;
        private QuestManager _questManager;
        private int _currentWaypointIndex;

        [Inject]
        public void Constructor(PlayerManager playerManager, DialogueManager dialogueManager, QuestManager questManager)
        {
            _playerManager = playerManager;
            _dialogueManager = dialogueManager;
            _questManager = questManager;
        }
        
        public void Awake()
        {
            NpcAgent = GetComponent<NavMeshAgent>();
        }
        
        public void Update()
        {
            switch (NpcState)
            {
                case NpcStates.Idle:
                    break;
                case NpcStates.FollowPlayer:
                    NpcAgent.SetDestination(_playerManager.transform.position - Vector3.one);
                    break;
                case NpcStates.RunAwayFromPlayer:
                    if (!(NpcAgent.remainingDistance < 0.01f)) return;
                    if (_currentWaypointIndex > Waypoints.Length - 1) return;

                    NpcAgent.SetDestination(Waypoints[_currentWaypointIndex].position);
                    _currentWaypointIndex += 1;
                    break;
                case NpcStates.Undefined:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void OnInteract()
        {
            if (DialogueContainer is null) return;
            
            if (_questManager.ActiveQuests.Any(quest => quest.ActiveTask == TaskOnWhichToTalk))
            {
                _dialogueManager.StartDialogue(DialogueContainer);
            }
        }
    }
}
