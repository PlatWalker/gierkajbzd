using System.Linq;
using UnityEditor;
using UnityEngine.UIElements;
using jbzdy.DialogueSystem.NodeDatas;
using System.Collections.Generic;
using jbzdy.DialogueSystem.Nodes;
using jbzdy.DialogueSystem.Editor;
using UnityEditor.Experimental.GraphView;

namespace jbzdy.DialogueSystem.SaveLoad
{
    public class DialogueSaveAndLoad
    {
        private List<Edge> _edges => _graphView.edges.ToList();
        private List<BaseNode> _nodes => _graphView.nodes.ToList().Where(node => node is BaseNode).Cast<BaseNode>().ToList();

        private DialogueGraphView _graphView;
        private DialogueEditorWindow _editorWindow;

        public DialogueSaveAndLoad(DialogueGraphView newGraphView, DialogueEditorWindow newEditorWindow)
        {
            _editorWindow = newEditorWindow;
            _graphView = newGraphView;
        }

        public void Save(DialogueContainerSO dialogueContainerSO)
        {
            SaveEdges(dialogueContainerSO);
            SaveNodes(dialogueContainerSO);

            EditorUtility.SetDirty(dialogueContainerSO);
            AssetDatabase.SaveAssets();

        }

        public void Load(DialogueContainerSO dialogueContainerSO)
        {
            ClearGraph();
            GenerateNodes(dialogueContainerSO);
            ConnectNodes(dialogueContainerSO);
        }

        #region Save
        private void SaveEdges(DialogueContainerSO dialogueContainerSO)
        {
            dialogueContainerSO.NodeLinkDatas.Clear();
            Edge[] connectedEdges = _edges.Where(edge => edge.input.node != null).ToArray();
            
            foreach (var edge in connectedEdges)
            {
                BaseNode outputNode = (BaseNode)edge.output.node;
                BaseNode inputNode = edge.input.node as BaseNode;


                dialogueContainerSO.NodeLinkDatas.Add(new NodeLinkData
                {
                    BaseNodeGuid = outputNode.NodeGuid,
                    TargetNodeGuid = inputNode.NodeGuid
                });
            }
        }

        private void SaveNodes(DialogueContainerSO dialogueContainerSO)
        {
            dialogueContainerSO.NodeDatas.Clear();
            foreach(BaseNode node in _nodes)
            {
                dialogueContainerSO.NodeDatas.Add(node.GetDataToSave());
            }
        }



        #endregion

        #region Load

        private void ClearGraph()
        {
            _edges.ForEach(edge => _graphView.RemoveElement(edge));

            foreach (BaseNode node in _nodes)
            {
                _graphView.RemoveElement(node);
            }
        }

        private void GenerateNodes(DialogueContainerSO dialogueContainer)
        {
            NodeList nodeList = new();
            foreach (BaseNodeData nodeData in dialogueContainer.NodeDatas)
            {
                BaseNode tempNode = nodeList.GenerateNodeBasedOnData(nodeData,_editorWindow,_graphView);
                _graphView.AddElement(tempNode);
            }
        }

        private void ConnectNodes(DialogueContainerSO dialogueContainer)
        {
            foreach (var node in _nodes)
            {
                List<NodeLinkData> connections = dialogueContainer.NodeLinkDatas.Where(edge => edge.BaseNodeGuid == node.NodeGuid).ToList();

                for (int j = 0; j < connections.Count; j++)
                {
                    string targetNodeGuid = connections[j].TargetNodeGuid;
                    BaseNode targetNode = _nodes.First(node => node.NodeGuid == targetNodeGuid);

                    if (node.AutoDrawOutputEdges)
                    {
                        LinkNodesTogether(node.outputContainer[j].Q<Port>(), (Port)targetNode.inputContainer[0]);
                    }
                }
            }

            List<BaseNode> nodesWithSpecificLinkingConditions = _nodes.FindAll(node => node is BaseNode).Cast<BaseNode>().Where(node=>node.AutoDrawOutputEdges==false).ToList();
            foreach(var node in nodesWithSpecificLinkingConditions)
            {
                node.LinkToOtherNodes(_nodes);
            }

        }

        private void LinkNodesTogether(Port outputPort, Port inputPort)
        {
            Edge tempEdge = new Edge()
            {
                output = outputPort,
                input = inputPort
            };

            tempEdge.input.Connect(tempEdge);
            tempEdge.output.Connect(tempEdge);
            _graphView.Add(tempEdge);
        }

        #endregion
    }
}

