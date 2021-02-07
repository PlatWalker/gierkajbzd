using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

/// <summary>
/// Napisane przez sharashino
/// 
/// potem to opisze jak jakiś noob nie rozumie
/// </summary>
public class GraphSaveUtility
{
    private DialogueGraphView dialogueGraphView;
    private DialogueContainer containerCache;

    private List<Edge> Edges => dialogueGraphView.edges.ToList();
    private List<DialogueNode> Nodes => dialogueGraphView.nodes.ToList().Cast<DialogueNode>().ToList();

    public static GraphSaveUtility GetInstance(DialogueGraphView targetGraphView)
    {
        return new GraphSaveUtility
        {
            dialogueGraphView = targetGraphView
        };
    }

    public void SaveGraph(string fileName)
    {

        var dialogueContainer = ScriptableObject.CreateInstance<DialogueContainer>();
        var connectedPorts = Edges.Where(x => x.input.node != null).ToArray();

        for (int i = 0; i < connectedPorts.Length; i++)
        {
            DialogueNode outputNode = connectedPorts[i].output.node as DialogueNode;
            DialogueNode inputNode = connectedPorts[i].input.node as DialogueNode;

            dialogueContainer.NodeLinks.Add(new NodeLinkData
            {
                BaseNodeGuid = outputNode.GUID,
                PortName = connectedPorts[i].output.portName,
                TargetNodeGuid = inputNode.GUID
            });
        }

        foreach (var dialogueNode in Nodes.Where(node => !node.EntryPoint))
        {
            dialogueContainer.DialogueNodeDatas.Add(new DialogueNodeData
            {
                NodeGUID = dialogueNode.GUID,
                DialogueText = dialogueNode.DialogueText,
                Position = dialogueNode.GetPosition().position
            });
        }
        
        if(!AssetDatabase.IsValidFolder("Assets/DialogueResources"))
        {
            AssetDatabase.CreateFolder("Assets", "DialogueResources");
        }

        AssetDatabase.CreateAsset(dialogueContainer, $"Assets/DialogueResources/{fileName}.asset");
        AssetDatabase.SaveAssets();
    }

    public void LoadGraph(string fileName)
    {
        containerCache = Resources.Load<DialogueContainer>(fileName);

        if(containerCache == null)
        {
            EditorUtility.DisplayDialog("File not found", "Dialogue graph file doesnt exist, nigger nigger nigger", "FUCK");
            return;
        }

        ClearGraph();
        CreateNodes();
        ConnectNodes();
    }

    private void ClearGraph()
    {
        //Ustaw początkowe punkty guida z save. Wypierdol obecny GUID
        Nodes.Find(x => x.EntryPoint).GUID = containerCache.NodeLinks[0].BaseNodeGuid;

        foreach (var node in Nodes)
        {
            if (node.EntryPoint) continue;

            //Najpierw usuń krawędzie co łączą to kolano 
            Edges.Where(x => x.input.node == node).ToList().ForEach(edge => dialogueGraphView.RemoveElement(edge));

            //A potem go wypierdol
            dialogueGraphView.RemoveElement(node);
        }
    }
    private void CreateNodes()
    {
        foreach (var nodeData in containerCache.DialogueNodeDatas)
        {
            var tempNode = dialogueGraphView.CreateDialogueNode(nodeData.DialogueText, Vector2.zero);
            tempNode.GUID = nodeData.NodeGUID;
            dialogueGraphView.AddElement(tempNode);

            var nodePorts = containerCache.NodeLinks.Where(x => x.BaseNodeGuid == nodeData.NodeGUID).ToList();

            nodePorts.ForEach(x => dialogueGraphView.AddChoicePort(tempNode, x.PortName));
        }
    }

    private void ConnectNodes()
    {
        for (int i = 0; i < Nodes.Count; i++)
        {
            var connections = containerCache.NodeLinks.Where(x => x.BaseNodeGuid == Nodes[i].GUID).ToList();

            for (var j = 0; j < connections.Count; j++)
            {
                var targetNodeGuid = connections[i].TargetNodeGuid;
                var targetNode = Nodes.First(x => x.GUID == targetNodeGuid);
                LinkNodes(Nodes[i].outputContainer[j].Q<Port>(), (Port)targetNode.inputContainer[0]);

                targetNode.SetPosition(new Rect(containerCache.DialogueNodeDatas.First(x => x.NodeGUID == targetNodeGuid).Position,
                    dialogueGraphView.defaultNodeSize
                ));
            }
        }
    }

    private void LinkNodes(Port output, Port input)
    {
        Edge tempEdge = new Edge
        {
            output = output,
            input = input
        };

        tempEdge?.input.Connect(tempEdge);
        tempEdge?.output.Connect(tempEdge);

        dialogueGraphView.Add(tempEdge);
    }
}
