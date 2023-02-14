using System;
using System.Collections.Generic;
using ModestTree;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace jbzd.Dialogues.RuntimeData
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "New Dialogue v2")]
    public class ContainerSO : ScriptableObject
    {
        [field: SerializeField] public string FileName { get; set; }
        [field: SerializeField] public string GraphContainerID { get; set; }
        [field: SerializeField] public string ContainerID { get; set; }
        [field: SerializeField] public SerializedDictionary<GroupRuntimeData, ListWrapper> Groups { get; set; }
        public Dictionary<GroupRuntimeData, List<NodeRuntimeData>> UtilityGroup { get; set; } = new();

        public void Initialize(string fileName)
        {
            FileName = fileName;
            GraphContainerID = null;
            ContainerID = AssetDatabase.GUIDFromAssetPath(AssetDatabase.GetAssetPath(this)).ToString();
            Groups = new SerializedDictionary<GroupRuntimeData, ListWrapper>();
        }

        public void LoadGroups()
        {
            if (!UtilityGroup.IsEmpty()) return;
            
            foreach (var group in Groups)
            {
                var newGroup = new GroupRuntimeData()
                {
                    GroupName = group.Key.GroupName,
                    WasGroupUsed = group.Key.WasGroupUsed
                };
                
                UtilityGroup.Add(newGroup, group.Value.myList);
            }
        }
    }

    [Serializable]
    public class ListWrapper
    {
        [field: SerializeField][field: SerializeReference] public List<NodeRuntimeData> myList ;
    }
}
