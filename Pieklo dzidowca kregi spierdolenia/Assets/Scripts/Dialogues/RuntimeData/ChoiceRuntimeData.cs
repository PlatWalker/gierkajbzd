using System;
using UnityEngine;

namespace jbzd.Dialogues.RuntimeData
{
    [Serializable]
    public class ChoiceRuntimeData 
    {
        [field: SerializeField] public string Text { get; set; }
        [field: SerializeField] public string NextDialogue { get; set; }
    }
}
