using System;
using jbzd.Common.RunnerThing;
using UnityEngine;

namespace jbzd.Dialogues.RuntimeData
{
    [Serializable]
    public class CommentRuntimeData : NodeRuntimeData
    {
        [field: SerializeField] public string Comment { get; set; }

        [RunMethod]
        public void Run(DialogueManager manager)
        {
            Debug.LogError("Komentarz się odpalił w dialogu");
        }
    }
}
