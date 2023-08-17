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
    public class CommentNode : BasicNode
    {
        private string Comment { get; set; }
        private TextField commentField {get;set;}
        
        public override void Initialize(Vector2 position, DialogueGraphView graphView)
        {
            base.Initialize(position, graphView);
            NodeType = NodeType.Comment;
            title = "Comment";
            mainContainer.AddToClassList("ds-comment_node__main-container");
        }
        public override void Draw()
        {
            commentField  = DialogueElementUtility.CreateTextArea(Comment, "", value => Comment = value.newValue);
            mainContainer.Add(commentField);       
        }

        public override NodeEditorData GetSavedData()
        {
            CommentNodeEditorData nodeEditorSaveData = new CommentNodeEditorData()
            {
                ID = ID.ToString(),
                GroupID = GroupID.ToString(),
                NodeType = NodeType,
                Position = GetPosition().position,
                Comment = Comment,

            };
            return nodeEditorSaveData;
        }

        public override void Load(NodeEditorData nodeData)
        {
            if(nodeData is CommentNodeEditorData nData)
                Comment = nData.Comment;
        }

        public override NodeRuntimeData GetSavedDataForDialogue()
        {
            CommentRuntimeData nodeSaveData = new CommentRuntimeData
            {
                NodeId = ID,
                Comment = Comment,
                NodeType = NodeType,
            };
            return nodeSaveData;
        }
    }
}
