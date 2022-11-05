using System.Collections.Generic;
using jbzd.Dialogues.Data;
using UnityEngine;

namespace jbzd.Dialogues.ScriptableObjects
{
    public class EndGroupSO : NodeSO
    {
        [field: SerializeField] public string SelectedGroup { get; set; }
        
        public void Initialize(string selectedGroup)
        {
            Choices = new List<ChoiceData>();
            NodeType = NodeType.EndGroup;
            SelectedGroup = selectedGroup;
        }
    }
}
