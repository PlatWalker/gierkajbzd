using UnityEngine;

namespace jbzdy.DialogueSystem.NodeDatas
{
    [System.Serializable]
    public class DialoguesCheckpoint
    {
        [field: SerializeField]
        public string Name { get; set; } = string.Empty;
        [field: SerializeField]
        public string Tag { get; set; } = string.Empty;
        [field: SerializeField]
        public int Level { get; set; } = 0;

        [field: SerializeField]
        public bool isSet { get; set; } = false;
        public DialoguesCheckpoint(string name, string tag, int level)
        {
            if(name is not null) 
                Name = name;
            if(tag is not null)
                Tag = tag;
            Level = level;
        }
    }
}