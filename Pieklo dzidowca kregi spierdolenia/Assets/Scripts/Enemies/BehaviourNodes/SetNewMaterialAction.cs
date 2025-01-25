using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Set New Material", story: "Set [ModelMesh] with a [NewMaterial]", category: "Action/Resource", id: "67d2ad43e8aad9286b9ceb035b0b2c85")]
public partial class SetNewMaterialAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> ModelMesh;
    [SerializeReference] public BlackboardVariable<Material> NewMaterial;
    protected override Status OnStart()
    {
        if (ModelMesh == null || NewMaterial == null)
        {
            Debug.LogWarning("Missing Skinned Mesh Renderer or Material");
            return Status.Failure;
        }

        if (!ModelMesh.Value.TryGetComponent<SkinnedMeshRenderer>(out var skinnedMeshRenderer))
        {
            Debug.LogWarning("Missing Skinned Mesh Renderer");
            return Status.Failure;
        }

        skinnedMeshRenderer.material = NewMaterial.Value;
        return Status.Success;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

