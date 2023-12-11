using System;
using System.Collections.Generic;
using System.Linq;
using jbzd.SavingSystem;
using jbzd.SavingSystem.SaveData;
using ModestTree;
#if UNITY_EDITOR
    using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace jbzd.Dialogues.RuntimeData
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "New Dialogue")]
    public class ContainerSO : ScriptableObject, ISaveable
    {
        private const string startGroupName = "Start";
        
        [field: SerializeField] public string FileName { get; set; }
        [field: SerializeField] public string GraphContainerID { get; set; }
        [field: SerializeField] public string ContainerID { get; set; }
        [field: SerializeField] public SerializedDictionary<GroupRuntimeData, ListWrapper> Groups { get; set; }
        public Dictionary<GroupRuntimeData, List<NodeRuntimeData>> UtilityGroup { get; set; } = new();
        [field: NonSerialized] public string CurrentGroup { get; set; } = startGroupName;
        static event UnityAction ResetEvent;
        
        private void OnEnable()
        {
            ResetEvent -= ResetData; ResetEvent += ResetData;
        }
        
        void OnDisable() { ResetEvent -= ResetData; }

        void ResetData()
        {
            CurrentGroup = "Start";
            UtilityGroup.Clear();
        }
        
    #if UNITY_EDITOR
        static ContainerSO()
        { 
            UnityEditor.EditorApplication.playModeStateChanged += LogPlayModeState; 
        }
        static void LogPlayModeState(UnityEditor.PlayModeStateChange state) 
        { if (state is UnityEditor.PlayModeStateChange.EnteredEditMode or UnityEditor.PlayModeStateChange.ExitingEditMode) 
            { ResetEvent?.Invoke(); } }
    #endif
        
        
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
            
            CurrentGroup = ((EndGroupRuntimeData) UtilityGroup
                .FirstOrDefault(x => x.Key.GroupName == CurrentGroup)
                .Value
                .First()).SelectedGroup;
        }

        public void LoadData(GameData gameData)
        {
            var saveData = gameData.dialoguesSaveDatas.FirstOrDefault(data => data.containerID == ContainerID);

            if (saveData is null)
            {
                Debug.LogError($"Something went wrong during loading object: {name}");
                return;
            }
            
            CurrentGroup = saveData.currentGroup;
            
            if (!UtilityGroup.IsEmpty()) return;

            if (CurrentGroup is startGroupName) return;
            
            foreach (var group in Groups)
            {
                UtilityGroup.Add(group.Key, group.Value.myList);
            }
        }

        public void SaveData(ref GameData gameData)
        {
            gameData.dialoguesSaveDatas.Add(new DialoguesSaveData
            {
                containerID = ContainerID,
                currentGroup = CurrentGroup
            });
        }
    }

    [Serializable]
    public class ListWrapper
    {
        [field: SerializeField][field: SerializeReference] public List<NodeRuntimeData> myList ;
    }
}
