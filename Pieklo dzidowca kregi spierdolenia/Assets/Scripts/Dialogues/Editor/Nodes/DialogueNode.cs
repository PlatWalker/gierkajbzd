using System.Collections.Generic;
using System.Linq;
using jbzd.Dialogues.Editor.Save;
using jbzd.Dialogues.Editor.Utilities;
using jbzd.Dialogues.RuntimeData;
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
        private List<Sprite> LeftAdditionalImages { get; set; } = new List<Sprite>();
        private List<Sprite> RigthAdditionalImages { get; set; } = new List<Sprite>();
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
                DialogueElementUtility.CreateToggle("Czy dany tekst jest wypowiadany przez gracza: ", IsPlayerTalking, 
                    callback => IsPlayerTalking = callback.newValue);
            
            var npcImage = DialogueElementUtility.CreateCustomField(NpcImage, "obrazek NPC: ", typeof(Sprite),
                callback => NpcImage = (Sprite)callback.newValue);

            var playerImage = DialogueElementUtility.CreateCustomField(PlayerImage, "obrazek gracza: ", typeof(Sprite),
                callback => PlayerImage = (Sprite)callback.newValue);

            Foldout imagesFoldout = DialogueElementUtility.CreateFoldout("Dodatkowe obrazki", true);
            Box rigthImagesBox = new Box();
            Label rigthImagesLabel = DialogueElementUtility.CreateReadOnlyText("Obrazki prawe");
            Button addRigthChoiceButton = DialogueElementUtility.CreateButton("Dodaj", () =>
            {
                AddImageChoice(RigthAdditionalImages, rigthImagesBox);
            });
            Button removeRigthChoiceButton = DialogueElementUtility.CreateButton("Usuń ostatni", () =>
            {
                RigthAdditionalImages.RemoveAt(RigthAdditionalImages.Count-1);
                rigthImagesBox.RemoveAt(rigthImagesBox.childCount - 1);
            });


            Box leftImagesBox = new Box();
            Label leftImagesLabel = DialogueElementUtility.CreateReadOnlyText("Obrazki lewe");
            Button addLeftChoiceButton = DialogueElementUtility.CreateButton("Dodaj", () =>
            {
                AddImageChoice(LeftAdditionalImages, leftImagesBox);
            });
            Button removeLeftChoiceButton = DialogueElementUtility.CreateButton("Usuń ostatni", () =>
            {
                LeftAdditionalImages.RemoveAt(LeftAdditionalImages.Count - 1);
                leftImagesBox.RemoveAt(leftImagesBox.childCount - 1);
            });

            var dialogueAudio = DialogueElementUtility.CreateCustomField(DialogueAudio, "audio: ", typeof(AudioClip),
                callback => DialogueAudio = (AudioClip)callback.newValue);

            rigthImagesBox.Add(rigthImagesLabel);
            rigthImagesBox.Add(addRigthChoiceButton);
            rigthImagesBox.Add(removeRigthChoiceButton);
            leftImagesBox.Add(leftImagesLabel);
            leftImagesBox.Add(addLeftChoiceButton);
            leftImagesBox.Add(removeLeftChoiceButton);
            imagesFoldout.Add(rigthImagesBox);
            imagesFoldout.Add(leftImagesBox);

            int RigthAdditionalImagesCount = RigthAdditionalImages.Count;
            for (int i = 0; i < RigthAdditionalImagesCount; i++)
            {
                AddImageChoice(RigthAdditionalImages, rigthImagesBox, RigthAdditionalImages[i]);
            }

            int LeftAdditionalImagesCount = LeftAdditionalImages.Count;
            for (int i = 0; i < LeftAdditionalImagesCount; i++)
            {
                AddImageChoice(LeftAdditionalImages, leftImagesBox, LeftAdditionalImages[i]);
            }

            extensionContainer.Add(npcImage);
            extensionContainer.Add(playerImage);
            extensionContainer.Add(imagesFoldout);
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

        private void AddImageChoice(List<Sprite> additionalImages, Box imagesBox, Sprite sprite = null)
        {
            Sprite newImage = sprite;
            if( sprite is null )
                additionalImages.Add(newImage);
            var additionalImage = DialogueElementUtility.CreateCustomField(newImage, "obrazek: ", typeof(Sprite),
            callback => {
                var imageIndex = additionalImages.FindIndex(image => image == newImage);
                additionalImages[imageIndex] = (Sprite)callback.newValue;
            });

            imagesBox.Add(additionalImage);            
        } 

        private void OnWarningFromNode(KeyUpEvent e)
        {
            warning1.visible = CheckTextLength();
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
                RigthAdditionalImages = this.RigthAdditionalImages,
                LeftAdditionalImages = this.LeftAdditionalImages,
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
            RigthAdditionalImages = nData.RigthAdditionalImages;
            LeftAdditionalImages = nData.LeftAdditionalImages;
        }

        public override NodeRuntimeData GetSavedDataForDialogue()
        {
            var convertedChoices = Choices.Select(choice => choice.ConvertToChoiceData()).ToList();
            DialogueRuntimeData nodeSaveData = 
                new DialogueRuntimeData(Text, convertedChoices, NodeType, IsStartingNode(), NpcImage, PlayerImage, ID,
                    NodeType != NodeType.SingleChoice, IsPlayerTalking, LeftAdditionalImages, RigthAdditionalImages);
            return nodeSaveData;
        }

        public override bool IsRuleViolated()
        {
            WarningInfos.Clear();

            if (CheckTextLength())
                WarningInfos.Add("W niektórych węzłach tekst jest za długi i będzie źle wyglądał.");
            if(string.IsNullOrEmpty(Text) || Text == placeholderText)
                WarningInfos.Add("W niektórych węzłach pole na tekst dialogu jest puste.");

            return base.IsRuleViolated();
        }
    }
}
