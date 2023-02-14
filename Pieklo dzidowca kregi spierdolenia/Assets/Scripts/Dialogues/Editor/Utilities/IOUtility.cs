using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using jbzd.Dialogues.Editor;
using jbzd.Dialogues.Editor.Nodes;
using jbzd.Dialogues.Editor.Save;
using jbzd.Dialogues.RuntimeData;
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
        private static Dictionary<string, BasicNode> _loadedNodes;
        
        public static void Initialize(DialogueGraphView dialogueGraphView, DialogueEditorWindow dialogueEditorWindow)
        {
            _graphView = dialogueGraphView;
            _editorWindow = dialogueEditorWindow;
            _nodes = new List<BasicNode>();
            _groups = new List<DialogueGroup>();
            _loadedGroups = new Dictionary<Guid, DialogueGroup>();
            _loadedNodes = new Dictionary<string, BasicNode>();
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
                SaveGroupToContainerSO(group);
            }

        }

        private static void SaveGroupToContainerSO(DialogueGroup group)   
        {
            GroupRuntimeData groupRuntimeData = new GroupRuntimeData()
            {
                GroupName = group.title
            };

            var nodeList = new List<NodeRuntimeData>();
            var nodePairs = new Dictionary<string, NodeRuntimeData>();

            foreach (var node in group.Nodes)
            {
                var newNode = node.GetSavedDataForDialogue();  
                nodeList.Add(newNode);
                nodePairs.Add(node.ID, newNode);
            }

            foreach (var node in group.Nodes)
            {
                var choiceList = new List<ChoiceRuntimeData>();
                
                foreach (var choice in node.Choices)
                {
                    var choiceToAdd = new ChoiceRuntimeData
                    {
                        Text = choice.Text
                    };

                    if (choice.NodeID != null)
                    {
                        if(nodePairs.ContainsKey(choice.NodeID))
                            choiceToAdd.NextDialogue = choice.NodeID;
                    }
                    choiceList.Add(choiceToAdd);
                }

                var nodeSO = nodePairs[node.ID]; 
                nodeSO.Choices = choiceList;
            }

            var wrapper = new ListWrapper{ myList = nodeList};
            _dialogueContainer.Groups.Add(groupRuntimeData, wrapper);
        }

        private static void SaveGroupToGraph(DialogueGroup group, GraphSaveDataSO graphData)
        {
            GroupEditorData groupData = new GroupEditorData()
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
                    _graphView.CreateStartGroup();
                    Debug.Log("Kontener dialogu posiada niepoprawne id kontenera grafu");
                    return;
                }

                LoadGroups(graphContainer);
                LoadNodes(graphContainer);
                LoadConnections();
            }
            else
            {
                _graphView.CreateStartGroup();
            }

        }

        private static void LoadConnections()
        {
            foreach (var loadedNode in _loadedNodes)
            {
                foreach (Port choicePort in loadedNode.Value.outputContainer.Children())
                {
                    ChoiceEditorData choiceData = (ChoiceEditorData)choicePort.userData;

                    if (!string.IsNullOrEmpty(choiceData.NodeID))
                    {
                        BasicNode nextNode = _loadedNodes[choiceData.NodeID];

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
                var node = _graphView.CreateNode(nodeData.Position, nodeData.NodeType, false);
                node.ID = nodeData.ID;
                var clonedChoices = CloneChoices(nodeData.Choices);
                node.Choices = clonedChoices;
                
                node.Load(nodeData);

                node.Draw();
                
                _loadedNodes.Add(node.ID, node);

                if (!string.IsNullOrEmpty(nodeData.GroupID))
                {
                    DialogueGroup group = _loadedGroups[Guid.Parse(nodeData.GroupID)];
                    
                    if (group.name == "Start")
                    {
                        group.Nodes[0].Load(nodeData);
                        continue;
                    }
                    _graphView.AddElement(node);
                    group.AddElement(node);
                }
                else
                {
                    _graphView.AddElement(node);
                }
            }
        }

        private static void LoadGroups(GraphSaveDataSO graphContainer)
        {
            foreach (var groupData in graphContainer.Groups)
            {
                DialogueGroup group = null;
                
                if (groupData.Name == "Start")
                {
                    group = _graphView.CreateStartGroup(groupData.Position, Guid.Parse(groupData.ID));
                }
                else
                {
                    group = _graphView.CreateGroup(groupData.Position, groupData.Name);
                    _graphView.AddElement(group);
                }

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
        
        private static List<ChoiceEditorData> CloneChoices(List<ChoiceEditorData> nodeChoices)
        {
            return nodeChoices.Select(choice => 
                new ChoiceEditorData
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

                if (element is DialogueGroup group)
                {
                    _groups.Add(group);
                    return;
                }
            });
        }

        public static void Validate()
        {
            GetElementsFromGraphView();

            List<string> warnings = new List<string>();
            var warning1 = "Niektóre węzły są poza grupą. Będą one zapisane w edytorze, " +
                           "ale nie będą używane podczas runtime'a (gry).";
            var warning2 = "W niektórych węzłach tekst jest za długi i będzie źle wyglądał.";
            
            foreach (var node in _nodes)
            {
                if (node.GroupID == null && !warnings.Contains(warning1))
                {
                    warnings.Add(warning1);
                }

                if (node.GetType().IsSubclassOf(typeof(DialogueNode)) && !warnings.Contains(warning2))
                {
                    if (((DialogueNode)node).CheckTextLength())
                    {
                        warnings.Add(warning2);
                    }
                }
            }

            _editorWindow.ShowValidationResult(warnings);
        }
    }
}
