using System.Collections.Generic;
using UnityEngine;

namespace jbzd.Dialogues.RuntimeData
{

    public class DialogueRuntimeData : NodeRuntimeData
    {
        [field: SerializeField] [field: TextArea()] public string Text { get; set; }

        public void Initialize(string text, List<ChoiceRuntimeData> choices, NodeType nodeType,
            bool isStartingDialogue)
        {
            Text = text;
            Choices = choices;
            NodeType = nodeType;
            IsStartingDialogue = isStartingDialogue;
        }
    }
}
