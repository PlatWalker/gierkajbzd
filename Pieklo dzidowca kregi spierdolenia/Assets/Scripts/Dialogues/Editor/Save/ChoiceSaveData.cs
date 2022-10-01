using System;
using jbzd.Dialogues.Data;
using UnityEngine;

namespace jbzd.Dialogues.Editor.Save
{
    [Serializable]
    public class ChoiceSaveData 
    {
        [field: SerializeField] public string Text { get; set; }
        [field: SerializeField] public Guid? NodeID { get; set; }

        public ChoiceData convertToChoiceData()
        {
            return new ChoiceData() { Text = Text };
        }
    }
}
