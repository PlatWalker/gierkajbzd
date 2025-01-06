using System;
using System.Collections.Generic;
using UnityEngine;

namespace jbzd.Dialogues.RuntimeData
{
    [Serializable]
    public class ChoiceRuntimeData
    {
        [field: SerializeField] public string Text { get; set; }
        [field: SerializeField] public string NextDialogue { get; set; }
        [field: NonSerialized] public bool WasUsed { get; set; } = false;
        
        public ChoiceRuntimeData()
        {
            RuntimeDataManager.Register(this);
        }

        public void ResetRuntimeState()
        {
            WasUsed = false;
        }
    }
    
    public static class RuntimeDataManager
    {
        private static List<ChoiceRuntimeData> runtimeDataList = new();

        public static void Register(ChoiceRuntimeData data)
        {
            runtimeDataList.Add(data);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void ResetAllRuntimeData()
        {
            foreach (var data in runtimeDataList)
            {
                data.ResetRuntimeState();
            }
        }
    }
}
