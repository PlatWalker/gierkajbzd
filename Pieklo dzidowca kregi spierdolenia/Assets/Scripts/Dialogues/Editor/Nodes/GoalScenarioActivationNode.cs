using System.Linq;
using jbzd.Dialogues.Editor.Save;
using jbzd.Dialogues.Editor.Utilities;
using jbzd.Dialogues.RuntimeData;
using jbzd.QuestSystem.QuestStructureElements;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace jbzd.Dialogues.Editor.Nodes
{
    public class GoalScenarioActivationNode : BasicNode
    {
        public GoalSO Goal { get; set; }
        
        public override void Initialize(Vector2 position, DialogueGraphView graphView)
        {
            base.Initialize(position, graphView);
            
            NodeType = NodeType.GoalScenarioActivation;
            title = "Aktywuj scenariusz goal'a";
            
            Choices.Add(new ChoiceEditorData(){ Text = "Output"});
            
            mainContainer.AddToClassList("ds-goal_node__main-container");
        }
    
        public override void Draw()
        {
            var inputPort = this.CreatePort("Input", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);
            
            inputContainer.Add(inputPort);

            var choice = Choices.First();
            var outputPort = this.CreatePort(choice.Text);
            outputPort.userData = choice;
            
            outputContainer.Add(outputPort);
            
            var quest = DialogueElementUtility.CreateCustomField(Goal, "Goal: ", typeof(GoalSO),
                callback => Goal = (GoalSO)callback.newValue);
            
            extensionContainer.Add(quest);
            
            RefreshExpandedState();
        }
    
        public override NodeEditorData GetSavedData()
        {
            var nodeData = new GoalScenarioActivationNodeEditorData
            {
                ID = ID,
                GroupID = GroupID.ToString(),
                NodeType = NodeType,
                Position = GetPosition().position,
                
                Goal = Goal
            };

            return nodeData;
        }
    
        public override void Load(NodeEditorData nodeData)
        {
            var nData =  nodeData as GoalScenarioActivationNodeEditorData;

            Goal = nData?.Goal;
        }
    
        public override NodeRuntimeData GetSavedDataForDialogue()
        {
            var convertedChoices = Choices.Select(choice => choice.ConvertToChoiceData()).ToList();

            var nodeSaveData = new GoalScenarioActivationNodeRuntimeData
            {
                NodeId = ID,
                Goal = Goal,
                NodeType = NodeType,
                IsStartingDialogue = IsStartingNode(),
                Choices = convertedChoices
            };
            
            return nodeSaveData;
        }
    }
}