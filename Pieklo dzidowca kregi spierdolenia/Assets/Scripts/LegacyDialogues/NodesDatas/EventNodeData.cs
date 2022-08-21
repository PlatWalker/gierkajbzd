

using jbzd.Quests;
using jbzdy.DialogueSystem;

namespace jbzd.LegacyDialogues.NodesDatas
{


    [System.Serializable]
    public class EventNodeData : BaseNodeData
    {
        public DialogueEventSO DialogueEventSO;
        public Quest QuestSO;

        public override void RunNode(DialogueTalk dialogueTalk)
        {
            DialogueEventSO.RunEvent(QuestSO);

            dialogueTalk.GetNextNode(this).RunNode(dialogueTalk);
        }
    }
}