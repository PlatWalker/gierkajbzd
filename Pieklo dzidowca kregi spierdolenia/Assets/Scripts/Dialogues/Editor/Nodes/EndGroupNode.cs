using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using jbzd.Dialogues.Editor.Save;
using jbzd.Dialogues.Editor.Utilities;
using jbzd.Dialogues.RuntimeData;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace jbzd.Dialogues.Editor.Nodes
{
    public class EndGroupNode : BasicNode
    {
        private readonly List<string> _groupNames = new();
        public string SelectedGroup { get; set; }
        private DropdownField _dropdownField;
        
        public override void Initialize(Vector2 position, DialogueGraphView graphView)
        {
            base.Initialize(position, graphView);

            title = "End Group";
            NodeType = NodeType.EndGroup;
            
            GraphView.Groups.CollectionChanged += UpdateDropdown;
            GraphView.Groups.ToList().ForEach(x=>
            {
                _groupNames.Add(x.title);
                x.GroupRenamed += UpdateDropdown;
            });
            SelectedGroup = _groupNames.FirstOrDefault();
            
            mainContainer.AddToClassList("ds-end_node__main-container");
        }
        public override void Draw()
        {
            Port inputPort =
                this.CreatePort("Input", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);
            
            inputContainer.Add(inputPort);
            
            _dropdownField = DialogueElementUtility.CreateDropdownField(_groupNames, "Grupa", SelectedGroup,
                value => SelectedGroup = value.newValue);
            mainContainer.Add(_dropdownField);
            
            RefreshExpandedState();
        }

        private void UpdateDropdown(object sender, EventArgs e)
        {
            _groupNames.Clear();
            GraphView.Groups.ToList().ForEach(x=>
            {
                _groupNames.Add(x.title);
                x.GroupRenamed += UpdateDropdown;
            });
            _dropdownField.choices = _groupNames;

            if (!_groupNames.Contains(SelectedGroup))
            {
                SelectedGroup = _groupNames.FirstOrDefault();
                _dropdownField.value = SelectedGroup;
            }
        }

        public override NodeEditorData GetSavedData()
        {
            EndGroupNodeEditorData nodeEditorSaveData = new EndGroupNodeEditorData()
            {
                ID = this.ID.ToString(),
                GroupID = this.GroupID.ToString(),
                NodeType = this.NodeType,
                Position = this.GetPosition().position,
                
                SelectedGroup = SelectedGroup,
            };
            return nodeEditorSaveData;
        }

        public override void Load(NodeEditorData nodeData)
        {
            if(nodeData is EndGroupNodeEditorData nData)
                SelectedGroup = nData.SelectedGroup;
                if(_dropdownField == null){
                    _dropdownField = new DropdownField();
                }
                _dropdownField.value = SelectedGroup;
        }

        public override NodeRuntimeData GetSavedDataForDialogue()
        {
            EndGroupRuntimeData nodeSaveData = new EndGroupRuntimeData
            {
                NodeId = ID,
                SelectedGroup = SelectedGroup,
                NodeType = NodeType,
                IsStartingDialogue = IsStartingNode()
            };

            return nodeSaveData;
        }
    }
}
