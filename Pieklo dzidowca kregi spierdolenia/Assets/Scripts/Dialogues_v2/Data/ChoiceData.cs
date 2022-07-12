using System;
using UnityEngine;

namespace jbzd.dialogues
{
    [Serializable]
    public class ChoiceData 
    {
        [field: SerializeField] public string Text { get; set; }
        [field: SerializeField] public DialogueSO NextDialogue { get; set; }
    }
}
