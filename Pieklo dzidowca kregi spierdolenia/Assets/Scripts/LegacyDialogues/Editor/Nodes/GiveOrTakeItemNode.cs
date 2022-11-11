using System;
using System.Text.RegularExpressions;
using jbzd.Items.Legacy.Inventory.Inventory;
using jbzd.LegacyDialogues.NodesDatas;
using jbzdy.Items;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace jbzd.LegacyDialogues.Editor.Nodes
{
    public class GiveOrTakeItemNode : BaseNode
    {
        private SharItem nodeItem;
        private ItemCheckNodeType itemCheckType;
        private string itemCheckValue;

        private EnumField itemCheckField;
        private ObjectField itemField;
        private TextField itemCheckValueField;

        public SharItem NodeItem { get => nodeItem; set => nodeItem = value; }
        public string ItemCheckValue { get => itemCheckValue; set => itemCheckValue = value; }
        public ItemCheckNodeType ItemCheckNodeType { get => itemCheckType; set => itemCheckType = value; }

        public GiveOrTakeItemNode()
        {
            
        }

        public GiveOrTakeItemNode(Vector2 position, DialogueEditorWindow newEditorWindow, DialogueGraphView newGraphView)
        {
            StyleSheet styleSheet = Resources.Load<StyleSheet>("ItemNodeStyleSheet");
            styleSheets.Add(styleSheet);

            editorWindow = newEditorWindow;
            graphView = newGraphView;

            title = "Give or Take Item";
            SetPosition(new Rect(position, defaultNodeSize));
            NodeGuid = Guid.NewGuid().ToString();

            AddInputPort("Input", Port.Capacity.Multi);
            AddOutputPort("Output", Port.Capacity.Single);

            itemCheckField = new EnumField()
            {
                value = itemCheckType
            };

            itemCheckField.Init(itemCheckType);

            itemCheckField.RegisterValueChangedCallback((value) =>
            {
                itemCheckType = (ItemCheckNodeType)value.newValue;
            });
            itemCheckField.SetValueWithoutNotify(itemCheckType);

            mainContainer.Add(itemCheckField);

            itemCheckValueField = new TextField()
            {
                label = "Item count",
                value = "1"
            };
            itemCheckValue = "1";

            itemCheckValueField.RegisterValueChangedCallback(value =>
            {
                if (!Regex.IsMatch(value.newValue, @"^\d+$"))
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
            itemCheckField.SetValueWithoutNotify(itemCheckType);
            itemField.SetValueWithoutNotify(nodeItem);
        }
        public override bool DrawNode(DialogueEditorWindow editorWindow, DialogueGraphView graphView, Vector2 graphMousePosition)
        {
            graphView.AddElement(new GiveOrTakeItemNode(graphMousePosition, editorWindow, graphView));
            return true;
        }

        public override BaseNodeData GetDataToSave()
        {
            return new GiveOrTakeItemNodeData()
            {
                ItemCheckType = ItemCheckNodeType,
                NodeGuid = NodeGuid,
                Position = GetPosition().position,
                ItemCheckValue = Int32.Parse(ItemCheckValue),
                NodeItem = NodeItem
            };
        }

        public override void LoadDataIntoNode(BaseNodeData dataToLoad)
        {
            if (dataToLoad is not GiveOrTakeItemNodeData)
            {
                Debug.Log("Podano błędne dane do node");
                return;
            }
            GiveOrTakeItemNodeData newData = (GiveOrTakeItemNodeData)dataToLoad;
            ItemCheckNodeType = newData.ItemCheckType;
            NodeGuid = newData.NodeGuid;
            SetPosition(new Rect(newData.Position, defaultNodeSize));
            itemCheckValue = newData.ItemCheckValue.ToString();
            NodeItem = newData.NodeItem;
            LoadValueInToField();
        }

        public override BaseNode CreateNewNode(DialogueEditorWindow newEditorWindow, DialogueGraphView newGraphView)
        {
            return new GiveOrTakeItemNode(Vector2.zero, newEditorWindow, newGraphView);
        }
    }
}

