using System.Collections.Generic;
using jbzd.Dialogues.Data;
using UnityEngine;

namespace jbzd.Dialogues.ScriptableObjects
{
    public class NodeSO : ScriptableObject
    {
        [field: SerializeField] public List<ChoiceData> Choices { get; set; }
        [field: SerializeField] public NodeType NodeType { get; set; }
    }
}
