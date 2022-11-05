using System;
using System.Collections.Generic;
using UnityEngine;

namespace jbzd.Dialogues.Editor.Save
{
    [Serializable]
    public class NodeSaveData
    {
        [field: SerializeField] public string ID { get; set; }
        [field: SerializeField] public List<ChoiceSaveData> Choices { get; set; }
        [field: SerializeField] public string GroupID { get; set; }
        [field: SerializeField] public NodeType NodeType { get; set; }
        [field: SerializeField] public Vector2 Position { get; set; }
    }
}
