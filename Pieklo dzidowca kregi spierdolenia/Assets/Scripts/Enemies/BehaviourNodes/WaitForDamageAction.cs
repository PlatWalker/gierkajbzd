using System;
using jbzd.Common.Interfaces;
using jbzd.Enemies.EnemiesComponents;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(
    name: "Wait For Damage",
    description: "Waits till agent gets damage, only works if IDamageable is implemented in agent.",
    story: "Wait for [Agent] getting damaged",
    category: "Action/Delay",
    id: "c33c5d87cd4933659ef493f9d5259c87")]
public partial class WaitForDamageAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    private IDamageable _damageableAgentComponent;
    private int _lastValueOfCurrentHealth;
    
    protected override Status OnStart()
    {
        if (Agent == null)
        {
            Debug.LogError($"Missing variable {nameof(Agent)} in action {nameof(WaitForDamageAction)}");
            return Status.Failure;
        }

        if (!Agent.Value.TryGetComponent(out _damageableAgentComponent))
        {
            Debug.LogError($"Missing component of type {nameof(CanBeDamaged)} in {nameof(Agent)}");
            return Status.Failure;
        }

        _lastValueOfCurrentHealth = _damageableAgentComponent.CurrentHealth;
        
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (_lastValueOfCurrentHealth == _damageableAgentComponent.CurrentHealth) return Status.Running;
        
        _lastValueOfCurrentHealth = _damageableAgentComponent.CurrentHealth;
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

