using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private static List<string> _graphFileNames  = new();
        private static List<string> _graphFilePaths  = new();
        private static List<ContainerSO> _dialogueContainers = new();
        private static List<DialogueGraphView> _graphViews = new();
        private static List<DialogueEditorWindow> _editorWindows = new();
        
        private static List<List<BasicNode>> _nodes = new();
        private static List<List<DialogueGroup>> _groups = new();
        private static List<Dictionary<Guid, DialogueGroup>> _loadedGroups = new();
        private static List<Dictionary<string, BasicNode>> _loadedNodes = new();

        public static void Initialize(DialogueGraphView dialogueGraphView, DialogueEditorWindow dialogueEditorWindow)
        {
            _graphFileNames.Add("");
            _graphFilePaths.Add("");
            _dialogueContainers.Add(new ContainerSO());

            _graphViews.Add(dialogueGraphView);
            _editorWindows.Add(dialogueEditorWindow);
            _nodes.Add(new List <BasicNode>());
            _groups.Add(new List<DialogueGroup>());
            _loadedGroups.Add(new Dictionary<Guid, DialogueGroup>());
            _loadedNodes.Add(new Dictionary<string, BasicNode>());

        }



        #region save

        public static void Save(int ind)
        {
            PrepareForUse(ind);
            GetElementsFromGraphView(ind);

            _dialogueContainers[ind].Initialize(_graphFileNames[ind]);
            
            GraphSaveDataSO graphData = CreateAsset<GraphSaveDataSO>(_graphFilePaths[ind], $"Graph{_graphFileNames[ind]}");
            
            graphData.Initialize(_graphFileNames[ind], _dialogueContainers[ind].ContainerID);
            _dialogueContainers[ind].GraphContainerID = graphData.GraphContainerID;
            

            SaveGroups(graphData,ind);
            SaveNodes(graphData, ind);
            
            SaveAsset(graphData);
            SaveAsset(_dialogueContainers[ind]);

        }

        private static void SaveGroups(GraphSaveDataSO graphData, int ind)
        {

            graphData.Groups.Clear();

            foreach (var group in _groups[ind])
            {
                SaveGroupToGraph(group, graphData);
                SaveGroupToContainerSO(group, ind);
            }

        }

        private static void SaveGroupToContainerSO(DialogueGroup group, int ind)   
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
            _dialogueContainers[ind].Groups.Add(groupRuntimeData, wrapper);
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
        
        private static void SaveNodes(GraphSaveDataSO graphData, int ind)
        {
            graphData.Nodes.Clear();
            
            foreach (var node in _nodes[ind])
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

        public static void Load(int ind)
        {
            PrepareForUse(ind);

            if (_dialogueContainers[ind].GraphContainerID != null)
            {
                var graphContainer = AssetFromGuid<GraphSaveDataSO>(_dialogueContainers[ind].GraphContainerID);
                if (graphContainer == null)
                {
                    _graphViews[ind].CreateStartGroup();
                    Debug.Log("Kontener dialogu posiada niepoprawne id kontenera grafu");
                    return;
                }

                LoadGroups(graphContainer, ind);
                LoadNodes(graphContainer, ind);
                LoadConnections(ind);
            }
            else
            {
                _graphViews[ind].CreateStartGroup();
            }

        }

        private static void LoadConnections(int ind)
        {
            foreach (var loadedNode in _loadedNodes[ind])
            {
                foreach (Port choicePort in loadedNode.Value.outputContainer.Children())
                {
                    ChoiceEditorData choiceData = (ChoiceEditorData)choicePort.userData;

                    if (!string.IsNullOrEmpty(choiceData.NodeID))
                    {
                        BasicNode nextNode = _loadedNodes[ind][choiceData.NodeID];

                        Port nextNodeInputPort = (Port) nextNode.inputContainer.Children().First();

                        Edge edge = choicePort.ConnectTo(nextNodeInputPort);
                        _graphViews[ind].AddElement(edge);

                        loadedNode.Value.RefreshPorts();
                    }
                }
            }
        }

        private static void LoadNodes(GraphSaveDataSO graphContainer, int ind)
        {
            foreach (var nodeData in graphContainer.Nodes)
            {
                var node = _graphViews[ind].CreateNode(nodeData.Position, nodeData.NodeType, false);
                node.ID = nodeData.ID;
                var clonedChoices = CloneChoices(nodeData.Choices);
                node.Choices = clonedChoices;
                
                node.Load(nodeData);

                node.Draw();
                
                _loadedNodes[ind].Add(node.ID, node);

                if (!string.IsNullOrEmpty(nodeData.GroupID))
                {
                    DialogueGroup group = _loadedGroups[ind][Guid.Parse(nodeData.GroupID)];
                    
                    if (group.name == "Start")
                    {
                        group.Nodes[0].Load(nodeData);
                        continue;
                    }
                    _graphViews[ind].AddElement(node);
                    group.AddElement(node);
                }
                else
                {
                    _graphViews[ind].AddElement(node);
                }
            }
        }

        private static void LoadGroups(GraphSaveDataSO graphContainer, int ind)
        {
            foreach (var groupData in graphContainer.Groups)
            {
                DialogueGroup group = null;
                
                if (groupData.Name == "Start")
                {
                    group = _graphViews[ind].CreateStartGroup(groupData.Position, Guid.Parse(groupData.ID));
                }
                else
                {
                    group = _graphViews[ind].CreateGroup(groupData.Position, groupData.Name);
                    _graphViews[ind].AddElement(group);
                }

                group.ID = Guid.Parse(groupData.ID);
                
                _loadedGroups[ind].Add(group.ID, group);
            }
        }

        #endregion

        private static void PrepareForUse(int ind)
        {
            _dialogueContainers[ind] = _editorWindows[ind].dialogueContainer;

            var assetPath = AssetDatabase.GetAssetPath(_dialogueContainers[ind]);
            
            _graphFileNames[ind] = Path.GetFileNameWithoutExtension(assetPath);
            _graphFilePaths[ind] = Path.GetDirectoryName(assetPath);
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

        private static void GetElementsFromGraphView(int ind)
        {
            _nodes[ind].Clear();
            _groups[ind].Clear();
            
            _graphViews[ind].graphElements.ForEach(element =>
            {
                if (element is BasicNode node)
                {
                    _nodes[ind].Add(node);
                    return;
                }

                if (element is DialogueGroup group)
                {   
                    _groups[ind].Add(group);
                    return;
                }
            });
        }

        public static void Validate(int ind)
        {
            GetElementsFromGraphView(ind);

            List<string> warnings = new List<string>();
            
            foreach (var element in _graphViews[ind].graphElements)
            {
                if (element is IValidate validatedObj)
                    ValidateElement(validatedObj);
            }

            ValidateElement(_graphViews[ind]);

            _editorWindows[ind].ShowValidationResult(warnings);

            void ValidateElement(IValidate element)
            {
                if (!element.IsRuleViolated()) return;
                
                foreach (var warningInfo in element.WarningInfos)
                {
                    if (!warnings.Contains(warningInfo))
                    {
                        warnings.Add(warningInfo);
                    }
                }
                
            }
        }



        public static void Delete(int ind){
            _graphFileNames[ind] = null;
            _graphFilePaths[ind] = null;
            _dialogueContainers[ind] = null;
            _graphViews[ind] = null;
            _editorWindows[ind] = null;
            _nodes[ind] = null;
            _groups[ind] = null;
            _loadedGroups[ind] = null;
            _loadedNodes[ind] = null;           
        }
    }
}