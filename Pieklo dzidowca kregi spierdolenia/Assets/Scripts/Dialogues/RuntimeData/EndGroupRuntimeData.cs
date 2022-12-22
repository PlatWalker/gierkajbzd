using System;
using UnityEngine;

namespace jbzd.Dialogues.RuntimeData
{
    [Serializable]
    public class EndGroupRuntimeData : NodeRuntimeData
    {
        [field: SerializeField] public string SelectedGroup { get; set; }

    }
}
