using System;
using jbzd.Common.RunnerThing;
using UnityEngine;

namespace jbzd.Dialogues.RuntimeData
{
    [Serializable]
    public class EndGroupRuntimeData : NodeRuntimeData
    {
        [field: SerializeField] public string SelectedGroup { get; set; }

        [RunMethod]
        public void Run(DialogueManager manager)
        {
            manager.ChangeGroupStatus();
            manager.ChangeGroupStatus(SelectedGroup, false);
            manager.EndDialogue();
        }
    }
}
