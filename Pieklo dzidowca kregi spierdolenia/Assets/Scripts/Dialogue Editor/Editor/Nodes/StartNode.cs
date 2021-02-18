using System;
using UnityEngine;
using jbzdy.DialogueSystem.Editor;
using UnityEditor.Experimental.GraphView;

namespace jbzdy.DialogueSystem.Nodes
{
    public class StartNode : BaseNode
    {

        public StartNode()
        {

        }

        public StartNode(Vector2 _position, DialogueEditorWindow _editorWindow, DialogueGraphView _graphView)
        {
            editorWindow = _editorWindow;
            graphView = _graphView;

            title = "Start";
            SetPosition(new Rect(_position, defaultNodeSize));
            nodeGuid = Guid.NewGuid().ToString();

            AddOutputPort("Output", Port.Capacity.Single);

            RefreshExpandedState();
            RefreshPorts();
        }
    }
}

