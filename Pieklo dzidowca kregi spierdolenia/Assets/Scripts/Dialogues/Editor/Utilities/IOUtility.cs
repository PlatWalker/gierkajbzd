using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using jbzd.Dialogues.Editor;
using jbzd.Dialogues.Editor.Nodes;
using jbzd.Dialogues.Editor.Save;
using jbzd.Dialogues.ScriptableObjects;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Object = UnityEngine.Object;

namespace jbzd.Dialogues.Editor.Utilities
{
    public static class IOUtility
    {
        private static string _graphFileName;
        private static string _graphFilePath;
        private static ContainerSO _dialogueContainer;
        private static DialogueGraphView _graphView;
        private static DialogueEditorWindow _editorWindow;
        
        private static List<BasicNode> _nodes;
        private static List<DialogueGroup> _groups;
        private static Dictionary<Guid, DialogueGroup> _loadedGroups;
        private static Dictionary<Guid, BasicNode> _loadedNodes;
        
        public static void Initialize(DialogueGraphView dialogueGraphView, DialogueEditorWindow dialogueEditorWindow)
        {
            _graphView = dialogueGraphView;
            _editorWindow = dialogueEditorWindow;
            _nodes = new List<BasicNode>();
            _groups = new List<DialogueGroup>();
            _loadedGroups = new Dictionary<Guid, DialogueGroup>();
            _loadedNodes = new Dictionary<Guid, BasicNode>();
        }

        #region save

        public static void Save()
        {
            PrepareForUse();
            GetElementsFromGraphView();

            _dialogueContainer.Initialize(_graphFileName);
            
            GraphSaveDataSO graphData = CreateAsset<GraphSaveDataSO>(_graphFilePath, $"Graph{_graphFileName}");
            
            graphData.Initialize(_graphFileName, _dialogueContainer.ContainerID);
            _dialogueContainer.GraphContainerID = graphData.GraphContainerID;
            

            SaveGroups(graphData);
            SaveNodes(graphData);
            
            SaveAsset(graphData);
            SaveAsset(_dialogueContainer);

        }

        private static void SaveGroups(GraphSaveDataSO graphData)
        {

            graphData.Groups.Clear();

            foreach (var group in _groups)
            {
                SaveGroupToGraph(group, graphData);
                SaveGroupToScriptableObject(group);
            }

        }

        private static void SaveGroupToScriptableObject(DialogueGroup group)    //part2
        {
            GroupSO groupSo = new GroupSO()
            {
                GroupName = group.title
            };
            
            //_dialogueContainer.Groups.Add(groupSo);
        }

        private static void SaveGroupToGraph(DialogueGroup group, GraphSaveDataSO graphData)
        {
            GroupSaveData groupData = new GroupSaveData()
            {
                ID = group.ID.ToString(),
                Name = group.title,
                Position = group.GetPosition().position
            };
            
            graphData.Groups.Add(groupData);
        }
        
        private static void SaveNodes(GraphSaveDataSO graphData)
        {
            graphData.Nodes.Clear();
            
            foreach (var node in _nodes)
            {
                SaveNodeToGraph(node, graphData);
                SaveNodesToScriptableObject(node);
            }
            
        }

        private static void SaveNodeToGraph(BasicNode node, GraphSaveDataSO graphData)
        {
            var nodeData = node.GetSavedData();
            var clonedChoices = CloneChoices(node.Choices);
            nodeData.Choices = clonedChoices;
            nodeData.ID = node.ID.ToString();
            graphData.Nodes.Add(nodeData);
        }

        private static void SaveNodesToScriptableObject(BasicNode node) //part2
        {
            
        }
        #endregion

        #region load

