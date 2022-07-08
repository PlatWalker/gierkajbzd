using jbzdy.DialogueSystem.Editor;
using jbzdy.DialogueSystem.NodeDatas;
using System;
using System.Collections.Generic;
using System.Linq;
using jbzd.Quests;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace jbzdy.DialogueSystem.Nodes
{
    public class QuestCheckNode : BaseNode
    {
        public override bool AutoDrawOutputEdges { get; } = false;
        private Quest _nodeQuest;
        private QuestGoal _nodeGoal;

        private ObjectField _questField;
        private DropdownField _subtaskDropdownField;

        public string PositiveResultGuid { get; private set; }
        public string NegativeResultGuid { get; private set; }




        public Quest NodeQuest { get => _nodeQuest; set => _nodeQuest = value; }
        public QuestGoal NodeGoal { get => _nodeGoal; set => _nodeGoal = value; }


        public QuestCheckNode()
        {

        }

        public QuestCheckNode(Vector2 position, DialogueEditorWindow newEditorWindow, DialogueGraphView newGraphView)
        {
            StyleSheet styleSheet = Resources.Load<StyleSheet>("QuestCheckNodeStyleSheet");
            styleSheets.Add(styleSheet);

            editorWindow = newEditorWindow;
            graphView = newGraphView;

            title = "Does Player completed quest/task?";
            SetPosition(new Rect(position, defaultNodeSize));
            NodeGuid = Guid.NewGuid().ToString();

            AddInputPort("Input", Port.Capacity.Multi);
            AddOutputPort("Yes", Port.Capacity.Single);
            AddOutputPort("No", Port.Capacity.Single);

            _questField = new ObjectField()
            {
                objectType = typeof(Quest),
                allowSceneObjects = false,

                value = _nodeQuest,
            };

            _questField.RegisterValueChangedCallback(value =>
            {
                _nodeQuest = _questField.value as Quest;
                PopulateDropDownList(_nodeQuest);
                _subtaskDropdownField.index = 0;
            });

            _questField.SetValueWithoutNotify(_nodeQuest);
            mainContainer.Add(_questField);

            _subtaskDropdownField = new();
            _subtaskDropdownField.label = "Subtask";
            _subtaskDropdownField.RegisterValueChangedCallback(value =>
            {
                foreach (var task in NodeQuest.tasks)
                {
                    foreach (var goal in task.goals)
                    {
                        if (goal.Description.Equals(_subtaskDropdownField.value))
                        {
                            NodeGoal = goal;
                        }
                    }
                }
            });
            mainContainer.Add(_subtaskDropdownField);

        }

        private void PopulateDropDownList(Quest nodeQuest)
        {
            List<string> choices = new();
            foreach(var task in nodeQuest.tasks)
            {
                foreach(var goal in task.goals)
                {
                    choices.Add(goal.Description);
                }
            }
            _subtaskDropdownField.choices = choices;
            if(NodeGoal is not null)
            {
                for(int i=0; i < _subtaskDropdownField.choices.Count; i++)
                {
                    if (NodeGoal.Description.Equals(_subtaskDropdownField.choices[i]))
                    {
                        _subtaskDropdownField.index = i;
                    }
                }
            }
        }

        public override void LoadValueInToField()
        {
            _questField.SetValueWithoutNotify(_nodeQuest);
            //dopisać uzupełnienie subtaska
        }
        public override BaseNode CreateNewNode(DialogueEditorWindow newEditorWindow, DialogueGraphView newGraphView)
        {
            return new QuestCheckNode(Vector2.zero, newEditorWindow, newGraphView);
        }

        public override bool DrawNode(DialogueEditorWindow editorWindow, DialogueGraphView graphView, Vector2 graphMousePosition)
        {
            graphView.AddElement(new QuestCheckNode(graphMousePosition, editorWindow, graphView));
            return true;
        }

        public override BaseNodeData GetDataToSave()
        {
            string positiveOutput = "";
            string negativeOutput = "";

            Edge[] connectedEdges = graphView.edges.ToList().FindAll(edge => edge.output.node.Equals(this)).ToArray();

            if (connectedEdges.Length != 2)
            {
                Debug.Log("Podczas zapisu QuestCheck Dialogue Node napotkano inną ilość stanów wyjściowych niż 2!");
            }

            foreach (Edge edge in connectedEdges)
            {
                switch (edge.output.portName)
                {
                    case "Yes":
                        positiveOutput = ((BaseNode)edge.input.node).NodeGuid;
                        break;
                    case "No":
                        negativeOutput = ((BaseNode)edge.input.node).NodeGuid;
                        break;
                    default:
                        Debug.Log("Podczas zapisu QuestCheck Dialogue Node napotkano port o nieodpowiedniej nazwie");
                        break;
                }
            }
            return new QuestCheckNodeData()
            {
                NodeGuid = NodeGuid,
                NegativeResultGuid = negativeOutput,
                PositiveResultGuid = positiveOutput,
                Position = GetPosition().position,
                NodeQuest = NodeQuest,
                NodeTask = NodeGoal
            };
        }

        public override void LoadDataIntoNode(BaseNodeData dataToLoad)
        {
            if (dataToLoad is not QuestCheckNodeData)
            {
                Debug.Log("Podano błędne dane do node");
                return;
            }
            var newData = (QuestCheckNodeData)dataToLoad;
            NodeGuid = newData.NodeGuid;
            SetPosition(new Rect(newData.Position, defaultNodeSize));
            NodeGoal = newData.NodeTask;
            PositiveResultGuid = newData.PositiveResultGuid;
            NegativeResultGuid = newData.NegativeResultGuid;
            LoadValueInToField();

            if (newData.NodeQuest is not null)
            {
                NodeQuest = newData.NodeQuest;
                PopulateDropDownList(NodeQuest);
            }

        }
        

        public override void LinkToOtherNodes(List<BaseNode> allNodes)
        {
            BaseNode positiveNode = allNodes.Find(node=>node.NodeGuid.Equals(PositiveResultGuid));
            BaseNode negativeNode = allNodes.Find(node => node.NodeGuid.Equals(NegativeResultGuid));

            if (positiveNode is not null)
                graphView.Add(MakeNewEdge((Port)outputContainer[0], (Port)positiveNode.inputContainer[0]));
            if (negativeNode is not null)
                graphView.Add(MakeNewEdge((Port)outputContainer[1], (Port)negativeNode.inputContainer[0]));
        }
    }

}