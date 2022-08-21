using jbzdy.DialogueSystem;

namespace jbzd.LegacyDialogues.NodesDatas
{

    [System.Serializable]
    public class StartNodeData : BaseNodeData
    {
        public override void RunNode(DialogueTalk dialogueTalk)
        {
            dialogueTalk.RunNode(dialogueTalk.GetNextNode(dialogueTalk.CurrentDialogue.GetStartNodeData()));
        }
    }
}