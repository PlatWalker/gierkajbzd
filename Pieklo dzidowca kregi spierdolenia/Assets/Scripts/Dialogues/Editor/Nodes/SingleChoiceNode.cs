using jbzd.Dialogues.Editor;
using jbzd.Dialogues.Editor.Save;
using jbzd.Dialogues.Editor.Utilities;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace jbzd.Dialogues.Editor.Nodes
{
    public class SingleChoiceNode : DialogueNode
    {
        public override void Initialize(Vector2 position, DialogueGraphView graphView)
        {
            base.Initialize(position, graphView);

            DialogueType = DialogueType.SingleChoice;

            ChoiceSaveData choiceData = new ChoiceSaveData(){ Text = "Nastepny dialog"};
            
            Choices.Add(choiceData);
        }

        public override void Draw()
        {
            base.Draw();

            foreach (var choice in Choices)
            {
                Port choicePort = this.CreatePort(choice.Text);
                
                choicePort.userData = choice;
                
                outputContainer.Add(choicePort);
            }
            
            RefreshExpandedState();
        }
    }
}
