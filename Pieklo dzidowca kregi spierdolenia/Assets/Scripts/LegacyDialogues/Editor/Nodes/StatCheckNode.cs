using System;
using jbzd.LegacyDialogues.NodesDatas;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace jbzd.LegacyDialogues.Editor.Nodes
{
    public class StatCheckNode : BaseNode
    {
        private StatCheckType statCheckType = StatCheckType.Luck;
        private string statCheckValue;
        private EnumField statCheckField;
        private TextField statCheckValueField;

        public StatCheckType CheckType { get => statCheckType; set => statCheckType = value; }
        public string StatCheckValue { get => statCheckValue; set => statCheckValue = value; }

        public StatCheckNode()
        {
           
        }

        public StatCheckNode(Vector2 position, DialogueEditorWindow newEditorWindow, DialogueGraphView newGraphView)
        {
            StyleSheet styleSheet = Resources.Load<StyleSheet>("StatCheckNodeStyleSheet");
            styleSheets.Add(styleSheet); 
            
            editorWindow = newEditorWindow;
            graphView = newGraphView;

            title = "Stat Check";
            SetPosition(new Rect(position, defaultNodeSize));
            NodeGuid = Guid.NewGuid().ToString();

            AddInputPort("Input", Port.Capacity.Multi);
            AddOutputPort("Output", Port.Capacity.Single);

            statCheckValueField = new TextField();
            statCheckValueField.RegisterValueChangedCallback(value =>
            {
                statCheckValue = value.newValue;
            });
            statCheckValueField.SetValueWithoutNotify(statCheckValue);
            mainContainer.Add(statCheckValueField);

            statCheckField = new EnumField()
            {
                value = statCheckType
            };

            statCheckField.Init(statCheckType);

            statCheckField.RegisterValueChangedCallback((value) =>
            {
                statCheckType = (StatCheckType)value.newValue;
            });
            statCheckField.SetValueWithoutNotify(statCheckType);

            mainContainer.Add(statCheckField);
        }

        public override void LoadValueInToField()
        {
            statCheckValueField.SetValueWithoutNotify(statCheckValue);
            statCheckField.SetValueWithoutNotify(statCheckType);
        }

        public override bool DrawNode(DialogueEditorWindow editorWindow, DialogueGraphView graphView, Vector2 graphMousePosition)
        {
            graphView.AddElement(new StatCheckNode(graphMousePosition, editorWindow, graphView));
            return true;
        }

        public override BaseNodeData GetDataToSave()
        {
            return new StatCheckNodeData()
            {
                StatCheckType = CheckType,
                StatCheckValue = Int32.Parse(StatCheckValue),
                NodeGuid = NodeGuid,
                Position = GetPosition().position
            };
        }

        public override void LoadDataIntoNode(BaseNodeData dataToLoad)
        {
            if(dataToLoad is not StatCheckNodeData)
            {
                Debug.Log("Podano błędne dane");
                return;
            }
            StatCheckNodeData newData = (StatCheckNodeData)dataToLoad;
            CheckType = newData.StatCheckType;
            StatCheckValue = newData.StatCheckValue.ToString();
            NodeGuid = newData.NodeGuid;
            SetPosition(new Rect(newData.Position, defaultNodeSize));
            LoadValueInToField();
        }

        public override BaseNode CreateNewNode(DialogueEditorWindow newEditorWindow, DialogueGraphView newGraphView)
        {
            return new StatCheckNode(Vector2.zero, newEditorWindow, newGraphView);
        }
    }
}

