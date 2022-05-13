using jbzdy.Inventory;
using jbzdy.Items;
using UnityEngine;

namespace jbzdy.DialogueSystem.NodeDatas
{
    [System.Serializable]
    public class ItemCheckNodeData : BaseNodeData
    {
        [field: SerializeField]
        public string PositiveResultGuid { get; set; }
        [field: SerializeField]
        public string NegativeResultGuid { get; set; }
        [field: SerializeField]
        public int ItemCheckValue { get; set; }
        [field: SerializeField]
        public Item NodeItem { get; set; }

        public override void RunNode(DialogueTalk dialogueTalk)
        {
            if (InventoryClass.Instance.CheckItemAmount(NodeItem) >= ItemCheckValue)
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
