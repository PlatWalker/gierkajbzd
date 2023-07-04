using System.Linq;
using jbzd.Dialogues.Editor.Save;
using jbzd.Dialogues.Editor.Utilities;
using jbzd.Dialogues.RuntimeData;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Timeline;

namespace jbzd.Dialogues.Editor.Nodes
{
    public class CutsceneNode : BasicNode
    {
        public TimelineAsset TimelineAsset { get; set; }
        
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
            
            var quest = DialogueElementUtility.CreateCustomField(TimelineAsset, "TimelineAsset: ", typeof(TimelineAsset),
                callback => TimelineAsset = (TimelineAsset)callback.newValue);
            
            extensionContainer.Add(quest);
            
            outputContainer.Add(outputPort);
            
            RefreshExpandedState();        
        }

        public override NodeEditorData GetSavedData()
        {
            var nodeData = new CutsceneNodeEditorData()
            {
                ID = ID,
                GroupID = GroupID.ToString(),
                NodeType = NodeType,
                Position = GetPosition().position,
                
                TimelineAsset = TimelineAsset
            };

            return nodeData;
        }

        public override void Load(NodeEditorData nodeData)
        {
            var nData =  nodeData as CutsceneNodeEditorData;
            
            TimelineAsset = nData?.TimelineAsset;
        }

        public override NodeRuntimeData GetSavedDataForDialogue()
        {
            var convertedChoices = Choices.Select(choice => choice.ConvertToChoiceData()).ToList();

            var nodeSaveData = new CutsceneRuntimeData
            {
                NodeId = ID,
                TimelineAsset = TimelineAsset,
                NodeType = NodeType,
                IsStartingDialogue = IsStartingNode(),
                Choices = convertedChoices,
            };
            
            return nodeSaveData;
        }
    }
}
