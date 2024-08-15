using jbzd.Common;
using System;
using UnityEngine;

namespace jbzd.Common.AssetsHelper
{
    public class DuplicatedScriptableObjects : ScriptableObject
    {
        [field: JbzdReadOnly]
        [field: SerializeField]
        public string Id { get; set; }

        public void OnEnable()
        {
            if (!HasGeneratedGuid()) ForceGenerateGuid();
        }

        public void ForceGenerateGuid()
        {
            Id = Guid.NewGuid().ToString();
        }

        public bool HasGeneratedGuid() => !string.IsNullOrEmpty(Id);


    }
}
