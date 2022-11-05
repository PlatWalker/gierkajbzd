using UnityEngine;

namespace jbzd.Dialogues.Editor.Save
{
    public class EndGroupNodeData : NodeSaveData
    {
        [field: SerializeField] public string SelectedGroup { get; set; }
    }
}
