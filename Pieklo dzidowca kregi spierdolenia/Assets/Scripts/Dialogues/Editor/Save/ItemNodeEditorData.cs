using jbzd.Items;
using UnityEngine;

namespace jbzd.Dialogues.Editor.Save
{
    public class ItemNodeEditorData : NodeEditorData
    {
        [field: SerializeField] public ItemSO Item { get; set; }
        [field: SerializeField] public int ItemsNumber { get; set; }
        [field: SerializeField] public bool IsItemAdded { get; set; }
    }
}
