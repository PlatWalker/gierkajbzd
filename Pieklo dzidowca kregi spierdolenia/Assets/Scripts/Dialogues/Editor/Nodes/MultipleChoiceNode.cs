using jbzd.Dialogues.Editor;
using jbzd.Dialogues.Editor.Save;
using jbzd.Dialogues.Editor.Utilities;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace jbzd.Dialogues.Editor.Nodes
{
    public class MultipleChoiceNode : DialogueNode
    {
        public override void Initialize(Vector2 position, DialogueGraphView graphView)
        {
            base.Initialize(position, graphView);

            NodeType = NodeType.MultipleChoice;
            
            ChoiceSaveData choiceData = new ChoiceSaveData(){ Text = "Nowy wybor"};
            Choices.Add(choiceData);
        }

        public override void Draw()
        {
            base.Draw();

            Button addChoiceButton = DialogueElementUtility.CreateButton("Dodaj wybor", () =>
            {
                ChoiceSaveData choiceData = new ChoiceSaveData(){ Text = "Nowy wybor"};
                Choices.Add(choiceData);
                
                Port choicePort = CreateChoicePort(choiceData);

                outputContainer.Add(choicePort);
            });
            
            addChoiceButton.AddToClassList("ds-node__button");
            
            mainContainer.Insert(1, addChoiceButton);
            
            

            foreach (var choice in Choices)
            {
                Port choicePort = CreateChoicePort(choice);
                outputContainer.Add(choicePort);
            }
            
            RefreshExpandedState();
        }

        private Port CreateChoicePort(object userData)
        {
            Port choicePort = this.CreatePort();

            choicePort.userData = userData;
            ChoiceSaveData choiceData = (ChoiceSaveData)userData;

            Button deleteChoiceButton = DialogueElementUtility.CreateButton("X", () =>
            {
                if (Choices.Count == 1) return;

                if (choicePort.connected)
                {
                    GraphView.DeleteElements(choicePort.connections);
                }
                
                Choices.Remove(choiceData);
                
                GraphView.RemoveElement(choicePort);
            });
                
            deleteChoiceButton.AddToClassList("ds-node__button");

            TextField choiceTextField = DialogueElementUtility.CreateTextField(choiceData.Text, null,
                callback => { choiceData.Text = callback.newValue;});

            choiceTextField.AddClasses(
                "ds-node__textfield",
                "ds-node__choice-textfield",
                "ds-node__textfield_hidden");

            choicePort.Add(choiceTextField);
            choicePort.Add(deleteChoiceButton);

            return choicePort;
        }
    }
}