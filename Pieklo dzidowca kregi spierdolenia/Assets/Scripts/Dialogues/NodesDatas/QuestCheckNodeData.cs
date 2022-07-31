using jbzd.Quests;
using UnityEngine;

namespace jbzdy.DialogueSystem.NodeDatas
{
    [System.Serializable]
    public class QuestCheckNodeData : BaseNodeData
    {
        [field: SerializeField]
        public string PositiveResultGuid { get; set; }
        [field: SerializeField]
        public string NegativeResultGuid { get; set; }
        [field: SerializeField]
        public Quest NodeQuest { get; set; }
        [field: SerializeField]
        public QuestGoal NodeTask { get; set; }
        public override void RunNode(DialogueTalk dialogueTalk)
        {
            if (NodeTask.completed)
            {
                dialogueTalk.GetNodeByGuid(PositiveResultGuid).RunNode(dialogueTalk);
            }
            else
            {
                dialogueTalk.GetNodeByGuid(NegativeResultGuid).RunNode(dialogueTalk);
            }
        }
    }
}