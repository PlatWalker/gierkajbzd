using System;
using System.Collections.Generic;
using UnityEngine;

namespace jbzd.Dialogues.RuntimeData
{
    [Serializable]
    public class NodeRuntimeData
    {
        [field: SerializeField] public string NodeId { get; set; }
        [field: SerializeField] public List<ChoiceRuntimeData> Choices { get; set; }
        [field: SerializeField] public NodeType NodeType { get; set; }
        [field: SerializeField] public bool IsStartingDialogue { get; set; }
    }
}
