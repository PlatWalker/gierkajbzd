using UnityEngine;

namespace jbzdy.DialogueSystem.NodeDatas
{
    [System.Serializable]
    public class StatCheckNodeData : BaseNodeData
    {
        [field: SerializeField]
        public StatCheckType StatCheckType { get; set; }
        [field: SerializeField]
        public int StatCheckValue { get; set; }

        public override void RunNode(DialogueTalk dialogueTalk)
        {
            dialogueTalk.StatCheckNodeDatas.Add(this);

            dialogueTalk.GetNextNode(this).RunNode(dialogueTalk);
        }
    }
}
