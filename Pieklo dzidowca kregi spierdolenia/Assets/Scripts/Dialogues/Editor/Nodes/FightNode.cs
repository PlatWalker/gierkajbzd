using System.Linq;
using jbzd.Dialogues.Editor.Save;
using jbzd.Dialogues.Editor.Utilities;
using jbzd.Dialogues.RuntimeData;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace jbzd.Dialogues.Editor.Nodes
{
    public class FightNode : BasicNode
    {
        public override void Initialize(Vector2 position, DialogueGraphView graphView)
        {
            base.Initialize(position, graphView);
            NodeType = NodeType.Fight;
            title = "Rozpocznij Walke";
            
            Choices.Add(new ChoiceEditorData(){ Text = "Output"});
            
            mainContainer.AddToClassList("ds-fight_node__main-container");
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
            FightNodeEditorData nodeData = new FightNodeEditorData()
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
            var nData =  nodeData as FightNodeEditorData;
        }

        public override NodeRuntimeData GetSavedDataForDialogue()
        {
            var convertedChoices = Choices.Select(choice => choice.ConvertToChoiceData()).ToList();

            FightRuntimeData nodeSaveData = new FightRuntimeData
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
