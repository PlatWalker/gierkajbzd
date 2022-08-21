using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace jbzd.Dialogues.Editor.Save
{
    public class GraphSaveDataSO : ScriptableObject
    {
        [field: SerializeField] public string FileName { get; set; }
        [field: SerializeField] public string ContainerID { get; set; }
        [field: SerializeField] public string GraphContainerID { get; set; }
        [field: SerializeField][field: SerializeReference] public List<GroupSaveData> Groups { get; set; } = new();
        [field: SerializeField][field: SerializeReference] public List<NodeSaveData> Nodes { get; set; } = new();

        public void Initialize(string filename, string container)
        {
            FileName = filename;
            ContainerID = container;
            GraphContainerID = AssetDatabase.GUIDFromAssetPath(AssetDatabase.GetAssetPath(this)).ToString();

        }
    }
}
