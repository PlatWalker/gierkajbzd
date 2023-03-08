using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
#if UNITY_EDITOR
    using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Rendering;

namespace jbzd.Dialogues.RuntimeData
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "New Dialogue")]
    public class ContainerSO : ScriptableObject
    {
        [field: SerializeField] public string FileName { get; set; }
        [field: SerializeField] public string GraphContainerID { get; set; }
        [field: SerializeField] public string ContainerID { get; set; }
        [field: SerializeField] public SerializedDictionary<GroupRuntimeData, ListWrapper> Groups { get; set; }
        public Dictionary<GroupRuntimeData, List<NodeRuntimeData>> UtilityGroup { get; set; } = new();
        [field: NonSerialized] public string CurrentGroup { get; set; } = "Start";

        public void Initialize(string fileName)
        {
            FileName = fileName;
            GraphContainerID = null;
        #if UNITY_EDITOR
            ContainerID = AssetDatabase.GUIDFromAssetPath(AssetDatabase.GetAssetPath(this)).ToString();
        #endif
            Groups = new SerializedDictionary<GroupRuntimeData, ListWrapper>();
        }

        public void LoadGroups()
        {
            if (!UtilityGroup.IsEmpty()) return;
            
            foreach (var group in Groups)
            {
                UtilityGroup.Add(group.Key, group.Value.myList);
            }
            
            CurrentGroup = ((EndGroupRuntimeData)UtilityGroup.
                FirstOrDefault(x => x.Key.GroupName == CurrentGroup).
                Value.
                First()).
                SelectedGroup;
        }
    }

    [Serializable]
    public class ListWrapper
    {
        [field: SerializeField][field: SerializeReference] public List<NodeRuntimeData> myList ;
    }
}
