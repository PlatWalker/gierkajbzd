using System;
using UnityEngine;

namespace jbzd.Dialogues.Editor.Save
{
    [Serializable]
    public class DialogueNodeSaveData : NodeSaveData
    {
        [field: SerializeField] public string Text { get; set; }
        [field: SerializeField] public Sprite PlayerImage { get; set; }
        [field: SerializeField] public Sprite NpcImage { get; set; }
        [field: SerializeField] public AudioClip Audio { get; set; }
    }
}
