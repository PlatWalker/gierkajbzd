using System;
using UnityEngine;

namespace jbzd.Dialogues.RuntimeData
{
    [Serializable]
    public class GroupRuntimeData 
    {
        [field: SerializeField] public string GroupName { get; set; }
        [field: SerializeField] public bool WasGroupUsed { get; set; } = false;

    }
}
