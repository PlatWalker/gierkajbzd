using System;
using System.Collections.Generic;
using jbzd.LegacyDialogues.NodesDatas;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace jbzd.LegacyDialogues.Editor.Nodes
{
    public class CheckpointSplitterNode : BaseNode
    {
        public override bool AutoDrawOutputEdges { get; } = false;

        private string _checkpointName;
        private string _checkpointTag;
        private int _level;

        private DropdownField _levelDropDownField;
        private DropdownField _checkpointNameDropDownField;
        private DropdownField _checkpointTagDropDownField;

        public string PositiveResultGuid { get; private set; }
        public string NegativeResultGuid { get; private set; }

        private DialoguesCheckPointsSO container;
        public CheckpointSplitterNode()
        {

        }

        public CheckpointSplitterNode(Vector2 position, DialogueEditorWindow newEditorWindow, DialogueGraphView newGraphView)
        {
            container = Resources.Load<DialoguesCheckPointsSO>("DONT_RENAME_OR_MOVE_DialogueCheckpointsList");
            StyleSheet styleSheet = Resources.Load<StyleSheet>("CheckpointSplitterStyleSheet");
            styleSheets.Add(styleSheet);

            editorWindow = newEditorWindow;
            graphView = newGraphView;

            title = "Was Checkpoint reached?";
            SetPosition(new Rect(position, defaultNodeSize));
            NodeGuid = Guid.NewGuid().ToString();

            AddInputPort("Input", Port.Capacity.Multi);
            AddOutputPort("Yes", Port.Capacity.Single);
            AddOutputPort("No", Port.Capacity.Single);

            _levelDropDownField = new();
            _levelDropDownField.label = "Kr¹g";
            List<string> choicesLevel = new();
            choicesLevel.Add("Every kr¹g");
            choicesLevel.Add("Kr¹g 1");
            choicesLevel.Add("Kr¹g 2");
            choicesLevel.Add("Kr¹g 3");
            choicesLevel.Add("Kr¹g 4");
            choicesLevel.Add("Kr¹g 5");
            choicesLevel.Add("Kr¹g 6");
            choicesLevel.Add("Kr¹g 7");
            choicesLevel.Add("Kr¹g 8");
            choicesLevel.Add("Kr¹g 9");
            _levelDropDownField.choices = choicesLevel;
            _levelDropDownField.index = 0;
            _levelDropDownField.RegisterValueChangedCallback(value =>
            {
                _level = _levelDropDownField.index;
                PopulateCheckpointsDropDown();
                PopulateTagsDropdowns();
            });
            
            mainContainer.Add(_levelDropDownField);

            _checkpointTagDropDownField = new();
            _checkpointTagDropDownField.label = "Tag";
            PopulateTagsDropdowns();
            _checkpointTagDropDownField.index = 0;
            _checkpointTagDropDownField.RegisterValueChangedCallback(value =>
            {
                _checkpointTag = _checkpointTagDropDownField.choices[_checkpointTagDropDownField.index];
                PopulateCheckpointsDropDown();
            });
            mainContainer.Add(_checkpointTagDropDownField);

            _checkpointNameDropDownField = new();
            _checkpointNameDropDownField.label = "Name";
            _checkpointNameDropDownField.RegisterValueChangedCallback(value =>
            {
                _checkpointName = _checkpointNameDropDownField.choices[_checkpointNameDropDownField.index];
            });
            mainContainer.Add(_checkpointNameDropDownField);

            this.RegisterCallback<MouseEnterEvent>((evt) =>
            {
                PopulateTagsDropdowns();
                PopulateCheckpointsDropDown();
            });
            
        }
        
        private void PopulateCheckpointsDropDown()
        {
            List<string> choices = new();
            foreach (DialoguesCheckpoint checkpoint in container.CheckpointsList)
            {
                if (checkpoint.Level+1 != _level && _level!=0) continue;
                if ((!checkpoint.Tag.Equals(_checkpointTag)) && _checkpointTagDropDownField.index != 0) continue;
                choices.Add(checkpoint.Name);
            }
            if (!choices.Contains(_checkpointNameDropDownField.value))
            {
                _checkpointNameDropDownField.SetValueWithoutNotify("");
            }
            _checkpointNameDropDownField.choices = choices;
        }
        private void PopulateTagsDropdowns()
        {
            List<string> choicesTag = new();
            choicesTag.Add("Every tags");
            foreach (DialoguesCheckpoint checkpoint in container.CheckpointsList)
            {
                if (choicesTag.Contains(checkpoint.Tag)) continue;

                choicesTag.Add(checkpoint.Tag);
            }
            if (!choicesTag.Contains(_checkpointTagDropDownField.value))
            {
                _checkpointTagDropDownField.index = 0;
            }
            _checkpointTagDropDownField.choices = choicesTag;
        }
        public override void OnSelected()
        {
           
            PopulateTagsDropdowns();
            PopulateCheckpointsDropDown();
            base.OnSelected();

        }
        

        public override BaseNode CreateNewNode(DialogueEditorWindow newEditorWindow, DialogueGraphView newGraphView)
        {
            return new CheckpointSplitterNode(Vector2.zero, newEditorWindow, newGraphView);
        }

        public override bool DrawNode(DialogueEditorWindow editorWindow, DialogueGraphView graphView, Vector2 graphMousePosition)
        {
            graphView.AddElement(new CheckpointSplitterNode(graphMousePosition, editorWindow, graphView));
            return true;
        }

        public override BaseNodeData GetDataToSave()
        {
            string positiveOutput = "";
            string negativeOutput = "";

            Edge[] connectedEdges = graphView.edges.ToList().FindAll(edge => edge.output.node.Equals(this)).ToArray();

            if (connectedEdges.Length != 2)
            {
                Debug.Log("Podczas zapisu Checkpoint Splitter Node napotkano inn¹ iloœæ stanów wyjœciowych ni¿ 2!");
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
            string tag = _checkpointTag;
            if (_checkpointTagDropDownField.index == 0)
            {
                tag = string.Empty;
            }
            return new CheckpointSplitterNodeData()
            {
                NodeGuid = NodeGuid,
                Position = GetPosition().position,
                CheckpointName = _checkpointName,
                CheckpointTag = tag,
                Level = _level,
                NegativeResultGuid = negativeOutput,
                PositiveResultGuid = positiveOutput
            };
        }

        public override void LoadDataIntoNode(BaseNodeData dataToLoad)
        {
            var newData = (CheckpointSplitterNodeData)dataToLoad;
            NodeGuid = newData.NodeGuid;
            SetPosition(new Rect(newData.Position, defaultNodeSize));
            _checkpointTag = 
            PositiveResultGuid = newData.PositiveResultGuid;
            NegativeResultGuid = newData.NegativeResultGuid;
            _checkpointName = newData.CheckpointName;
            _checkpointTag = newData.CheckpointTag;
            _level = newData.Level;
            LoadValueInToField();
        }
        public override void LoadValueInToField()
        {
            base.LoadValueInToField();
            _levelDropDownField.index = _level + 1;
            PopulateTagsDropdowns();
            _checkpointTagDropDownField.index = _checkpointTagDropDownField.choices.FindIndex(choice => choice.Equals(_checkpointTag));
            if (_checkpointTagDropDownField.index < 0)
                _checkpointTagDropDownField.index = 0;
            PopulateCheckpointsDropDown();
            _checkpointNameDropDownField.index = _checkpointNameDropDownField.choices.FindIndex(choice => choice.Equals(_checkpointName));
        }
        public override void LinkToOtherNodes(List<BaseNode> allNodes)
        {
            BaseNode positiveNode = allNodes.Find(node => node.NodeGuid.Equals(PositiveResultGuid));
            BaseNode negativeNode = allNodes.Find(node => node.NodeGuid.Equals(NegativeResultGuid));

            if (positiveNode is not null)
                graphView.Add(MakeNewEdge((Port)outputContainer[0], (Port)positiveNode.inputContainer[0]));
            if (negativeNode is not null)
                graphView.Add(MakeNewEdge((Port)outputContainer[1], (Port)negativeNode.inputContainer[0]));
        }
    }
}