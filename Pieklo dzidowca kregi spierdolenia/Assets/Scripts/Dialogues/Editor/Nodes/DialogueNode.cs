using jbzdy.DialogueSystem.Editor;
using jbzdy.DialogueSystem.NodeDatas;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace jbzdy.DialogueSystem.Nodes
{
    public class DialogueNode : BaseNode
    {
        public override bool AutoDrawOutputEdges { get; } = false;
        private List<DialogueNodePort> dialogueNodePorts = new List<DialogueNodePort>();
        public string Text { get; set; }
        public AudioClip AudioClip { get; set; }
        public Sprite NpcFaceImage { get; set; }
        public Sprite PlayerFaceImage { get; set; }
        public string NameText { get; set; }
        public List<DialogueNodePort> DialogueNodePorts { get => dialogueNodePorts; set => dialogueNodePorts = value; }

        private TextField textField;
        private ObjectField audioClipField;
        private ObjectField npcImageField;
        private ObjectField playerImageField;
        private TextField nameField;

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
            NodeGuid = Guid.NewGuid().ToString();

            AddInputPort("Input", Port.Capacity.Multi);


            //Enemy Face Image
            npcImageField = new ObjectField
            {
                label = "NPC image: ",
                objectType = typeof(Sprite),
                allowSceneObjects = false,
                value = NpcFaceImage
            };
            npcImageField.RegisterValueChangedCallback(value =>
            {
                NpcFaceImage = value.newValue as Sprite;
            });            
            
            mainContainer.Add(npcImageField);
            
            playerImageField = new ObjectField
            {
                label = "Player image: ",
                objectType = typeof(Sprite),
                allowSceneObjects = false,
                value = PlayerFaceImage
            };
            playerImageField.RegisterValueChangedCallback(value =>
            {
                PlayerFaceImage = value.newValue as Sprite;
            });   
            
            mainContainer.Add(playerImageField);

            // Audio Clip
            audioClipField = new ObjectField()
            {
                objectType = typeof(AudioClip),
                allowSceneObjects = false,
                value = AudioClip,
            };
            audioClipField.RegisterValueChangedCallback(value =>
            {
                AudioClip = value.newValue as AudioClip;
            });
            audioClipField.SetValueWithoutNotify(AudioClip);
            mainContainer.Add(audioClipField);

            // Text Name
            Label label_name = new Label("Name");
            label_name.AddToClassList("label_name");
            label_name.AddToClassList("Label");
            mainContainer.Add(label_name);

            nameField = new TextField("");
            nameField.RegisterValueChangedCallback(value =>
            {
                NameText = value.newValue;
            });
            nameField.SetValueWithoutNotify(NameText);
            nameField.AddToClassList("TextName");

            mainContainer.Add(nameField);

            // Text Box
            Label label_texts = new Label("Text Box");
            label_texts.AddToClassList("label_texts");
            label_texts.AddToClassList("Label");
            mainContainer.Add(label_texts);

            textField = new TextField("");
            textField.RegisterValueChangedCallback(value =>
            {
                Text = value.newValue;
            });
            textField.SetValueWithoutNotify(Text);
            textField.multiline = true;
            textField.style.height = 50;
            textField.style.width = 600;
            

            textField.AddToClassList("TextBox");
            mainContainer.Add(textField);

            Button button = new Button()
            {
                text = "Add Choice"
            };
            button.clicked += () =>
            {
                AddChoicePort(this);
            };

            titleButtonContainer.Add(button);
            RegisterCallback<MouseDownEvent>((evt) =>
            {
                if (evt.button == 0 && evt.ctrlKey) {
                    DuplicateThisNode();
                }
            });
        }

        private void DuplicateThisNode()
        {
            DialogueNode duplicate = new DialogueNode(Vector2.zero, editorWindow, graphView);

            Vector2 newPosition = new Vector2(GetPosition().position.x + 50, GetPosition().position.y + 50);
            duplicate.SetPosition(new Rect(newPosition,defaultNodeSize));
            graphView.AddElement(duplicate);
            duplicate.NameText = NameText;
            duplicate.AudioClip = AudioClip;
            duplicate.NpcFaceImage = NpcFaceImage;
            duplicate.PlayerFaceImage = PlayerFaceImage;
            duplicate.Text = Text;
            duplicate.LoadValueInToField();
        }
        public override void LoadValueInToField()
        {
            textField.SetValueWithoutNotify(Text);
            audioClipField.SetValueWithoutNotify(AudioClip);
            npcImageField.SetValueWithoutNotify(NpcFaceImage);
            playerImageField.SetValueWithoutNotify(PlayerFaceImage);
            nameField.SetValueWithoutNotify(NameText);
        }

        public Port AddChoicePort(BaseNode newBaseNode, DialogueNodePort newDialogueNodePort = null)
        {
            Port port = GetPortInstance(Direction.Output);

            int outputPortCount = newBaseNode.outputContainer.Query("connector").ToList().Count();
            string outputPortName = $"Continue";

            DialogueNodePort dialogueNodePort = new DialogueNodePort
            {
                PortGuid = Guid.NewGuid().ToString()
            };

            if (newDialogueNodePort != null)
            {
                dialogueNodePort.InputGuid = newDialogueNodePort.InputGuid;
                dialogueNodePort.OutputGuid = newDialogueNodePort.OutputGuid;
                dialogueNodePort.PortGuid = newDialogueNodePort.PortGuid;
                dialogueNodePort.Text = newDialogueNodePort.Text;
            }

            // Text for the port
            dialogueNodePort.TextField = new TextField();
            dialogueNodePort.TextField.RegisterValueChangedCallback(value =>
            {
                dialogueNodePort.Text = value.newValue;
            });
            dialogueNodePort.TextField.SetValueWithoutNotify(dialogueNodePort.Text);
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

            delNode.RefreshPorts();
            delNode.RefreshExpandedState();
        }

        public override bool DrawNode(DialogueEditorWindow editorWindow, DialogueGraphView graphView, Vector2 graphMousePosition)
        {
            graphView.AddElement(new DialogueNode(graphMousePosition, editorWindow, graphView));
            return true;
        }

        public override BaseNodeData GetDataToSave()
        {
            DialogueNodeData dataToSave = new()
            {
                Position = GetPosition().position,
                NodeGuid = NodeGuid,
                Text = Text,
                AudioClip = AudioClip,
                npcSprite = NpcFaceImage,
                playerSprite = PlayerFaceImage,
                Name = NameText,
                DialogueNodePorts = new List<DialogueNodePort>(DialogueNodePorts)
            };

            foreach (DialogueNodePort nodePort in dataToSave.DialogueNodePorts)
            {
                nodePort.OutputGuid = string.Empty;
                nodePort.InputGuid = string.Empty;
                foreach (Edge edge in graphView.edges.ToList())
                {
                    if (edge.output == nodePort.MyPort)
                    {
                        nodePort.OutputGuid = (edge.output.node as BaseNode).NodeGuid;
                        nodePort.InputGuid = (edge.input.node as BaseNode).NodeGuid;
                    }
                }
            }
            return dataToSave;
        }

        public override void LoadDataIntoNode(BaseNodeData dataToLoad)
        {
            
            if (dataToLoad is not DialogueNodeData)
            {
                Debug.Log("Błędne dane otrzymane do node'a");
                return;
            }
            DialogueNodeData newData = (DialogueNodeData) dataToLoad;
            SetPosition(new Rect(newData.Position, defaultNodeSize));
            NodeGuid = newData.NodeGuid;
            Text = newData.Text;
            AudioClip = newData.AudioClip;
            NpcFaceImage = newData.npcSprite;
            PlayerFaceImage = newData.playerSprite;
            NameText = newData.Name;
            foreach (DialogueNodePort nodePort in newData.DialogueNodePorts)
            {
                AddChoicePort(this, nodePort);
            }

            LoadValueInToField();
        }

        public override BaseNode CreateNewNode(DialogueEditorWindow newEditorWindow, DialogueGraphView newGraphView)
        {
            return new DialogueNode(Vector2.zero, newEditorWindow, newGraphView);
        }

        public override void LinkToOtherNodes(List<BaseNode> allNodes)
        {
            foreach (DialogueNodePort nodePort in DialogueNodePorts)
            {
                if (nodePort.InputGuid != string.Empty)
                {
                    BaseNode targetNode = allNodes.First(Node => Node.NodeGuid == nodePort.InputGuid);
                    graphView.Add(MakeNewEdge(nodePort.MyPort, (Port)targetNode.inputContainer[0]));
                }
            }
        }
    }
}

