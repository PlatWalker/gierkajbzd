using System;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.VFX;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Play Visual Effect", story: "Play [VisualEffect]", category: "Action/Resource", id: "90423b0ed619d27f20f4947536c9e5ba")]
public partial class PlayVisualEffectAction : Action
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
        VisualEffect.Value.Play();
        return Status.Success;
    }
}

