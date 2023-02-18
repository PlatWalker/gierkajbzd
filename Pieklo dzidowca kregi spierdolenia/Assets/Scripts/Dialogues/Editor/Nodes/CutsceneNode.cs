using System.Linq;
using jbzd.Dialogues.Editor.Save;
using jbzd.Dialogues.Editor.Utilities;
using jbzd.Dialogues.RuntimeData;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace jbzd.Dialogues.Editor.Nodes
{
    public class CutsceneNode : BasicNode
    {
        public override void Initialize(Vector2 position, DialogueGraphView graphView)
        {
            base.Initialize(position, graphView);
            NodeType = NodeType.Cutscene;
            title = "Rozpocznij Cutscene";
            
            Choices.Add(new ChoiceEditorData(){ Text = "Output"});
            
            mainContainer.AddToClassList("ds-cutscene_node__main-container");
        }
        
        public override void Draw()
        {
            Port inputPort =
                this.CreatePort("Input", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);
            
            inputContainer.Add(inputPort);

            var choice = Choices.First();
            Port outputPort = this.CreatePort(choice.Text);
            outputPort.userData = choice;
            
            outputContainer.Add(outputPort);
            
            RefreshExpandedState();        
        }

        public override NodeEditorData GetSavedData()
        {
            CutsceneNodeEditorData nodeData = new CutsceneNodeEditorData()
            {
                ID = this.ID,
                GroupID = this.GroupID.ToString(),
                NodeType = this.NodeType,
                Position = this.GetPosition().position,
            };

            return nodeData;
        }

        public override void Load(NodeEditorData nodeData)
        {
            var nData =  nodeData as CutsceneNodeEditorData;
        }

        public override NodeRuntimeData GetSavedDataForDialogue()
        {
            var convertedChoices = Choices.Select(choice => choice.ConvertToChoiceData()).ToList();

            CutsceneRuntimeData nodeSaveData = new CutsceneRuntimeData
            {
                NodeId = ID,
                NodeType = NodeType,
                IsStartingDialogue = IsStartingNode(),
                Choices = convertedChoices,
            };
            
            return nodeSaveData;
        }
    }
}
