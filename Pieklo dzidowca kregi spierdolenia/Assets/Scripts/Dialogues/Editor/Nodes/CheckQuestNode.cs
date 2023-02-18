using System.Linq;
using jbzd.Dialogues.Editor.Save;
using jbzd.Dialogues.Editor.Utilities;
using jbzd.Dialogues.RuntimeData;
using jbzd.QuestSystem.QuestStructureElements;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace jbzd.Dialogues.Editor.Nodes
{
    public class CheckQuestNode : BasicNode
    {
        private QuestSO Quest { get; set; }

        public override void Initialize(Vector2 position, DialogueGraphView graphView)
        {
            base.Initialize(position, graphView);
            NodeType = NodeType.CheckQuest;
            title = "Czy quest został ukończony";
            
            Choices.Add(new ChoiceEditorData(){ Text = "Tak"});
            Choices.Add(new ChoiceEditorData(){ Text = "Nie"});
            
            mainContainer.AddToClassList("ds-check_quest_node__main-container");
        }
        
        public override void Draw()
        {
            Port inputPort =
                this.CreatePort("Input", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);
            
            inputContainer.Add(inputPort);

            foreach (var choice in Choices)
            {
                Port outputPort = this.CreatePort(choice.Text);
                outputPort.userData = choice;
                outputContainer.Add(outputPort);
            }
            
            var quest = DialogueElementUtility.CreateCustomField(Quest, "Quest: ", typeof(QuestSO),
                callback => Quest = (QuestSO)callback.newValue);

            extensionContainer.Add(quest);

            RefreshExpandedState();
        }

        public override NodeEditorData GetSavedData()
        {
            CheckQuestNodeEditorData nodeData = new CheckQuestNodeEditorData()
            {
                Choices = this.Choices,
                GroupID = this.GroupID.ToString(),
                ID = this.ID,
                NodeType = this.NodeType,
                Position = this.GetPosition().position,
                Quest = this.Quest
            };

            return nodeData;
        }

        public override void Load(NodeEditorData nodeData)
        {
            var nData =  nodeData as CheckQuestNodeEditorData;

            Quest = nData.Quest;
        }

        public override NodeRuntimeData GetSavedDataForDialogue()
        {
            var convertedChoices = Choices.Select(choice => choice.ConvertToChoiceData()).ToList();
            
            CheckQuestRuntimeData nodeSaveData = new CheckQuestRuntimeData
            {
                NodeId = ID,
                Quest = Quest,
                NodeType = NodeType,
                IsStartingDialogue = IsStartingNode(),
                Choices = convertedChoices
            };
            
            return nodeSaveData;
        }
    }
}
