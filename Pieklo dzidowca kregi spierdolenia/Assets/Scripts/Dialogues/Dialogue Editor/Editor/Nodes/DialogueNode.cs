using System;
using System.Linq;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using jbzdy.DialogueSystem.SO;
using System.Collections.Generic;
using jbzdy.DialogueSystem.Enums;
using jbzdy.DialogueSystem.Editor;
using UnityEditor.Experimental.GraphView;

namespace jbzdy.DialogueSystem.Nodes
{
    public class DialogueNode : BaseNode
    {
        private List<LanguageGeneric<string>> texts = new List<LanguageGeneric<string>>();
        private List<LanguageGeneric<AudioClip>> audioClips = new List<LanguageGeneric<AudioClip>>();
        private Sprite faceImage;
        private string nameText = "";
        private DialogueFaceImageType faceImageType;

        private List<DialogueNodePort> dialogueNodePorts = new List<DialogueNodePort>();
        public List<LanguageGeneric<string>> Texts { get => texts; set => texts = value; }
        public List<LanguageGeneric<AudioClip>> AudioClips { get => audioClips; set => audioClips = value; }
        public Sprite FaceImage { get => faceImage; set => faceImage = value; }
        public string NameText { get => nameText; set => nameText = value; }
        public DialogueFaceImageType FaceImageType { get => faceImageType; set => faceImageType = value; }
        public List<DialogueNodePort> DialogueNodePorts { get => dialogueNodePorts; set => dialogueNodePorts = value; }

        private TextField texts_Field;
        private ObjectField audioClips_Field;
        private ObjectField faceImage_Field;
        private TextField name_Field;
        private EnumField faceImageType_Field;

        public DialogueNode()
        {
            
        }

        public DialogueNode(Vector2 position, DialogueEditorWindow newEditorWindow, DialogueGraphView newGraphView)
        {
            StyleSheet styleSheet = Resources.Load<StyleSheet>("DialogueNodeStyleSheet");
            styleSheets.Add(styleSheet);

            editorWindow = newEditorWindow;
            graphView = newGraphView;

            title = "Dialogue";
            SetPosition(new Rect(position, defaultNodeSize));
            nodeGuid = Guid.NewGuid().ToString();

            AddInputPort("Input", Port.Capacity.Multi);

            foreach (LanguageType language in (LanguageType[])Enum.GetValues(typeof(LanguageType)))
            {
                texts.Add(new LanguageGeneric<string>
                {
                    LanguageType = language,
                    LanguageGenericType = ""
                });

                audioClips.Add(new LanguageGeneric<AudioClip>
                {
                    LanguageType = language,
                    LanguageGenericType = null
                });
            }

            // Face Image
            faceImage_Field = new ObjectField
            {
                objectType = typeof(Sprite),
                allowSceneObjects = false,
                value = faceImage
            };
            faceImage_Field.RegisterValueChangedCallback(value =>
            {
                faceImage = value.newValue as Sprite;
            });
            mainContainer.Add(faceImage_Field);

            // Face Image Enum
            faceImageType_Field = new EnumField()
            {
                value = faceImageType
            };
            faceImageType_Field.Init(faceImageType);
            faceImageType_Field.RegisterValueChangedCallback(value =>
            {
                faceImageType = (DialogueFaceImageType)value.newValue;
            });
            mainContainer.Add(faceImageType_Field);

            // Audio Chilp
            audioClips_Field = new ObjectField()
            {
                objectType = typeof(AudioClip),
                allowSceneObjects = false,
                value = audioClips.Find(audioClip => audioClip.LanguageType == editorWindow.LanguageType).LanguageGenericType,
            };
            audioClips_Field.RegisterValueChangedCallback(value =>
            {
                audioClips.Find(audioClip => audioClip.LanguageType == editorWindow.LanguageType).LanguageGenericType = value.newValue as AudioClip;
            });
            audioClips_Field.SetValueWithoutNotify(audioClips.Find(audioClip => audioClip.LanguageType == editorWindow.LanguageType).LanguageGenericType);
            mainContainer.Add(audioClips_Field);

            // Text Name
            Label label_name = new Label("Name");
            label_name.AddToClassList("label_name");
            label_name.AddToClassList("Label");
            mainContainer.Add(label_name);

            name_Field = new TextField("");
            name_Field.RegisterValueChangedCallback(value =>
            {
                nameText = value.newValue;
            });
            name_Field.SetValueWithoutNotify(nameText);
            name_Field.AddToClassList("TextName");
            mainContainer.Add(name_Field);

            // Text Box
            Label label_texts = new Label("Text Box");
            label_texts.AddToClassList("label_texts");
            label_texts.AddToClassList("Label");
            mainContainer.Add(label_texts);

            texts_Field = new TextField("");
            texts_Field.RegisterValueChangedCallback(value =>
            {
                texts.Find(text => text.LanguageType == editorWindow.LanguageType).LanguageGenericType = value.newValue;
            });
            texts_Field.SetValueWithoutNotify(texts.Find(text => text.LanguageType == editorWindow.LanguageType).LanguageGenericType);
            texts_Field.multiline = true;

            texts_Field.AddToClassList("TextBox");
            mainContainer.Add(texts_Field);

            Button button = new Button()
            {
                text = "Add Choice"
            };
            button.clicked += () =>
            {
                AddChoicePort(this);
            };

            titleButtonContainer.Add(button);
        }

