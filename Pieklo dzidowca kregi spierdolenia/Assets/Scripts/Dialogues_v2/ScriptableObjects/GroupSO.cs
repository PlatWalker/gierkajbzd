using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jbzd.dialogues
{
    public class GroupSO : ScriptableObject
    {
        [field: SerializeField] public string GroupName { get; set; }

        public void Initialize(string groupName)
        {
            GroupName = groupName;
        }
    }
}
