using jbzdy.DialogueSystem.Editor;
using jbzdy.DialogueSystem.NodeDatas;
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace jbzdy.DialogueSystem.Nodes
{
    public class StartNode : BaseNode
    {
        public StartNode()
        {
            StyleSheet styleSheet = Resources.Load<StyleSheet>("NodeStyleSheet");
            styleSheets.Add(styleSheet);
        }

        public StartNode(Vector2 position, DialogueEditorWindow newEditorWindow, DialogueGraphView newGraphView)
        {
            editorWindow = newEditorWindow;
            graphView = newGraphView;

            title = "Start";
            SetPosition(new Rect(position, defaultNodeSize));
            NodeGuid = Guid.NewGuid().ToString();

            AddOutputPort("Output", Port.Capacity.Single);

            RefreshExpandedState();
            RefreshPorts();
        }

        public override BaseNode CreateNewNode(DialogueEditorWindow newEditorWindow, DialogueGraphView newGraphView)
        {
            return new StartNode(new Vector2(0, 0), newEditorWindow, newGraphView);
        }

        public override bool DrawNode(DialogueEditorWindow editorWindow, DialogueGraphView graphView, Vector2 graphMousePosition)
        {
            graphView.AddElement(new StartNode(graphMousePosition, editorWindow, graphView));
            return true;
        }

        public override BaseNodeData GetDataToSave()
        {
            return new StartNodeData()
            {
                NodeGuid = NodeGuid,
                Position = GetPosition().position,
            };
        }

        public override void LoadDataIntoNode(BaseNodeData dataToLoad)
        {
            if(dataToLoad is not StartNodeData)
            {
                Debug.Log("Podano błędne dane do node");
                return;
            }
            NodeGuid = dataToLoad.NodeGuid;
            SetPosition(new Rect(dataToLoad.Position, defaultNodeSize));
        }
    }
}

