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
        private NavMeshAgent _agent;
        public GoalSO goalToAct;
        public Quest questToStart;

        private bool _followPlayer;
        
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
        }

        public void OnInteract()
        {
            if (questToStart is not null)
            {
                Debug.Log("Quest Started");
                _questManager.StartQuest(questToStart);
            }
            else switch (goalToAct)
            {
                case TalkWithNpcGoal:
                    Debug.Log("Dialogue opened");
                    _questManager.MakeActorPlay(goalToAct);
                    break;
                case EscortGoal:
                    Debug.Log("Started Following");
                    StartFollow();
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
    }
}