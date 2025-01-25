using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Find With Tag On GameObject", story: "Find [Object] with [Tag] in children of [OtherObject]", category: "Action/Find", id: "595744157fc15ae9d6760ad9cf6d85de")]
public partial class FindWithTagOnGameObjectAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Object;
    [SerializeReference] public BlackboardVariable<string> Tag;
    [SerializeReference] public BlackboardVariable<GameObject> OtherObject;
    
    protected override Status OnStart()
    {
        if (Object == null || Tag == null || OtherObject == null)
        {
            return Status.Failure;
        }
        
        var childTransforms = OtherObject.Value.transform.GetComponentsInChildren<Transform>();

        if (childTransforms.Length <= 0) return Status.Failure;
        
        foreach (var childTransform in childTransforms)
        {
            if(childTransform.CompareTag(Tag.Value))
            {
                Object.Value = childTransform.gameObject;
            }
        }
        
        return Object.Value == null ? Status.Failure : Status.Success;
    }
}

