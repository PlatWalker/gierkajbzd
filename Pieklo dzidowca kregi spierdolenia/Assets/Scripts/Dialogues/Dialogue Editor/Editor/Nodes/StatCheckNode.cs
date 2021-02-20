using System;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using jbzdy.DialogueSystem.Enums;
using jbzdy.DialogueSystem.Editor;
using UnityEditor.Experimental.GraphView;

namespace jbzdy.DialogueSystem.Nodes
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

        public StatCheckNode(Vector2 _position, DialogueEditorWindow _editorWindow, DialogueGraphView _graphView)
        {
            StyleSheet styleSheet = Resources.Load<StyleSheet>("StatCheckNodeStyleSheet");
            styleSheets.Add(styleSheet); 
            
            editorWindow = _editorWindow;
            graphView = _graphView;

            title = "Stat Check";
            SetPosition(new Rect(_position, defaultNodeSize));
            nodeGuid = Guid.NewGuid().ToString();

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
            statCheckField.SetValueWithoutNotify(statCheckType);
            statCheckValueField.SetValueWithoutNotify(statCheckValue);
        }
    }
}

