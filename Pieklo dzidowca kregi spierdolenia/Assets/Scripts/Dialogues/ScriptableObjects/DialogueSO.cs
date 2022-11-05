using System.Collections.Generic;
using jbzd.Dialogues.Data;
using UnityEngine;

namespace jbzd.Dialogues.ScriptableObjects
{

    public class DialogueSO : NodeSO
    {
        [field: SerializeField] [field: TextArea()] public string Text { get; set; }
        [field: SerializeField] public bool IsStartingDialogue { get; set; }

        public void Initialize(string text, List<ChoiceData> choices, NodeType nodeType,
            bool isStartingDialogue)
        {
            Text = text;
            Choices = choices;
            NodeType = nodeType;
            IsStartingDialogue = isStartingDialogue;
        }
    }
}
