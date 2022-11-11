using jbzd.Items.Legacy.Inventory.Inventory;
using jbzdy.DialogueSystem;
using jbzdy.Inventory;
using jbzdy.Items;
using UnityEngine;

namespace jbzd.LegacyDialogues.NodesDatas
{
    [System.Serializable]
    public class GiveOrTakeItemNodeData : BaseNodeData
    {
        [field: SerializeField]
        public ItemCheckNodeType ItemCheckType { get; set; }
        [field: SerializeField]
        public int ItemCheckValue { get; set; }
        [field: SerializeField]
        public SharItem NodeItem { get; set; }

        public override void RunNode(DialogueTalk dialogueTalk)
        {
            if (ItemCheckType == ItemCheckNodeType.GiveToPlayer)
            {
                InventoryClass.Instance.AddItem(Object.Instantiate(NodeItem));
            }
            else if (ItemCheckType == ItemCheckNodeType.TakeFromPlayer)
            {
                InventoryClass.Instance.TakeItemFromPlayer(NodeItem, ItemCheckValue, true);
            }
            dialogueTalk.GetNextNode(this).RunNode(dialogueTalk);
        }
    }
}
