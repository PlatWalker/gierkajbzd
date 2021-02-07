using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using Button = UnityEngine.UIElements.Button;
using UnityEditor;

/// <summary>
/// Napisane przez Sharashino
/// 
/// potem to opisze jak jakiś noob nie rozumie
/// </summary>
public class DialogueGraphView : GraphView
{
    public readonly Vector2 defaultNodeSize = new Vector2(150, 200);
    private NodeSearchWindow searchWindow;

    public DialogueGraphView(EditorWindow editorWindow)
    {
        styleSheets.Add(Resources.Load<StyleSheet>("DialogueGraph"));

        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        GridBackground grid = new GridBackground();
        Insert(0, grid);
        grid.StretchToParentSize();

        AddElement(GetEntryPointNodeInstance());
        AddSearchWindow(editorWindow);
    }

    private void AddSearchWindow(EditorWindow editorWindow)
    {
        searchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
        searchWindow.Init(editorWindow, this);
        nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        List<Port> compatiblePorts = new List<Port>();
        var startPortView = startPort;

        ports.ForEach(port => 
        {
            var portView = port;

            if(startPort != port && startPortView.node != portView.node)
            {
                compatiblePorts.Add(port);
            }

        });

        return compatiblePorts;
    }

    public void CreateNode(string nodeName, Vector2 position)
    {
        AddElement(CreateDialogueNode(nodeName, position));
    }

    public DialogueNode CreateDialogueNode(string nodeName, Vector2 position)
    {
        var dialogueNode = new DialogueNode
        {
            title = nodeName,
            DialogueText = nodeName,
            GUID = Guid.NewGuid().ToString()
        };

        dialogueNode.styleSheets.Add(Resources.Load<StyleSheet>("Node"));
        Port inputPort = GeneratePort(dialogueNode, Direction.Input, Port.Capacity.Multi);
        inputPort.portName = "Input";
        dialogueNode.inputContainer.Add(inputPort);
        dialogueNode.RefreshExpandedState();
        dialogueNode.RefreshPorts();
        dialogueNode.SetPosition(new Rect(position, defaultNodeSize));

        var textField = new TextField("");
        textField.RegisterValueChangedCallback(evt =>
        {
            dialogueNode.DialogueText = evt.newValue;
            dialogueNode.title = evt.newValue;
        });

        textField.SetValueWithoutNotify(dialogueNode.title);
        dialogueNode.mainContainer.Add(textField);

        var button = new Button(() => { AddChoicePort(dialogueNode); })
        {
            text = "Add Choice"
        };

        dialogueNode.titleButtonContainer.Add(button);
        return dialogueNode;
    }

    public void AddChoicePort(DialogueNode dialogueNode, string overridenPortName = "")
    {
        Port generatedPort = GeneratePort(dialogueNode, Direction.Output);
        Label oldLabel = generatedPort.contentContainer.Q<Label>("type");
        generatedPort.contentContainer.Remove(oldLabel);

        int outputPortCount = dialogueNode.outputContainer.Query(name = "connector").ToList().Count;
        string choicePortName = string.IsNullOrEmpty(overridenPortName) ? $"Choice {outputPortCount + 1}" : overridenPortName;

        TextField textField = new TextField()
        {
            name = string.Empty,
            value = choicePortName
        };

        textField.RegisterValueChangedCallback(evt => generatedPort.portName = evt.newValue);
        generatedPort.contentContainer.Add(new Label("  "));
        generatedPort.contentContainer.Add(textField);
        
        Button deleteButton = new Button(() => RemovePort(dialogueNode, generatedPort))
        {
            text = "x"
        };

        generatedPort.contentContainer.Add(deleteButton);
        generatedPort.portName = choicePortName; 
        dialogueNode.outputContainer.Add(generatedPort);
        dialogueNode.RefreshPorts();
        dialogueNode.RefreshExpandedState();
    }

    private Port GeneratePort(DialogueNode node, Direction portDirection, Port.Capacity capacity = Port.Capacity.Single)
    {
        return node.InstantiatePort(Orientation.Horizontal, portDirection, capacity, typeof(float));
    }

    private void RemovePort(DialogueNode dialogueNode, Port generatedPort)
    {
        var targetEdge = edges.ToList().Where(x => x.output.portName == generatedPort.portName && x.output.node == generatedPort.node);

        if (targetEdge.Any())
        {
            Edge edge = targetEdge.First();
            edge.input.Disconnect(edge);
            RemoveElement(targetEdge.First());
        }

        dialogueNode.outputContainer.Remove(generatedPort);
        dialogueNode.RefreshPorts();
        dialogueNode.RefreshExpandedState();
    }

    private DialogueNode GetEntryPointNodeInstance()
    {
        DialogueNode nodeCache = new DialogueNode()
        {
            title = "START",
            GUID = Guid.NewGuid().ToString(),
            DialogueText = "ENTRYPOINT",
            EntryPoint = true
        };

        var generatedPort = GeneratePort(nodeCache, Direction.Output);
        generatedPort.portName = "Next";

        nodeCache.outputContainer.Add(generatedPort);
        nodeCache.capabilities &= ~Capabilities.Movable;
        nodeCache.capabilities &= ~Capabilities.Deletable;

        nodeCache.RefreshExpandedState();
        nodeCache.RefreshPorts();
        nodeCache.SetPosition(new Rect(100, 200, 100, 150));

        return nodeCache;
    }
}
 