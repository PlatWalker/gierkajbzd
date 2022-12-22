using System;
using jbzd.Dialogues.RuntimeData;
using UnityEngine;

namespace jbzd.Dialogues.Editor.Save
{
    [Serializable]
    public class ChoiceEditorData 
    {
        [field: SerializeField] public string Text { get; set; }
        [field: SerializeField] public string NodeID { get; set; }

        public ChoiceRuntimeData ConvertToChoiceData()
        {
            return new ChoiceRuntimeData() { Text = Text };
        }
    }
}
