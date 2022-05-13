using UnityEngine;

namespace jbzdy.DialogueSystem.NodeDatas
{
    [System.Serializable]
    public abstract class BaseNodeData
    {
        [field: SerializeField]
        public string NodeGuid { get; set; }
        [field: SerializeField]
        public Vector2 Position { get; set; }

        public abstract void RunNode(DialogueTalk dialogueTalk);
    }
}

