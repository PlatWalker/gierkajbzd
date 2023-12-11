using System;
using System.Collections.Generic;

namespace jbzd.SavingSystem.SaveData
{
    [Serializable]
    public class DisabledAndDestroyedGameObjectsSaveData
    {
        public List<string> objectPath;
        public string name;
        public DisabledOrDestroyed disabledOrDestroyed;
    }

    [Serializable]
    public enum DisabledOrDestroyed
    {
        Unrecognized = 0,
        Disabled = 1,
        Destroyed = 2
    }
}