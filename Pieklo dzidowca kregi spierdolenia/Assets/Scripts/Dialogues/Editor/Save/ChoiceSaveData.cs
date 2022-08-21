using System;
using UnityEngine;

namespace jbzd.Dialogues.Editor.Save
{
    [Serializable]
    public class ChoiceSaveData 
    {
        [field: SerializeField] public string Text { get; set; }
        [field: SerializeField] public Guid? NodeID { get; set; }
    }
}
