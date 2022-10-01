using System;
using jbzd.Dialogues;
using jbzd.Dialogues.ScriptableObjects;
using UnityEngine;

namespace jbzd.Dialogues.Data
{
    [Serializable]
    public class ChoiceData 
    {
        [field: SerializeField] public string Text { get; set; }
        [field: SerializeField] public NodeSO NextDialogue { get; set; }
    }
}
