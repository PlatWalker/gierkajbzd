using System;
using System.Collections.Generic;
using System.Linq;
using jbzd.Dialogues.Editor.Save;
using jbzd.Dialogues.Editor.Utilities;
using jbzd.Dialogues.RuntimeData;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace jbzd.Dialogues.Editor.Nodes
{
    public abstract class BasicNode : Node, IValidate
    {
        public string ID { get; set; }
        public List<ChoiceEditorData> Choices { get; set; }
        protected NodeType NodeType { get; set; }
        public Guid? GroupID { get; set; }

        public List<string> WarningInfos { get; set; } = new();

        protected DialogueGraphView GraphView;

        public virtual void Initialize(Vector2 position, DialogueGraphView graphView)
        {
            ID = Guid.NewGuid().ToString();
            title = "Dialog";
            Choices = new List<ChoiceEditorData>();
            GroupID = null;

            GraphView = graphView;
            SetPosition(new Rect(position, Vector2.zero));
            
            mainContainer.AddToClassList("ds-node__main-container");
            extensionContainer.AddToClassList("ds-node__extension-container");

        }

        public bool IsStartingNode()
        {
            var inputPorts = inputContainer.Children().ToList();

            if (inputPorts.Count == 0)
            {
                return true;
            }

            var inputPort = (Port)inputPorts.First();
            
            return !inputPort.connected;
        }

        public abstract void Draw();

        public abstract NodeEditorData GetSavedData();
        
        public abstract void Load(NodeEditorData nodeData);

        public abstract NodeRuntimeData GetSavedDataForDialogue();

        public virtual bool IsRuleViolated() 
        {
            if (GroupID == null)
                WarningInfos.Add("Niektóre węzły są poza grupą. Będą one zapisane w edytorze, ale nie będą używane podczas runtime'a (gry).");

            return WarningInfos.Count > 0;
        }
    }
}
