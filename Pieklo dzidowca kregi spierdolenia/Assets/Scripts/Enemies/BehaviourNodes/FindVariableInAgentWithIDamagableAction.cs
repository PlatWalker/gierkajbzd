using System;
using jbzd.Common.Interfaces;
using jbzd.Enemies.Level1.Konkubent;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Find Variable in Agent with IDamagable", story: "Find [Integer] as [Value] in [Agent] with IDamagable", category: "Action/Find", id: "458941a92aa7b60e8a2721138c54fa73")]
public partial class FindVariableInAgentWithIDamagableAction : Action
{
    [SerializeReference] public BlackboardVariable<int> Integer;
    [SerializeReference] public BlackboardVariable<IDamagableVariables> Value;
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    protected override Status OnStart()
    {
        if (Agent == null)
        {
            Debug.LogError($"Missing variable {nameof(Agent)} in node {nameof(FindVariableInAgentWithIDamagableAction)}");
            return Status.Failure;
        }

        if (!Agent.Value.TryGetComponent<IDamageable>(out var damageable))
        {
            Debug.LogError($"Missing {nameof(IDamageable)} interface on {nameof(Agent)}");
            return Status.Failure;
        }

        Integer.Value = Value.Value switch
        {
            IDamagableVariables.MaximumHealth => damageable.MaximumHealth,
            IDamagableVariables.CurrentHealth => damageable.CurrentHealth,
            _ => throw new ArgumentOutOfRangeException()
        };

        return Status.Success;
    }
}

