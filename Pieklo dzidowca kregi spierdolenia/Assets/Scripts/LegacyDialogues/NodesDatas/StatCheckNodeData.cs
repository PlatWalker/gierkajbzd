using jbzdy.DialogueSystem;
using UnityEngine;

namespace jbzd.LegacyDialogues.NodesDatas
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
