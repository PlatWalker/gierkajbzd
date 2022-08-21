using System;
using jbzd.Dialogues.Editor;
using jbzd.Dialogues.Editor.Nodes;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace jbzd.Dialogues.Editor.Utilities
{
    public static class DialogueElementUtility
    {
        public static TextField CreateTextField(string value = null, string label = null, 
            EventCallback<ChangeEvent<string>> onValueChanged = null)
        {
            TextField textField = new TextField()
            {
                value = value,
                label = label
            };

            if (onValueChanged != null)
            {
                textField.RegisterValueChangedCallback(onValueChanged);
            }

            return textField;
        }

        public static TextField CreateTextArea(string value = null, string label = null,
            EventCallback<ChangeEvent<string>> onValueChanged = null)
        {
            TextField textArea = CreateTextField(value, label, onValueChanged);

            textArea.multiline = true;

            return textArea;
        }
        
        public static Foldout CreateFoldout(string title = null, bool collapsed = false)
        {
            Foldout foldout = new Foldout()
            {
                text = title,
                value = !collapsed
            };

            return foldout;
        }

        public static Button CreateButton(string text, Action onClick = null)
        {
            Button button = new Button(onClick)
            {
                text = text
            };
            
            return button;
        }

        public static Port CreatePort(this BasicNode node, string portName = "",
            Orientation orientation = Orientation.Horizontal, Direction direction = Direction.Output, 
            Port.Capacity capacity = Port.Capacity.Single)
        {
            Port port = node.InstantiatePort(orientation, direction, capacity, typeof(bool));

            port.portName = portName;

            return port;
        }

        public static ObjectField CreateCustomField(Object value, string label, Type type, 
            EventCallback<ChangeEvent<Object>> onValueChanged = null)
        {
            var customField = new ObjectField
            {
                value = value,
                label = label,
                objectType = type,
                allowSceneObjects = false,
            };

            if (onValueChanged != null)
            {
                customField.RegisterValueChangedCallback(onValueChanged);
            }

            return customField;
        }
        
    }
}
