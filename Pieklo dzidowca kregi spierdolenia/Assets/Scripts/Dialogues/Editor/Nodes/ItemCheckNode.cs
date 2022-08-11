using System;
using UnityEngine;
using jbzdy.Items;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using jbzdy.DialogueSystem.NodeDatas;
using jbzdy.DialogueSystem.Editor;
using UnityEditor.Experimental.GraphView;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace jbzdy.DialogueSystem.Nodes
{
    public class ItemCheckNode : BaseNode
    {
        public override bool AutoDrawOutputEdges { get; } = false;
        private SharItem nodeItem;
        private string itemCheckValue;

        private ObjectField itemField;
        private TextField itemCheckValueField;

        public String NegativeResultGuid { get; private set; }
        public String PositiveResultGuid { get; private set; }
        public SharItem NodeItem { get => nodeItem; set => nodeItem = value; }
        public string ItemCheckValue { get => itemCheckValue; set => itemCheckValue = value; }

        public ItemCheckNode()
        {

        }

        public ItemCheckNode(Vector2 position, DialogueEditorWindow newEditorWindow, DialogueGraphView newGraphView)
        {
            StyleSheet styleSheet = Resources.Load<StyleSheet>("ItemNodeStyleSheet");
            styleSheets.Add(styleSheet);

            editorWindow = newEditorWindow;
            graphView = newGraphView;

            title = "Does Player have item?";
            SetPosition(new Rect(position, defaultNodeSize));
            NodeGuid = Guid.NewGuid().ToString();

            AddInputPort("Input", Port.Capacity.Multi);
            AddOutputPort("Yes", Port.Capacity.Single);
            AddOutputPort("No", Port.Capacity.Single);

            itemCheckValueField = new TextField()
            {
                label = "Item count",
                value = "1"
            };
            itemCheckValue = "1";

            itemCheckValueField.RegisterValueChangedCallback(value =>
            {
                if(!Regex.IsMatch(value.newValue, @"^\d+$"))
                {
                    this.itemCheckValueField.value = value.previousValue;
                }
                itemCheckValue = value.newValue;
            });
            itemCheckValueField.SetValueWithoutNotify(itemCheckValue);
            mainContainer.Add(itemCheckValueField);

            itemField = new ObjectField()
            {
                objectType = typeof(SharItem),
                allowSceneObjects = false,

                value = nodeItem,
            };

            itemField.RegisterValueChangedCallback(value =>
            {
                nodeItem = itemField.value as SharItem;
            });

            itemField.SetValueWithoutNotify(nodeItem);
            mainContainer.Add(itemField);
        }

        public override void LoadValueInToField()
        {
            itemCheckValueField.SetValueWithoutNotify(itemCheckValue);
            itemField.SetValueWithoutNotify(nodeItem);
        }

        public override bool DrawNode(DialogueEditorWindow editorWindow, DialogueGraphView graphView, Vector2 graphMousePosition)
        {
            graphView.AddElement(new ItemCheckNode(graphMousePosition, editorWindow, graphView));
            return true;
        }

        public override BaseNodeData GetDataToSave()
        {
            string positiveOutput = "";
            string negativeOutput = "";

            Edge[] connectedEdges = graphView.edges.ToList().FindAll(edge => edge.output.node.Equals(this)).ToArray();

            if (connectedEdges.Length != 2)
            {
                Debug.Log("Podczas zapisu ItemCheck Dialogue Node napotkano inną ilość stanów wyjściowych niż 2!");
            }

            foreach (Edge edge in connectedEdges)
            {
                switch (edge.output.portName)
                {
                    case "Yes":
                        positiveOutput = ((BaseNode)edge.input.node).NodeGuid;
                        break;
                    case "No":
                        negativeOutput = ((BaseNode)edge.input.node).NodeGuid;
                        break;
                    default:
                        Debug.Log("Podczas zapisu ItemCheck Dialogue Node napotkano port o nieodpowiedniej nazwie");
                        break;
                }
            }
            return new ItemCheckNodeData()
            {
                ItemCheckValue = Int32.Parse(ItemCheckValue),
                NegativeResultGuid = negativeOutput,
                PositiveResultGuid = positiveOutput,
                NodeGuid = NodeGuid,
                NodeItem = NodeItem,
                Position = GetPosition().position,
            };
        }

        public override void LoadDataIntoNode(BaseNodeData dataToLoad)
        {
            if(dataToLoad is not ItemCheckNodeData)
            {
                Debug.Log("Podano błędne dane do node");
                return;
            }
            ItemCheckNodeData newData = (ItemCheckNodeData)dataToLoad;
            itemCheckValue = newData.ItemCheckValue.ToString();
            NodeGuid = newData.NodeGuid;
            NodeItem = newData.NodeItem;
            PositiveResultGuid = newData.PositiveResultGuid;
            NegativeResultGuid = newData.NegativeResultGuid;
            SetPosition(new Rect(newData.Position, defaultNodeSize));
            LoadValueInToField();
        }

        public override BaseNode CreateNewNode(DialogueEditorWindow newEditorWindow, DialogueGraphView newGraphView)
        {
            return new ItemCheckNode(Vector2.zero, newEditorWindow, newGraphView);
        }

        public override void LinkToOtherNodes(List<BaseNode> allNodes)
        {
            BaseNode positiveNode = allNodes.Find(node => node.NodeGuid.Equals(PositiveResultGuid));
            BaseNode negativeNode = allNodes.Find(node => node.NodeGuid.Equals(NegativeResultGuid));

            if (positiveNode is not null)
                graphView.Add(MakeNewEdge((Port)outputContainer[0], (Port)positiveNode.inputContainer[0]));
            if (negativeNode is not null)
                graphView.Add(MakeNewEdge((Port)outputContainer[1], (Port)negativeNode.inputContainer[0]));
        }
    }
}


