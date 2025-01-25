using System;
using Unity.Behavior;
using UnityEngine;
using Composite = Unity.Behavior.Composite;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Run in Order", story: "Execute childrens until all complets", category: "Flow", id: "219735266678e48e19a298032e3d2b38")]
public partial class RunInOrderSequence : Composite
{
    [CreateProperty] private int _mCurrentChild;

    protected override Status OnStart()
    {
        _mCurrentChild = 0;
        if (Children.Count == 0)
            return Status.Success;

        var status = StartNode(Children[_mCurrentChild]);
        return status switch
        {
            Status.Success => Status.Running,
            Status.Failure => Status.Running,
            _ => Status.Waiting
        };
    }

    protected override Status OnUpdate()
    {
        if (++_mCurrentChild >= Children.Count)
            return Status.Success;
        
        var status = StartNode(Children[_mCurrentChild]);
        return status switch
        {
            Status.Success => Status.Running,
            Status.Failure => Status.Running,
            _ => Status.Waiting
        };
    }
}

