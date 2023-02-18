using System.Linq;
using jbzd.Dialogues.Editor.Save;
using jbzd.Dialogues.Editor.Utilities;
using jbzd.Dialogues.RuntimeData;
using jbzd.QuestSystem.QuestStructureElements;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace jbzd.Dialogues.Editor.Nodes
{
    public class QuestNode : BasicNode
    {
        private QuestSO Quest { get; set; }
        private bool IsQuestStart { get; set; } = false;

        private const string START_QUEST_TITLE = "Rozpocznij Quest";
        private const string END_QUEST_TITLE = "Zakończ Quest";
        
        public override void Initialize(Vector2 position, DialogueGraphView graphView)
        {
            base.Initialize(position, graphView);
            NodeType = NodeType.Quest;
            title = END_QUEST_TITLE;
            
            Choices.Add(new ChoiceEditorData(){ Text = "Output"});
            
            mainContainer.AddToClassList("ds-quest_node__main-container");
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
            
            var quest = DialogueElementUtility.CreateCustomField(Quest, "Quest: ", typeof(QuestSO),
                callback => Quest = (QuestSO)callback.newValue);
            
            var startOrEnd = DialogueElementUtility.CreateToggle("Rozpocznij Quest", IsQuestStart,
                callback =>
                {
                    IsQuestStart = callback.newValue;
                    title = IsQuestStart ? START_QUEST_TITLE : END_QUEST_TITLE;
                }
            );
            
            extensionContainer.Add(startOrEnd);
            extensionContainer.Add(quest);
            
            RefreshExpandedState();
        }

        public override NodeEditorData GetSavedData()
        {
            QuestNodeEditorData nodeData = new QuestNodeEditorData()
            {
                ID = this.ID,
                GroupID = this.GroupID.ToString(),
                NodeType = this.NodeType,
                Position = this.GetPosition().position,

                Quest = this.Quest,
                IsQuestStart = this.IsQuestStart
            };

            return nodeData;
        }

        public override void Load(NodeEditorData nodeData)
        {
            var nData =  nodeData as QuestNodeEditorData;

            Quest = nData.Quest;
            IsQuestStart = nData.IsQuestStart;
            
            title = IsQuestStart ? START_QUEST_TITLE : END_QUEST_TITLE;
        }

        public override NodeRuntimeData GetSavedDataForDialogue()
        {
            var convertedChoices = Choices.Select(choice => choice.ConvertToChoiceData()).ToList();

            QuestRuntimeData nodeSaveData = new QuestRuntimeData
            {
                NodeId = ID,
                Quest = Quest,
                NodeType = NodeType,
                IsStartingDialogue = IsStartingNode(),
                Choices = convertedChoices,
                IsQuestStart = IsQuestStart
            };
            
            return nodeSaveData;
        }
    }
}
