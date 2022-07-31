using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace jbzd.dialogues.editor
{
    public class BasicNode : Node
    {
        private Guid ID { get; set; }
        protected List<ChoiceSaveData> Choices { get; set; }
        private string Text { get; set; }
        protected DialogueType DialogueType { get; set; }
        protected DialogueGraphView GraphView;

        public virtual void Initialize(Vector2 position, DialogueGraphView graphView)
        {
            ID = Guid.NewGuid();
            title = "Dialog";
            Choices = new List<ChoiceSaveData>();
            Text = "Tekst dialogu...";

            GraphView = graphView;
            SetPosition(new Rect(position, Vector2.zero));
            
            mainContainer.AddToClassList("ds-node__main-container");
            extensionContainer.AddToClassList("ds-node__extension-container");

        }

        public virtual void Draw()
        {
            Port inputPort =
                this.CreatePort("Input", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);

            inputContainer.Add(inputPort);
            
            DrawAdditionalInput();

            VisualElement customDataContainer = new VisualElement();
            
            customDataContainer.AddToClassList("ds-node__custom-data-container");

            Foldout textFoldout = DialogueElementUtility.CreateFoldout("Tekst");

            TextField textField = DialogueElementUtility.CreateTextArea(Text);

            textField.AddClasses(
                "ds-node__textfield",
                "ds-node__quote-textfield");

            textFoldout.Add(textField);
            customDataContainer.Add(textFoldout);
            extensionContainer.Add(customDataContainer);
            
            RefreshExpandedState();
        }

        private void DrawAdditionalInput()
        {
            var npcImage = DialogueElementUtility.CreateCustomField("obrazek NPC: ", typeof(Sprite));

            var playerImage = DialogueElementUtility.CreateCustomField("obrazek gracza: ", typeof(Sprite));

            var dialogueAudio = DialogueElementUtility.CreateCustomField("audio: ", typeof(AudioClip));

            extensionContainer.Add(npcImage);
            extensionContainer.Add(playerImage);
            extensionContainer.Add(dialogueAudio);

        }
    }
}
