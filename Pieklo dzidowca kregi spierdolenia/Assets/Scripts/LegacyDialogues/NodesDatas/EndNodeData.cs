using jbzdy.DialogueSystem;
using UnityEngine;

namespace jbzd.LegacyDialogues.NodesDatas
{
    [System.Serializable]
    public class EndNodeData : BaseNodeData
    {
        [field: SerializeField]
        public EndNodeType EndNodeType { get; set; }

        public override void RunNode(DialogueTalk dialogueTalk)
        {
            switch (EndNodeType)
            {
                case EndNodeType.End:
                    dialogueTalk.EndDialogue();
                    break;
                case EndNodeType.Repeat:
                    dialogueTalk.GetNodeByGuid(dialogueTalk.CurrentDialogueNodeData.NodeGuid).RunNode(dialogueTalk);
                    break;
                case EndNodeType.Goback:
                    dialogueTalk.GetNodeByGuid(dialogueTalk.LastDialogueNodeData.NodeGuid).RunNode(dialogueTalk);
                    break;
                case EndNodeType.RetrunToStart:
                    dialogueTalk.GetNextNode(dialogueTalk.CurrentDialogue.GetStartNodeData()).RunNode(dialogueTalk);
                    break;
            }
        }
    }
}
