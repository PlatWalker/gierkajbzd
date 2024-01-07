using System;
using System.Collections.Generic;
using UnityEngine;

namespace jbzd.SavingSystem.SaveData
{
    [Serializable]
    public class DisabledAndDestroyedGameObjectsSaveData : ICloneable
    {
        public DisabledAndDestroyedGameObjectsSaveData(
            string sceneName,
            List<string> objectPath,
            string name,
            DisabledOrDestroyed disabledOrDestroyed)
        {
            SceneName = sceneName;
            ObjectPath = objectPath;
            Name = name;
            DisabledOrDestroyed = disabledOrDestroyed;
        }
        
        [field:SerializeField]
        public string SceneName { get; set; }
        [field:SerializeField]
        public List<string> ObjectPath { get; set; }
        [field:SerializeField]
        public string Name { get; set; }
        [field:SerializeField]
        public DisabledOrDestroyed DisabledOrDestroyed { get; set; }
        
        //This should return HARD copy of this class, very important!
        public object Clone()
        {
            return new DisabledAndDestroyedGameObjectsSaveData(
                SceneName,
                new List<string>(ObjectPath),
                Name,
                DisabledOrDestroyed);
        }
    }

    [Serializable]
    public enum DisabledOrDestroyed
    {
        Unrecognized = 0,
        Disabled = 1,
        Destroyed = 2
    }
}