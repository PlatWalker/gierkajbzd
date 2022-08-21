using System.Collections.Generic;
using jbzd.LegacyDialogues.NodesDatas;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace jbzd.LegacyDialogues.Editor.Nodes
{
    public abstract class BaseNode : Node
    {
        public virtual bool AutoDrawOutputEdges { get; } = true;
        protected DialogueGraphView graphView;
        protected DialogueEditorWindow editorWindow;
        protected Vector2 defaultNodeSize = new Vector2(200, 250);

        public string NodeGuid { get; set; }

        public BaseNode()
        {
            StyleSheet styleSheet = Resources.Load<StyleSheet>("NodeStyleSheet");
            styleSheets.Add(styleSheet);
        }

        public void AddOutputPort(string outputPortName, Port.Capacity capacity = Port.Capacity.Single)
        {
            Port outputPort = GetPortInstance(Direction.Output, capacity);
            outputPort.portName = outputPortName;

            outputContainer.Add(outputPort);
        }

        public void AddInputPort(string inputPortName, Port.Capacity capacity = Port.Capacity.Multi)
        {
            Port inputPort = GetPortInstance(Direction.Input, capacity);
            inputPort.portName = inputPortName;

            inputContainer.Add(inputPort);
        }

        public Port GetPortInstance(Direction nodeDirection, Port.Capacity capacity = Port.Capacity.Single)
        {
            return InstantiatePort(Orientation.Horizontal, nodeDirection, capacity, typeof(float));
        }

        public virtual void LoadValueInToField()
        {

        }
        public abstract bool DrawNode(DialogueEditorWindow editorWindow, DialogueGraphView graphView, Vector2 graphMousePosition);

        public abstract BaseNodeData GetDataToSave();

        public abstract void LoadDataIntoNode(BaseNodeData dataToLoad);

        public abstract BaseNode CreateNewNode(DialogueEditorWindow newEditorWindow, DialogueGraphView newGraphView);

        public virtual void LinkToOtherNodes(List<BaseNode> allNodes)
        {
            Debug.Log("Nastąpiła próba specyficznego łączenia node'ów pomimo braku imlementacji tejże funkcji w nodzie");
        }
        protected Edge MakeNewEdge(Port outputPort, Port inputPort)
        {
            Edge tempEdge = new Edge()
            {
                output = outputPort,
                input = inputPort
            };

            tempEdge.input.Connect(tempEdge);
            tempEdge.output.Connect(tempEdge);
            return tempEdge;
        }
    }
}
