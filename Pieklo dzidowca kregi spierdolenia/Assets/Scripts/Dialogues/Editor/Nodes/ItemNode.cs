using System.Collections.Generic;
using System.Linq;
using jbzd.Dialogues.Editor.Save;
using jbzd.Dialogues.Editor.Utilities;
using jbzd.Dialogues.RuntimeData;
using jbzd.Items;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace jbzd.Dialogues.Editor.Nodes
{
    public class ItemNode : BasicNode
    {
        private ItemSO Item { get; set; }
        private bool IsItemAdded { get; set; } = false;
        private int ItemsNumber { get; set; } = 1;

        private const string ADD_ITEM_TITLE = "Dodaj Item";
        private const string REMOVE_ITEM_TITLE = "Zabierz Item";
        private const string REMOVE_ITEM_OUTPUT_1 = "Gracz posiada item";
        private const string REMOVE_ITEM_OUTPUT_2 = "Gracz nie posiada itemu";
        private const string ADD_ITEM_OUTPUT_1 = "Output";
        private const string ADD_ITEM_OUTPUT_2 = "Opcja 2 na razie nie używana";

        private List<Port> outputs = new();

        public override void Initialize(Vector2 position, DialogueGraphView graphView)
        {
            base.Initialize(position, graphView);
            NodeType = NodeType.Item;
            title = REMOVE_ITEM_TITLE;
            
            Choices.Add(new ChoiceEditorData(){ Text = REMOVE_ITEM_OUTPUT_1});
            Choices.Add(new ChoiceEditorData(){ Text = REMOVE_ITEM_OUTPUT_2});
            
            mainContainer.AddToClassList("ds-item_node__main-container");
        }
        
        public override void Draw()
        {
            Port inputPort =
                this.CreatePort("Input", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);
            
            inputContainer.Add(inputPort);


            foreach (var choice in Choices)
            {
                Port outputPort = this.CreatePort(choice.Text);
                outputPort.userData = choice;

                outputContainer.Add(outputPort);
                outputs.Add(outputPort);
            }

            var item = DialogueElementUtility.CreateCustomField(Item, "Item: ", typeof(ItemSO),
                callback => Item = (ItemSO)callback.newValue);

            var giveOrRemove = DialogueElementUtility.CreateToggle("Dostań item", IsItemAdded,
                callback =>
                {
                    IsItemAdded = callback.newValue;

                    if (IsItemAdded)
                    {
                        title = ADD_ITEM_TITLE;
                        Choices[0].Text = ADD_ITEM_OUTPUT_1;
                        Choices[1].Text = ADD_ITEM_OUTPUT_2;
                        outputs[0].portName = ADD_ITEM_OUTPUT_1;
                        outputs[1].portName = ADD_ITEM_OUTPUT_2;
                    }
                    else
                    {
                        title = REMOVE_ITEM_TITLE;
                        Choices[0].Text = REMOVE_ITEM_OUTPUT_1;
                        Choices[1].Text = REMOVE_ITEM_OUTPUT_2;
                        outputs[0].portName = REMOVE_ITEM_OUTPUT_1;
                        outputs[1].portName = REMOVE_ITEM_OUTPUT_2;
                    }
                }
            );

            var itemNumber = DialogueElementUtility.CreateIntegerField("Ilość: ", ItemsNumber,
                callback => ItemsNumber = callback.newValue);
            
            extensionContainer.Add(giveOrRemove);
            extensionContainer.Add(itemNumber);
            extensionContainer.Add(item);
            
            RefreshExpandedState();
        }

        public override NodeEditorData GetSavedData()
        {
            ItemNodeEditorData nodeData = new ItemNodeEditorData()
            {
                ID = this.ID,
                GroupID = this.GroupID.ToString(),
                NodeType = this.NodeType,
                Position = this.GetPosition().position,

                Item = this.Item,
                ItemsNumber = this.ItemsNumber,
                IsItemAdded = this.IsItemAdded
            };

            return nodeData;
        }

        public override void Load(NodeEditorData nodeData)
        {
            var nData =  nodeData as ItemNodeEditorData;

            Item = nData.Item;
            ItemsNumber = nData.ItemsNumber;
            IsItemAdded = nData.IsItemAdded;
            
            title = IsItemAdded ? ADD_ITEM_TITLE : REMOVE_ITEM_TITLE;
        }

        public override NodeRuntimeData GetSavedDataForDialogue()
        {
            var convertedChoices = Choices.Select(choice => choice.ConvertToChoiceData()).ToList();

            ItemRuntimeData nodeSaveData = new ItemRuntimeData
            {
                NodeId = ID,
                Item = Item,
                NodeType = NodeType,
                IsStartingDialogue = IsStartingNode(),
                Choices = convertedChoices,
                ItemsNumber = ItemsNumber,
                IsItemAdded = IsItemAdded
            };

            return nodeSaveData;  
        }
    }
}
