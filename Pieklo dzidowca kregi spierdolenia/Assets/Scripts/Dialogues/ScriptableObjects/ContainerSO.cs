using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace jbzd.Dialogues.ScriptableObjects
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "New Dialogue v2")]
    public class ContainerSO : ScriptableObject
    {
        [field: SerializeField] public string FileName { get; set; }
        [field: SerializeField] public string GraphContainerID { get; set; }
        [field: SerializeField] public string ContainerID { get; set; }
        [field: SerializeField] public SerializedDictionary<GroupSO, List<NodeSO>> Groups { get; set; }

        public void Initialize(string fileName)
        {
            FileName = fileName;
            GraphContainerID = null;
            ContainerID = AssetDatabase.GUIDFromAssetPath(AssetDatabase.GetAssetPath(this)).ToString();
            Groups = new SerializedDictionary<GroupSO, List<NodeSO>>();

        }
    }
}