        public void ReloadLanguage()
        {
            texts_Field.RegisterValueChangedCallback(value =>
            {
                texts.Find(text => text.LanguageType == editorWindow.LanguageType).LanguageGenericType = value.newValue;
            });
            texts_Field.SetValueWithoutNotify(texts.Find(text => text.LanguageType == editorWindow.LanguageType).LanguageGenericType);

            audioClips_Field.RegisterValueChangedCallback(value =>
            {
                audioClips.Find(audioClip => audioClip.LanguageType == editorWindow.LanguageType).LanguageGenericType = value.newValue as AudioClip;
            });
            audioClips_Field.SetValueWithoutNotify(audioClips.Find(audioClip => audioClip.LanguageType == editorWindow.LanguageType).LanguageGenericType);

            foreach (DialogueNodePort nodePort in dialogueNodePorts)
            {
                nodePort.TextField.RegisterValueChangedCallback(value =>
                {
                    nodePort.TextLanguages.Find(language => language.LanguageType == editorWindow.LanguageType).LanguageGenericType = value.newValue;
                });
                nodePort.TextField.SetValueWithoutNotify(nodePort.TextLanguages.Find(language => language.LanguageType == editorWindow.LanguageType).LanguageGenericType);
            }
        }

        public override void LoadValueInToField()
        {
            texts_Field.SetValueWithoutNotify(texts.Find(language => language.LanguageType == editorWindow.LanguageType).LanguageGenericType);
            audioClips_Field.SetValueWithoutNotify(audioClips.Find(language => language.LanguageType == editorWindow.LanguageType).LanguageGenericType);
            faceImage_Field.SetValueWithoutNotify(faceImage);
            faceImageType_Field.SetValueWithoutNotify(faceImageType);
            name_Field.SetValueWithoutNotify(nameText);
        }

        public Port AddChoicePort(BaseNode newBaseNode, DialogueNodePort newDialogueNodePort = null)
        {
            Port port = GetPortInstance(Direction.Output);

            int outputPortCount = newBaseNode.outputContainer.Query("connector").ToList().Count();
            string outputPortName = $"Continue";

            DialogueNodePort dialogueNodePort = new DialogueNodePort();
            dialogueNodePort.PortGuid = Guid.NewGuid().ToString();

            foreach (LanguageType language in (LanguageType[])Enum.GetValues(typeof(LanguageType)))
            {
                dialogueNodePort.TextLanguages.Add(new LanguageGeneric<string>()
                {
                    LanguageType = language,
                    LanguageGenericType = outputPortName
                });
            }

            if (newDialogueNodePort != null)
            {
                dialogueNodePort.InputGuid = newDialogueNodePort.InputGuid;
                dialogueNodePort.OutputGuid = newDialogueNodePort.OutputGuid;
                dialogueNodePort.PortGuid = newDialogueNodePort.PortGuid;

                foreach (LanguageGeneric<string> languageGeneric in newDialogueNodePort.TextLanguages)
                {
                    dialogueNodePort.TextLanguages.Find(language => language.LanguageType == languageGeneric.LanguageType).LanguageGenericType = languageGeneric.LanguageGenericType;
                }
            }

            // Text for the port
            dialogueNodePort.TextField = new TextField();
            dialogueNodePort.TextField.RegisterValueChangedCallback(value =>
            {
                dialogueNodePort.TextLanguages.Find(language => language.LanguageType == editorWindow.LanguageType).LanguageGenericType = value.newValue;
            });
            dialogueNodePort.TextField.SetValueWithoutNotify(dialogueNodePort.TextLanguages.Find(language => language.LanguageType == editorWindow.LanguageType).LanguageGenericType);
            port.contentContainer.Add(dialogueNodePort.TextField);

            // Delete button
            Button deleteButton = new Button(() => DeletePort(newBaseNode, port))
            {
                text = "X",
            };
            port.contentContainer.Add(deleteButton);


            dialogueNodePort.MyPort = port;
            port.portName = "";

            dialogueNodePorts.Add(dialogueNodePort);

            newBaseNode.outputContainer.Add(port);

            // Refresh
            newBaseNode.RefreshPorts();
            newBaseNode.RefreshExpandedState();

            return port;
        }


        private void DeletePort(BaseNode delNode, Port delPort)
        {
            DialogueNodePort tmp = dialogueNodePorts.Find(port => port.MyPort == delPort);
            dialogueNodePorts.Remove(tmp);

            IEnumerable<Edge> portEdge = graphView.edges.ToList().Where(edge => edge.output == delPort);

            if (portEdge.Any())
            {
                Edge edge = portEdge.First();
                edge.input.Disconnect(edge);
                edge.output.Disconnect(edge);
                graphView.RemoveElement(edge);
            }

            delNode.outputContainer.Remove(delPort);

            // Refresh
            delNode.RefreshPorts();
            delNode.RefreshExpandedState();
        }
    }
}

