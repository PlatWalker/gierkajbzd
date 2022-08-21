using jbzd.Dialogues.Editor;
using jbzd.Dialogues.Editor.Save;
using jbzd.Dialogues.Editor.Utilities;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace jbzd.Dialogues.Editor.Nodes
{
    public class DialogueNode : BasicNode
    {
        public string Text { get; set; }
        private Sprite NpcImage { get; set; }
        private Sprite PlayerImage { get; set; }
        private AudioClip DialogueAudio { get; set; }
        
        public override void Initialize(Vector2 position, DialogueGraphView graphView)
        {
            base.Initialize(position, graphView);
            Text = "Tekst dialogu...";
        }

        public override void Draw()
        {   
            Port inputPort =
                this.CreatePort("Input", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);

            inputContainer.Add(inputPort);
            
            var npcImage = DialogueElementUtility.CreateCustomField(NpcImage, "obrazek NPC: ", typeof(Sprite),
                callback => NpcImage = (Sprite)callback.newValue);

            var playerImage = DialogueElementUtility.CreateCustomField(PlayerImage, "obrazek gracza: ", typeof(Sprite),
                callback => PlayerImage = (Sprite)callback.newValue);

            var dialogueAudio = DialogueElementUtility.CreateCustomField(DialogueAudio, "audio: ", typeof(AudioClip),
                callback => DialogueAudio = (AudioClip)callback.newValue);

            extensionContainer.Add(npcImage);
            extensionContainer.Add(playerImage);
            extensionContainer.Add(dialogueAudio);

            VisualElement customDataContainer = new VisualElement();
            
            customDataContainer.AddToClassList("ds-node__custom-data-container");

            Foldout textFoldout = DialogueElementUtility.CreateFoldout("Tekst");

            TextField textField = DialogueElementUtility.CreateTextArea(Text, null, 
                callback => Text = callback.newValue);

            textField.AddClasses(
                "ds-node__textfield",
                "ds-node__quote-textfield");

            textFoldout.Add(textField);
            customDataContainer.Add(textFoldout);
            extensionContainer.Add(customDataContainer);
            
            RefreshExpandedState();
        }

        public override NodeSaveData GetSavedData()
        {
            DialogueNodeSaveData nodeSaveData = new DialogueNodeSaveData()
            {
                ID = this.ID.ToString(),
                GroupID = this.GroupID.ToString(),
                DialogueType = this.DialogueType,
                Position = this.GetPosition().position,
                
                Text = this.Text,
                PlayerImage = this.PlayerImage,
                NpcImage = this.NpcImage,
                Audio = this.DialogueAudio
            };
            return nodeSaveData;
        }

        public override void Load(NodeSaveData nodeData)
        {
            var nData =  nodeData as DialogueNodeSaveData;
            
            Text = nData.Text;
            PlayerImage = nData.PlayerImage;
            NpcImage = nData.NpcImage;
            DialogueAudio = nData.Audio;
            
        }
    }
}