        public static void Load()
        {
            PrepareForUse();

            if (_dialogueContainer.GraphContainerID != null)
            {
                var graphContainer = AssetFromGuid<GraphSaveDataSO>(_dialogueContainer.GraphContainerID);
                if (graphContainer == null)
                {
                    Debug.Log("Kontener dialogu posiada niepoprawne id kontenera grafu");
                    return;
                }

                LoadGroups(graphContainer);
                LoadNodes(graphContainer);
                LoadConnections();
            }

        }

        private static void LoadConnections()
        {
            foreach (var loadedNode in _loadedNodes)
            {
                foreach (Port choicePort in loadedNode.Value.outputContainer.Children())
                {
                    ChoiceSaveData choiceData = (ChoiceSaveData)choicePort.userData;

                    if (choiceData.NodeID != null)
                    {
                        BasicNode nextNode = _loadedNodes[(Guid)choiceData.NodeID];

                        Port nextNodeInputPort = (Port) nextNode.inputContainer.Children().First();

                        Edge edge = choicePort.ConnectTo(nextNodeInputPort);
                        _graphView.AddElement(edge);

                        loadedNode.Value.RefreshPorts();
                    }
                }
            }
        }

        private static void LoadNodes(GraphSaveDataSO graphContainer)
        {
            foreach (var nodeData in graphContainer.Nodes)
            {
                var node = _graphView.CreateNode(nodeData.Position, nodeData.DialogueType, false);
                node.ID = Guid.Parse(nodeData.ID);
                var clonedChoices = CloneChoices(nodeData.Choices);
                node.Choices = clonedChoices;
                
                node.Load(nodeData);

                node.Draw();
                _graphView.AddElement(node);
                _loadedNodes.Add(node.ID, node);

                if (!string.IsNullOrEmpty(nodeData.GroupID))
                {
                    DialogueGroup group = _loadedGroups[Guid.Parse(nodeData.GroupID)];
                    group.AddElement(node);
                }
            }
        }

        private static void LoadGroups(GraphSaveDataSO graphContainer)
        {
            foreach (var groupData in graphContainer.Groups)
            {
                var group = _graphView.CreateGroup(groupData.Position, groupData.Name);
                _graphView.AddElement(group);
                group.ID = Guid.Parse(groupData.ID);
                
                _loadedGroups.Add(group.ID, group);
            }
        }

        #endregion

        private static void PrepareForUse()
        {
            _dialogueContainer = _editorWindow.dialogueContainer;

            var assetPath = AssetDatabase.GetAssetPath(_dialogueContainer);
            
            _graphFileName = Path.GetFileNameWithoutExtension(assetPath);
            _graphFilePath = Path.GetDirectoryName(assetPath);
        }
        
        public static T AssetFromGuid<T>(string guid) where T : Object
        {
            var asset = AssetDatabase.GUIDToAssetPath(guid);
            return AssetDatabase.LoadAssetAtPath<T>(asset);
        }
        
        private static List<ChoiceSaveData> CloneChoices(List<ChoiceSaveData> nodeChoices)
        {
            return nodeChoices.Select(choice => 
                new ChoiceSaveData
                {
                    Text = choice.Text,
                    NodeID = choice.NodeID
                }
            ).ToList();
        }
        
        private static T CreateAsset<T>(string path, string assetName) where T : ScriptableObject
        {
            string fullPath = $"{path}/{assetName}.asset";
            
            T asset = AssetDatabase.LoadAssetAtPath<T>(fullPath);

            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<T>();
            
                AssetDatabase.CreateAsset(asset, fullPath);
            }

            return asset;
        }

        private static void SaveAsset(UnityEngine.Object asset)
        {
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void GetElementsFromGraphView()
        {
            _nodes.Clear();
            _groups.Clear();
            
            _graphView.graphElements.ForEach(element =>
            {
                if (element is BasicNode node)
                {
                    _nodes.Add(node);
                    return;
                }

                if (element.GetType() == typeof(DialogueGroup))
                {
                    _groups.Add((DialogueGroup)element);
                    return;
                }
            });
        }
    }
}
