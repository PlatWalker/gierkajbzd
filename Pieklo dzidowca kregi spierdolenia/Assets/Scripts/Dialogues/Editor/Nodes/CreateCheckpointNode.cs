using jbzdy.DialogueSystem.Editor;
using jbzdy.DialogueSystem.NodeDatas;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEngine;
using System;
using UnityEditor.Experimental.GraphView;
using System.Collections.Generic;

namespace jbzdy.DialogueSystem.Nodes
{
    public class CreateCheckpointNode : BaseNode
    { 
        
        private string _checkpointName = string.Empty;
        private string _checkpointTag = string.Empty;
        private int _level;
        
        private DropdownField _levelDropDownField;
        private TextField _checkpointNameTextField;
        private TextField _checkpointTagTextField;

        private DialoguesCheckPointsSO container;

        public CreateCheckpointNode()
        {

        }

        public CreateCheckpointNode(Vector2 position, DialogueEditorWindow newEditorWindow, DialogueGraphView newGraphView)
        {
            container = Resources.Load<DialoguesCheckPointsSO>("DONT_RENAME_OR_MOVE_DialogueCheckpointsList");
            StyleSheet styleSheet = Resources.Load<StyleSheet>("CreateCheckpointNodeStyleSheet");
            styleSheets.Add(styleSheet);

            editorWindow = newEditorWindow;
            graphView = newGraphView;

            title = "Create Dialogues Checkpoint";
            SetPosition(new Rect(position, defaultNodeSize));
            NodeGuid = Guid.NewGuid().ToString();

            AddInputPort("Input", Port.Capacity.Multi);
            AddOutputPort("Output", Port.Capacity.Single);

            _levelDropDownField = new();
            _levelDropDownField.label = "Kr퉓";
            List<string> choices = new();
            choices.Add("Kr퉓 1");
            choices.Add("Kr퉓 2");
            choices.Add("Kr퉓 3");
            choices.Add("Kr퉓 4");
            choices.Add("Kr퉓 5");
            choices.Add("Kr퉓 6");
            choices.Add("Kr퉓 7");
            choices.Add("Kr퉓 8");
            choices.Add("Kr퉓 9");
            _levelDropDownField.choices = choices;
            _levelDropDownField.RegisterValueChangedCallback(value =>
            {
                RemoveCheckpoint(_checkpointName, _checkpointTag, _level);
                _level = _levelDropDownField.index;
                AddCheckpoint(_checkpointName, _checkpointTag, _level);
            });
            mainContainer.Add(_levelDropDownField);

            _checkpointTagTextField = new();
            _checkpointTagTextField.label = "Tag";
            _checkpointTagTextField.RegisterValueChangedCallback(value =>
            {
                RemoveCheckpoint(_checkpointName, _checkpointTag, _level);
                _checkpointTag = value.newValue;
                AddCheckpoint(_checkpointName, _checkpointTag, _level);
            });
            _checkpointTagTextField.multiline = false;
            mainContainer.Add(_checkpointTagTextField);

            _checkpointNameTextField = new();
            _checkpointNameTextField.label = "Name";
            _checkpointNameTextField.RegisterValueChangedCallback(value =>
            {
                RemoveCheckpoint(_checkpointName, _checkpointTag, _level);
                _checkpointName = value.newValue;
                AddCheckpoint(_checkpointName, _checkpointTag, _level);
            });
            _checkpointNameTextField.multiline = false;
            mainContainer.Add(_checkpointNameTextField);

            this.capabilities -= Capabilities.Deletable;

            Button deleteButton = new();
            deleteButton.text = "Delete";
            deleteButton.clicked += () =>
            {
                RemoveCheckpoint(_checkpointName, _checkpointTag, _level);
                graphView.RemoveElement(this);
            };
            deleteButton.style.backgroundColor = new Color(0.4f, 0, 0);
            mainContainer.Add(deleteButton);
        }

        private void RemoveCheckpoint(string name,string tag, int level)
        {
            container.CheckpointsList.RemoveAll(checkpoint => checkpoint.Level == level && name.Equals(checkpoint.Name) && tag.Equals(checkpoint.Tag));
        }
        private void AddCheckpoint(string name, string tag, int level)
        {
            container.CheckpointsList.Add(new DialoguesCheckpoint(name, tag, level));
        }
        public override BaseNode CreateNewNode(DialogueEditorWindow newEditorWindow, DialogueGraphView newGraphView)
        {
            return new CreateCheckpointNode(Vector2.zero, newEditorWindow, newGraphView);
        }

        public override bool DrawNode(DialogueEditorWindow editorWindow, DialogueGraphView graphView, Vector2 graphMousePosition)
        {
            graphView.AddElement(new CreateCheckpointNode(graphMousePosition, editorWindow, graphView));
            return true;
        }

        public override BaseNodeData GetDataToSave()
        {
            return new CreateCheckpointNodeData()
            {
                CheckpointName = _checkpointName,
                CheckpointTag = _checkpointTag,
                Level = _level,
                NodeGuid = NodeGuid,
                Position = GetPosition().position
            };
        }

        public override void LoadDataIntoNode(BaseNodeData dataToLoad)
        {
            var newData = (CreateCheckpointNodeData)dataToLoad;
            NodeGuid = newData.NodeGuid;
            SetPosition(new Rect(newData.Position, defaultNodeSize));
            _checkpointName = newData.CheckpointName;
            _checkpointTag = newData.CheckpointTag;
            _level = newData.Level;
            _checkpointNameTextField.SetValueWithoutNotify(_checkpointName);
            _checkpointTagTextField.SetValueWithoutNotify(_checkpointTag);
            _levelDropDownField.index = _level;
            LoadValueInToField();
        }
    }
}