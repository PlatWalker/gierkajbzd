using System.Collections.Generic;
using jbzd.Dialogues.Data;
using UnityEngine;

namespace jbzd.Dialogues.ScriptableObjects
{

    public class DialogueSO : ScriptableObject
    {
        [field: SerializeField] [field: TextArea()] public string Text { get; set; }
        [field: SerializeField] public List<ChoiceData> Choices { get; set; }
        [field: SerializeField] public DialogueType DialogueType { get; set; }
        [field: SerializeField] public bool IsStartingDialogue { get; set; }

        public void Initialize(string text, List<ChoiceData> choices, DialogueType dialogueType,
            bool isStartingDialogue)
        {
            Text = text;
            Choices = choices;
            DialogueType = dialogueType;
            IsStartingDialogue = isStartingDialogue;
        }
    }
}
