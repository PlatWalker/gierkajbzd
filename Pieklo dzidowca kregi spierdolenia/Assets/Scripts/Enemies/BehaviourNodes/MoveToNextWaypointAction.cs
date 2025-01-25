using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Move To Next Waypoint", story: "Move [Agent] to next [Waypoints]", category: "Action/Navigation", id: "3be126f53d99006e7f7768bb6077ca5b")]
public partial class MoveToNextWaypointAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<List<GameObject>> Waypoints;
    
    [SerializeReference] public BlackboardVariable<float> Speed;
    [SerializeReference] public BlackboardVariable<float> DistanceThreshold = new BlackboardVariable<float>(0.2f);
    [SerializeReference] public BlackboardVariable<string> AnimatorSpeedParam = new BlackboardVariable<string>("SpeedMagnitude");

    private UnityEngine.AI.NavMeshAgent m_NavMeshAgent;
    private Animator m_Animator;
    private float m_PreviousStoppingDistance;
    
    [CreateProperty]
    private Vector3 m_CurrentTarget;
    [CreateProperty]
    private int m_CurrentPatrolPoint = 0;

    protected override Status OnStart()
    {
        if (Agent.Value == null)
        {
            LogFailure("No agent assigned.");
            return Status.Failure;
        }

        if (Waypoints.Value == null || Waypoints.Value.Count == 0)
        {
            LogFailure("No waypoints to patrol assigned.");
            return Status.Failure;
        }

        Initialize();

        MoveToNextWaypoint();
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Agent.Value == null || Waypoints.Value == null)
        {
            return Status.Failure;
        }

        var distance = GetDistanceToWaypoint();
            
        // If we are using navmesh, get the animator speed out of the velocity.
        if (m_Animator != null && m_NavMeshAgent != null)
        {
            m_Animator.SetFloat(AnimatorSpeedParam, m_NavMeshAgent.velocity.magnitude);
        }

        if (distance <= DistanceThreshold)
        {
            if (m_Animator != null)
            {
                m_Animator.SetFloat(AnimatorSpeedParam, 0);
            }
            
            return Status.Success;
        }

        return Status.Running;
    }

    protected override void OnEnd()
    {
        if (m_Animator != null)
        {
            m_Animator.SetFloat(AnimatorSpeedParam, 0);
        }

        if (m_NavMeshAgent != null)
        {
            if (m_NavMeshAgent.isOnNavMesh)
            {
                m_NavMeshAgent.ResetPath();
            }
            m_NavMeshAgent.stoppingDistance = m_PreviousStoppingDistance;
        }
    }

    protected override void OnDeserialize()
    {
        Initialize();
    }

    private void Initialize()
    {
        m_Animator = Agent.Value.GetComponentInChildren<Animator>();
        if (m_Animator != null)
        {
            m_Animator.SetFloat(AnimatorSpeedParam, 0);
        }

        m_NavMeshAgent = Agent.Value.GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
        if (m_NavMeshAgent != null)
        {
            if (m_NavMeshAgent.isOnNavMesh)
            {
                m_NavMeshAgent.ResetPath();
            }
            m_NavMeshAgent.speed = Speed.Value;
            m_PreviousStoppingDistance = m_NavMeshAgent.stoppingDistance;
            m_NavMeshAgent.stoppingDistance = DistanceThreshold;
        }
    }

    private float GetDistanceToWaypoint()
    {
        if (m_NavMeshAgent != null)
        {
            return m_NavMeshAgent.remainingDistance;
        }

        Vector3 targetPosition = m_CurrentTarget;
        Vector3 agentPosition = Agent.Value.transform.position;
        agentPosition.y = targetPosition.y; // Ignore y for distance check.
        return Vector3.Distance(
            agentPosition,
            targetPosition
        );
    }

    private void MoveToNextWaypoint()
    {
        m_CurrentPatrolPoint = (m_CurrentPatrolPoint + 1) % Waypoints.Value.Count;            

        m_CurrentTarget = Waypoints.Value[m_CurrentPatrolPoint].transform.position;
        if (m_NavMeshAgent != null)
        {
            m_NavMeshAgent.SetDestination(m_CurrentTarget);
        }
        else if (m_Animator != null)
        {
            // We set the animator speed once if we are using transform.
            m_Animator.SetFloat(AnimatorSpeedParam, Speed.Value);
        }
    }
}

