using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Wait For Animation To Finish", story: "Wait for [animation] to finish in [animator]", category: "Action/Delay", id: "7b6e81fa4fc2768c0d4ea73601d1ae68")]
public partial class WaitForAnimationToFinishAction : Action
{
    [SerializeReference] public BlackboardVariable<AnimationClip> Animation;
    [SerializeReference] public BlackboardVariable<Animator> Animator;
    protected override Status OnStart()
    {
        if (Animator is null || Animation is null)
        {
            Debug.LogError($"Missing variables at node {nameof(WaitForAnimationToFinishAction)}");
            return Status.Failure;
        }
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Animator.Value.GetCurrentAnimatorClipInfo(0)[0].clip.name == Animation.Value.name
            && Animator.Value.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
        {
            return Status.Success;
        }
        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

