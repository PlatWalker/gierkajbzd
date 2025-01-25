using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Run Away From", story: "[Agent] run away from [Target] on [Distance]", category: "Action/Navigation", id: "2b53165f81929c7dbf8b010d55294b48")]
public partial class RunAwayFromAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<float> Distance;
    
    [SerializeReference] public BlackboardVariable<float> Speed = new(1.0f);
    
    private NavMeshAgent _navMeshAgent;

    protected override Status OnStart()
    {
        if (Agent.Value == null || Target.Value == null)
        {
            return Status.Failure;
        }

        _navMeshAgent = Agent.Value.GetComponent<NavMeshAgent>();
        
        _navMeshAgent.SetDestination(GetPointAwayFromPlayer());
        _navMeshAgent.speed = Speed;
        
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Vector3.Distance(Agent.Value.transform.position, Target.Value.transform.position) > Distance)
        {
            return Status.Success;
        }
        
        _navMeshAgent.SetDestination(GetPointAwayFromPlayer());
        
        return Status.Running;
    }

    protected override void OnEnd()
    {
        _navMeshAgent.ResetPath();
    }
    
    private Vector3 GetPointAwayFromPlayer()
    {
        // Direction away from the player
        var directionAway = (Agent.Value.transform.position - Target.Value.transform.position).normalized;

        // Target position in the opposite direction
        var targetPosition = Agent.Value.transform.position + directionAway * Distance.Value;

        // Check if the target position is valid on the NavMesh
        if (NavMesh.SamplePosition(targetPosition, out var hit, 1f, NavMesh.AllAreas))
        {
            return hit.position; // Valid NavMesh point
        }

        // If no valid position, return the current position to prevent errors
        return Agent.Value.transform.position;
    }
}

