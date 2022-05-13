using System;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using jbzdy.DialogueSystem.NodeDatas;
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
            NodeGuid = Guid.NewGuid().ToString();

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
                if (dialogueEvent.GetType() == typeof(jbzdy.DialogueSystem.EventGetQuest))
                    MakeQuest();
                else if (questField != null)
                {               
                    mainContainer.Remove(questField);
                    questField = null;
                }
                
            });

            objectField.SetValueWithoutNotify(dialogueEvent);
            mainContainer.Add(objectField);

        }

        public override void LoadValueInToField()
        {
            objectField.SetValueWithoutNotify(dialogueEvent);

            if(questField != null) questField.SetValueWithoutNotify(quest);
        }

        public void MakeQuest()
        {
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
        }
        public override bool DrawNode(DialogueEditorWindow editorWindow, DialogueGraphView graphView, Vector2 graphMousePosition)
        {
            graphView.AddElement(new EventNode(graphMousePosition, editorWindow, graphView));
            return true;
        }

        public override BaseNodeData GetDataToSave()
        {
            return new EventNodeData()
            {
                DialogueEventSO = DialogueEvent,
                NodeGuid = NodeGuid,
                Position = GetPosition().position,
                QuestSO = Quest
            };
        }

        public override void LoadDataIntoNode(BaseNodeData dataToLoad)
        {
            if(dataToLoad is not EventNodeData)
            {
                Debug.Log("Błędne dane podane do node");
                return;
            }
            EventNodeData newData = (EventNodeData)dataToLoad;
            DialogueEvent = newData.DialogueEventSO;
            NodeGuid = newData.NodeGuid;
            SetPosition(new Rect(newData.Position, defaultNodeSize));
            Quest = newData.QuestSO;
            if (Quest != null || (DialogueEvent != null && DialogueEvent.GetType() == typeof(jbzdy.DialogueSystem.EventGetQuest)))
            {
                MakeQuest();
            }
            LoadValueInToField();
        }

        public override BaseNode CreateNewNode(DialogueEditorWindow newEditorWindow, DialogueGraphView newGraphView)
        {
            return new EventNode(Vector2.zero, newEditorWindow, newGraphView);
        }
    }   
}
