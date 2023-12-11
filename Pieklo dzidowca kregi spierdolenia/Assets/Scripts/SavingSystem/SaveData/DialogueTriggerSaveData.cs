using System;
using System.Collections.Generic;

namespace jbzd.SavingSystem.SaveData
{
    [Serializable]
    public class DialogueTriggerSaveData
    {
        public List<string> objectPath;
        public string triggerGameObjectName;
        public bool wasTriggered;
    }
}