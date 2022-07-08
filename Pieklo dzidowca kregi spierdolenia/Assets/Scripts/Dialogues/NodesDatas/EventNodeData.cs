

using jbzd.Quests;

namespace jbzdy.DialogueSystem.NodeDatas
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