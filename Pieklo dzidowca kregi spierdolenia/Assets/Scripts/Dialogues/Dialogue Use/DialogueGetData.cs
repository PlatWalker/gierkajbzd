using UnityEngine;
using jbzdy.DialogueSystem.SO;

/// <summary>
/// Napisane przez sharashino
/// 
/// Skrypt pobierający dane z ScriptableObject'a z dialogiem
/// </summary>
public class DialogueGetData : MonoBehaviour
{
    [SerializeField] protected DialogueContainerSO dialogue;

    protected BaseNodeData GetNodeByGuid(string targetNodeGuid)
    {
        return dialogue.AllNodes.Find(node => node.NodeGuid == targetNodeGuid);
    }

    protected BaseNodeData GetNodeByNodePort(DialogueNodePort nodePort)
    {
        return dialogue.AllNodes.Find(node => node.NodeGuid == nodePort.InputGuid);
    }

    protected BaseNodeData GetNextNode(BaseNodeData baseNodeData)
    {
        NodeLinkData nodeLinkData = dialogue.NodeLinkDatas.Find(egde => egde.BaseNodeGuid == baseNodeData.NodeGuid);

        return GetNodeByGuid(nodeLinkData.TargetNodeGuid);
    }

    
}
