using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using jbzd.Dialogues.Editor.Save;
using jbzd.Dialogues.Editor.Utilities;
using jbzd.Dialogues.ScriptableObjects;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace jbzd.Dialogues.Editor.Nodes
{
    public class EndGroupNode : BasicNode
    {
        private readonly List<string> _groupNames = new();
        private string SelectedGroup { get; set; }
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
            }
        }

        public override NodeSaveData GetSavedData()
        {
            EndGroupNodeData nodeSaveData = new EndGroupNodeData()
            {
                ID = this.ID.ToString(),
                GroupID = this.GroupID.ToString(),
                NodeType = this.NodeType,
                Position = this.GetPosition().position,
                
                SelectedGroup = SelectedGroup,
            };
            return nodeSaveData;
        }

        public override void Load(NodeSaveData nodeData)
        {
            if(nodeData is EndGroupNodeData nData)
                SelectedGroup = nData.SelectedGroup;
        }

        public override NodeSO GetSavedDataForDialogue()
        {
            EndGroupSO nodeSaveData = ScriptableObject.CreateInstance<EndGroupSO>();

            nodeSaveData.Initialize(SelectedGroup);
            
            return nodeSaveData;
        }
    }
}
