using System;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using jbzdy.DialogueSystem.SO;
using jbzdy.DialogueSystem.Enums;
using jbzdy.DialogueSystem.Editor;
using UnityEditor.Experimental.GraphView;

namespace jbzdy.DialogueSystem.Nodes
{
    public class EventNode : BaseNode
    {
        private DialogueEventSO dialogueEvent;
        private ObjectField objectField;
        private Quest quest;
        private ObjectField questField;
        private StatCheckType checkType = StatCheckType.Exp;

        public DialogueEventSO DialogueEvent { get => dialogueEvent; set => dialogueEvent = value; }
        public Quest Quest { get => quest; set => quest = value; }
        public StatCheckType CheckType { get => checkType; set => checkType = value; }

        public EventNode()
        {
           
        }

        public EventNode(Vector2 position, DialogueEditorWindow newEditorWindow, DialogueGraphView newGraphView)
        {
            StyleSheet styleSheet = Resources.Load<StyleSheet>("EventNodeStyleSheet");
            styleSheets.Add(styleSheet);

            editorWindow = newEditorWindow;
            graphView = newGraphView;

            title = "Event";
            SetPosition(new Rect(position, defaultNodeSize));
            nodeGuid = Guid.NewGuid().ToString();

            AddInputPort("Input", Port.Capacity.Multi);
            AddOutputPort("Output", Port.Capacity.Single);

            objectField = new ObjectField()
            {
                objectType = typeof(DialogueEventSO),
                allowSceneObjects = false,
                value = dialogueEvent,
            };

            objectField.RegisterValueChangedCallback(value =>
            {
                dialogueEvent = objectField.value as DialogueEventSO;
            });

            objectField.SetValueWithoutNotify(dialogueEvent);
            mainContainer.Add(objectField);

            //if (objectField.GetType() == typeof(jbzdy.DialogueSystem.Events.EventGetQuest))
            //{
            questField = new ObjectField
            {
                label = "Quest: ",
                objectType = typeof(Quest),
                allowSceneObjects = false,
                value = quest,
            };
            questField.RegisterValueChangedCallback(value =>
            {
                quest = questField.value as Quest;
            });
            questField.SetValueWithoutNotify(quest);
            mainContainer.Add(questField);
            // }
        }

        public override void LoadValueInToField()
        {
            objectField.SetValueWithoutNotify(dialogueEvent);
            questField.SetValueWithoutNotify(quest);
        }
    }   
}
