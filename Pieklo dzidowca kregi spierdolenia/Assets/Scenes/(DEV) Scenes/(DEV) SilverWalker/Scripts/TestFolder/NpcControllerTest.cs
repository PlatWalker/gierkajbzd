using System.Linq;
using jbzd.Common.Interfaces;
using jbzd.MainHero;
using jbzd.QuestSystem.Goals;
using jbzd.QuestSystem.QuestStructureElements;
using UnityEngine;
using UnityEngine.AI;
using Zenject;
using EscortGoal = jbzd.QuestSystem.Goals.EscortGoal;

namespace jbzd.QuestSystem.TestFolder
{
    public class NpcControllerTest : MonoBehaviour, IInteractable
    {
        private QuestManager _questManager;
        private PlayerManager _playerManager;
        [HideInInspector]
        public NavMeshAgent _agent;
        public GoalSO goalToAct;
        public Quest questToStart;

        public bool _followPlayer;
        public bool _runAwayFromPlayer;
        public Transform[] waypoints;
        private int _currentWaypointIndex;

        public void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        [Inject]
        public void Constructor(QuestManager questManager, PlayerManager playerManager)
        {
            _questManager = questManager;
            _playerManager = playerManager;
        }

        public void Update()
        {
            if(_followPlayer) _agent.SetDestination(_playerManager.transform.position - Vector3.one);
            if (_runAwayFromPlayer) RunToWaypoints();
        }

        public void OnInteract()
        {
            Debug.LogError("test");
            
            if (questToStart is not null)
            {
                //Debug.Log("Quest Started");
                _questManager.StartQuest(questToStart);
            }
            
            switch (goalToAct)
            {
                case TalkWithNpcGoal:
                    //Debug.Log("Dialogue opened");
                    _questManager.MakeActorPlay(goalToAct);
                    break;
                case EscortGoal:
                    //Debug.Log("Started Following");
                    StartFollow();
                    break;
                case ChaseGoal:
                    _questManager.MakeActorPlay(goalToAct);
                    break;
                case KillEnemiesGoal:
                    _questManager.MakeActorPlay(goalToAct);
                    break;
            }
        }

        public void StartFollow()
        {
            _followPlayer = true;
        }
        
        public void StopFollow()
        {
            _followPlayer = false;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.TryGetComponent<Actor>(out var otherActor)) return;

            if (goalToAct.ActorsData.Any(actor => actor == otherActor.ActorData))
            {
                _questManager.MakeActorPlay(goalToAct);
            }
        }

        public void StartDialogue()
        {
            Debug.Log("Quest dialogue Started");
        }
        
        private void RunToWaypoints()
        {
            if (!(_agent.remainingDistance < 0.01f)) return;
            if (_currentWaypointIndex > waypoints.Length - 1) return;

            _agent.SetDestination(waypoints[_currentWaypointIndex].position);
            _currentWaypointIndex += 1;
        }
    }
}