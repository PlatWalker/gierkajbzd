using System;
using System.Collections.Generic;
using System.Linq;
using jbzd.Dialogues.Editor;
using jbzd.Dialogues.Editor.Save;
using jbzd.Dialogues.ScriptableObjects;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace jbzd.Dialogues.Editor.Nodes
{
    public abstract class BasicNode : Node
    {
        public Guid ID { get; set; }
        public List<ChoiceSaveData> Choices { get; set; }
        protected DialogueType DialogueType { get; set; }
        public Guid? GroupID { get; set; }
        protected DialogueGraphView GraphView;

        public virtual void Initialize(Vector2 position, DialogueGraphView graphView)
        {
            ID = Guid.NewGuid();
            title = "Dialog";
            Choices = new List<ChoiceSaveData>();
            GroupID = null;

            GraphView = graphView;
            SetPosition(new Rect(position, Vector2.zero));
            
            mainContainer.AddToClassList("ds-node__main-container");
            extensionContainer.AddToClassList("ds-node__extension-container");

        }

        public bool IsStartingNode()
        {
            Port inputPort = (Port)inputContainer.Children().First();

            return !inputPort.connected;
        }

        public abstract void Draw();

        public abstract NodeSaveData GetSavedData();
        
        public abstract void Load(NodeSaveData nodeData);

        public abstract NodeSO GetSavedDataForDialogue();
    }
}
