using System;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.VFX;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Stop Visual Effect", story: "Stop [VisualEffect]", category: "Action/Resource", id: "fe6b9acb90133745edf4939bdd50974b")]
public partial class StopVisualEffectAction : Action
{
    [SerializeReference] public BlackboardVariable<VisualEffect> VisualEffect;

    protected override Status OnStart()
    {
        if (VisualEffect == null)
        {
            Debug.LogError($"Variable {nameof(VisualEffect)} is not set");
            return Status.Failure;
        }
        
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        VisualEffect.Value.Stop();
        return Status.Success;
    }
}

