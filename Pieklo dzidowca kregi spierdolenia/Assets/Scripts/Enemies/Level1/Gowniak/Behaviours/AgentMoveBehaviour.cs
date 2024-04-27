using System;
using System.Linq;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using jbzd.Enemies.Level1.Gowniak.Config;
using jbzd.Enemies.Level1.Gowniak.Targets;
using jbzd.MainHero;

using UnityEngine;
using UnityEngine.AI;

namespace jbzd.Enemies.Level1.Gowniak.Behaviors
{
    [RequireComponent(typeof(NavMeshAgent), typeof(Animator), typeof(AgentBehaviour))]
    public class AgentMoveBehaviour : MonoBehaviour
    {

        public AttackConfigSO AttackConfig;
        private NavMeshAgent NavMeshAgent;
        private Animator Animator;
        private AgentBehaviour AgentBehavior;
        private ITarget CurrentTarget;
        [SerializeField] private float MinMoveDistance = 0.25f;
        private bool shouldMove = true;

        private Vector3 LastPosition;
        private static readonly int RUN = Animator.StringToHash("Move");
        private static readonly int IDLE = Animator.StringToHash("Idle");


        private void Awake()
        {
            NavMeshAgent = GetComponent<NavMeshAgent>();
            Animator = GetComponent<Animator>();
            AgentBehavior = GetComponent<AgentBehaviour>();
            if(Animator.parameters.Any(x => x.nameHash == RUN) == false) 
                Debug.LogError("Blad w nazwie parametru biegania");
            if (Animator.parameters.Any(x => x.nameHash == IDLE) == false)
                Debug.LogError("Blad w nazwie parametru  IDLE");
        }

        private void OnEnable()
        {
            AgentBehavior.Events.OnTargetChanged += EventsOnTargetChanged;
            this.AgentBehavior.Events.OnTargetInRange += this.OnTargetInRange;
            this.AgentBehavior.Events.OnTargetChanged += this.OnTargetChanged;
            this.AgentBehavior.Events.OnTargetOutOfRange += this.OnTargetOutOfRange;
        }

        private void OnDisable()
        {
            AgentBehavior.Events.OnTargetChanged -= EventsOnTargetChanged;
            this.AgentBehavior.Events.OnTargetInRange -= this.OnTargetInRange;
            this.AgentBehavior.Events.OnTargetChanged -= this.OnTargetChanged;
            this.AgentBehavior.Events.OnTargetOutOfRange -= this.OnTargetOutOfRange;
        }

        private void EventsOnTargetChanged(ITarget target, bool inRange)
        {
            CurrentTarget = target;
            LastPosition = CurrentTarget.Position;
            NavMeshAgent.SetDestination(target.Position);
            Animator.SetBool(RUN, true);
        }

        private void OnTargetInRange(ITarget target)
        {
            //this.shouldMove = false;
            this.CurrentTarget = target;
        }

        private void OnTargetChanged(ITarget target, bool inRange)
        {
            this.CurrentTarget = target;
        }

        private void OnTargetOutOfRange(ITarget target)
        {
            //this.shouldMove = true;
        }

        private void Update()
        {
            //distance between player and enemy
            if(CurrentTarget != null && Vector3.Distance(CurrentTarget.Position, transform.position) < AttackConfig.MeleeAttackRadius)
            {
                shouldMove = false;
            }
            else
            {
                shouldMove = true;
            }


            if(!shouldMove){
                NavMeshAgent.SetDestination(transform.position);
                return;
            }   
            
            if (CurrentTarget == null)
            {
                return;
            }
            


            if (MinMoveDistance <= Vector3.Distance(CurrentTarget.Position, LastPosition))
            {
                LastPosition = CurrentTarget.Position;
                NavMeshAgent.SetDestination(CurrentTarget.Position);    
            }
            
            Animator.SetBool(RUN, NavMeshAgent.velocity.magnitude > 0.1f);
        }
    }
}