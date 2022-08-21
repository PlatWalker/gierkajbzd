using jbzd.LegacyDialogues.NodesDatas;
using UnityEngine;

namespace jbzdy.DialogueSystem
{

    public class DialogueGetData : MonoBehaviour
    {
        public DialogueContainerSO CurrentDialogue { get; protected set; }

        public BaseNodeData GetNodeByGuid(string targetNodeGuid)
        {
            return CurrentDialogue.NodeDatas.Find(node => node.NodeGuid == targetNodeGuid);
        }

        public BaseNodeData GetNodeByNodePort(DialogueNodePort nodePort)
        {
            return CurrentDialogue.NodeDatas.Find(node => node.NodeGuid == nodePort.InputGuid);
        }

        public BaseNodeData GetNextNode(BaseNodeData baseNodeData)
        {
            NodeLinkData nodeLinkData = CurrentDialogue.NodeLinkDatas.Find(egde => egde.BaseNodeGuid == baseNodeData.NodeGuid);

            return GetNodeByGuid(nodeLinkData.TargetNodeGuid);
        }
    }
}
