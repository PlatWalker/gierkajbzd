using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jbzd.dialogues
{
    public class GraphSaveDataSO : ScriptableObject
    {
        [field: SerializeField] public string FileName { get; set; }
        [field: SerializeField] public List<GroupSaveData> Groups { get; set; }
        [field: SerializeField] public List<NodeSaveData> Nodes { get; set; }

        public void Initialize(string filename)
        {
            FileName = filename;
            Groups = new List<GroupSaveData>();
            Nodes = new List<NodeSaveData>();
        }
    }
}
