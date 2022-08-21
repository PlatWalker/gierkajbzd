using System;
using jbzd.LegacyDialogues.NodesDatas;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace jbzd.LegacyDialogues.Editor.Nodes
{
    public class EndNode : BaseNode
    {
        private EnumField enumField;
        private EndNodeType endNodeType = EndNodeType.End;

        public EndNodeType EndNodeType { get => endNodeType; set => endNodeType = value; }

        public EndNode()
        {
            
        }

        public EndNode(Vector2 position, DialogueEditorWindow newEditorWindow, DialogueGraphView newGraphView)
        {
            StyleSheet styleSheet = Resources.Load<StyleSheet>("EndNodeStyleSheet");
            styleSheets.Add(styleSheet);

            editorWindow = newEditorWindow;
            graphView = newGraphView;

            title = "End";
            SetPosition(new Rect(position, defaultNodeSize));
            NodeGuid = Guid.NewGuid().ToString();

            AddInputPort("Input", Port.Capacity.Multi);

            enumField = new EnumField()
            {
                value = endNodeType
            };

            enumField.Init(endNodeType);

            enumField.RegisterValueChangedCallback((value) =>
            {
                endNodeType = (EndNodeType)value.newValue;
            });
            enumField.SetValueWithoutNotify(endNodeType);

            mainContainer.Add(enumField);
        }

        public override void LoadValueInToField()
        {
            enumField.SetValueWithoutNotify(endNodeType);
        }
        public override bool DrawNode(DialogueEditorWindow editorWindow, DialogueGraphView graphView, Vector2 graphMousePosition)
        {
            graphView.AddElement(new EndNode(graphMousePosition, editorWindow, graphView));
            return true;
        }

        public override BaseNodeData GetDataToSave()
        {
            return new EndNodeData()
            {
                EndNodeType = EndNodeType,
                NodeGuid = NodeGuid,
                Position = GetPosition().position
            };
        }

        public override void LoadDataIntoNode(BaseNodeData dataToLoad)
        {
            if(dataToLoad is not EndNodeData)
            {
                Debug.Log("Błędne dane");
                return;
            }
            EndNodeData newData = (EndNodeData)dataToLoad;
            SetPosition(new Rect(newData.Position, defaultNodeSize));
            NodeGuid = newData.NodeGuid;
            EndNodeType = newData.EndNodeType;
            LoadValueInToField();
        }

        public override BaseNode CreateNewNode(DialogueEditorWindow newEditorWindow, DialogueGraphView newGraphView)
        {
            return new EndNode(Vector2.zero, newEditorWindow, newGraphView);
        }
    }
}