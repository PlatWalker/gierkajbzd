using System.Collections.Generic;
using System.Linq;
using jbzd.Dialogues.Editor;
using jbzd.Dialogues.Editor.Save;
using jbzd.Dialogues.Editor.Utilities;
using jbzd.Dialogues.RuntimeData;
using NUnit.Framework;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace jbzd.Dialogues.Editor.Nodes
{
    public class DialogueNode : BasicNode
    {
        private readonly int textLengthLimit = int.MaxValue;
        private Label warning1;
        private Label warning2;
        private string Text { get; set; }
        private string placeholderText = "Tekst dialogu...";
        private bool IsPlayerTalking { get; set; }
        private Sprite NpcImage { get; set; }
        private Sprite PlayerImage { get; set; }
        private AudioClip DialogueAudio { get; set; }
        
        public override void Initialize(Vector2 position, DialogueGraphView graphView)
        {
            base.Initialize(position, graphView);
            RegisterCallback<KeyUpEvent>(OnWarningFromNode);
        }

        public override void Draw()
        {   
            Port inputPort =
                this.CreatePort("Input", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);

            inputContainer.Add(inputPort);

            var isPlayerTalking =
                DialogueElementUtility.CreateToggle("Czy gracz mowi: ", IsPlayerTalking, 
                    callback => IsPlayerTalking = callback.newValue);
            
            var npcImage = DialogueElementUtility.CreateCustomField(NpcImage, "obrazek NPC: ", typeof(Sprite),
                callback => NpcImage = (Sprite)callback.newValue);

            var playerImage = DialogueElementUtility.CreateCustomField(PlayerImage, "obrazek gracza: ", typeof(Sprite),
                callback => PlayerImage = (Sprite)callback.newValue);

            var dialogueAudio = DialogueElementUtility.CreateCustomField(DialogueAudio, "audio: ", typeof(AudioClip),
                callback => DialogueAudio = (AudioClip)callback.newValue);

            extensionContainer.Add(npcImage);
            extensionContainer.Add(playerImage);
            extensionContainer.Add(dialogueAudio);
            extensionContainer.Add(isPlayerTalking);

            VisualElement customDataContainer = new VisualElement();
            
            customDataContainer.AddToClassList("ds-node__custom-data-container");

            Foldout textFoldout = DialogueElementUtility.CreateFoldout("Tekst");


            TextField textField = DialogueElementUtility.CreateTextArea(Text, null, 
                callback => Text = callback.newValue);

            if(string.IsNullOrEmpty(Text))textField.value = placeholderText;

            textField.RegisterCallback<FocusInEvent>(evt =>
            {
                if (textField.value == placeholderText)
                {
                    textField.value = "";
                }
            });

            textField.RegisterCallback<FocusOutEvent>(evt =>
            {
                if (string.IsNullOrEmpty(textField.value))
                {
                    textField.value = placeholderText;
                }
            });
            
            textField.AddClasses(
                "ds-node__textfield",
                "ds-node__quote-textfield");

            warning1 =
                DialogueElementUtility.CreateReadOnlyText(
                    "Tekst jest za długi i będzie źle wyglądał w grze. Rozbij na więcej węzłów.");
            warning2 = 
                DialogueElementUtility.CreateReadOnlyText(
                    "Pole na tekst dialogu jest puste.");
            warning1.AddClasses("ds-node__warning");
            warning2.AddClasses("ds-node__warning");
            
            if(Text == null){
                textField.value = placeholderText;
                Text = placeholderText;
            }
            warning1.visible = Text.Length > textLengthLimit;
            warning2.visible = (string.IsNullOrEmpty(Text) || Text == placeholderText);

            textFoldout.Add(textField);
            customDataContainer.Add(textFoldout);
            extensionContainer.Add(customDataContainer);
            extensionContainer.Add(warning2);
            extensionContainer.Add(warning1);
            
            RefreshExpandedState();
        }

        private void OnWarningFromNode(KeyUpEvent e)
        {
            warning1.visible = Text.Length > textLengthLimit;
            warning2.visible = (string.IsNullOrEmpty(Text) || Text == placeholderText);
        }

        public bool CheckTextLength()
        {
            return Text.Length > textLengthLimit;
        }

        public override NodeEditorData GetSavedData()
        {
            DialogueNodeEditorData nodeEditorData = new DialogueNodeEditorData()
            {
                ID = this.ID.ToString(),
                GroupID = this.GroupID.ToString(),
                NodeType = this.NodeType,
                Position = this.GetPosition().position,
                
                Text = this.Text,
                PlayerImage = this.PlayerImage,
                NpcImage = this.NpcImage,
                Audio = this.DialogueAudio,
                IsPlayerTalking = this.IsPlayerTalking
            };
            return nodeEditorData;
        }

        public override void Load(NodeEditorData nodeData)
        {
            var nData =  nodeData as DialogueNodeEditorData;
            
            Text = nData.Text;
            PlayerImage = nData.PlayerImage;
            NpcImage = nData.NpcImage;
            DialogueAudio = nData.Audio;
            IsPlayerTalking = nData.IsPlayerTalking;
        }

        public override NodeRuntimeData GetSavedDataForDialogue()
        {
            var convertedChoices = Choices.Select(choice => choice.ConvertToChoiceData()).ToList();
            DialogueRuntimeData nodeSaveData = 
                new DialogueRuntimeData(Text, convertedChoices, NodeType, IsStartingNode(), NpcImage, PlayerImage, ID,
                    NodeType != NodeType.SingleChoice, IsPlayerTalking);
            return nodeSaveData;
        }
    }
}
