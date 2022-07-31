using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace jbzd.dialogues.editor
{
    public class DialogueGraphView : GraphView
    {
        private readonly DialogueEditorWindow _editorWindow;
        private SearchWindow _searchWindow;
        private readonly Dictionary<string, Type> _nodesByName;

        public DialogueGraphView(DialogueEditorWindow editorWindow)
        {
            _editorWindow = editorWindow;
            _nodesByName = new Dictionary<string, Type>();
        }

        public void Init()
        {
            AddManipulators();
            AddSearchWindow();
            AddGridBackground();

            AddStyles();
            
            var nodeTypes = Assembly.GetAssembly(typeof(BasicNode)).GetTypes()
                .Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(BasicNode)));

            foreach (var type in nodeTypes)
            {
                _nodesByName.Add(type.Name, type);
            }
        }

        private void AddManipulators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            
            this.AddManipulator(CreateNodeContextualMenu("Add Node (Single Choice)", DialogueType.SingleChoice));
            this.AddManipulator(CreateNodeContextualMenu("Add Node (Multiple Choice)", DialogueType.MultipleChoice));
            
            this.AddManipulator(CreateGroupContextualMenu());
        }
        
        private void AddSearchWindow()
        {
            if (_searchWindow == null)
            {
                _searchWindow = ScriptableObject.CreateInstance<SearchWindow>();
                
                _searchWindow.Initialize(this);
            }

            nodeCreationRequest = context =>
                UnityEditor.Experimental.GraphView.SearchWindow.Open(
                    new SearchWindowContext(context.screenMousePosition), _searchWindow);
        }
        
        private void AddGridBackground()
        {
            GridBackground gridBackground = new GridBackground();
            
            gridBackground.StretchToParentSize();
            
            Insert(0, gridBackground);
        }

        private void AddStyles()
        {
            this.AddStyleSheets(
                "GraphViewStyleSheet",
                "NodeStyleSheet"
            );

        }
        
        public Vector2 GetLocalMousePosition(Vector2 mousePosition, bool isSearchWindow = false)
        {
            Vector2 worldMousePosition = mousePosition;

            if (isSearchWindow)
            {
                worldMousePosition -= _editorWindow.position.position;
            }
            
            Vector2 localMousePosition = contentViewContainer.WorldToLocal(worldMousePosition);

            return localMousePosition;
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new List<Port>();

            ports.ForEach(port =>
            {
                if (startPort == port) return;

                if (startPort.node == port.node) return;

                if (startPort.direction == port.direction) return;
                
                compatiblePorts.Add(port);
            });
            
            return compatiblePorts;
        }

        public BasicNode CreateNode(Vector2 position, DialogueType dialogueType)
        {
            var typeName = $"{dialogueType}Node";
            
            if (_nodesByName.ContainsKey(typeName))
            {
                Type nodeType = _nodesByName[typeName];

                BasicNode node = (BasicNode)Activator.CreateInstance(nodeType);

                node.Initialize(position, this);
                node.Draw();
                
                return node;
            }

            Debug.LogError("Brak podanego typu node'a");
            return null;
        }
        
        private IManipulator CreateGroupContextualMenu()
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction("Add Group", 
                    actionEvent=>AddElement(CreateGroup(
                        GetLocalMousePosition(actionEvent.eventInfo.localMousePosition), "Grupa")))
            );

            return contextualMenuManipulator;
        }

        public Group CreateGroup(Vector2 localMousePosition, string title)
        {
            Group group = new Group()
            {
                title = title
            };
            
            group.SetPosition(new Rect(localMousePosition, Vector2.zero));

            return group;
        }

        private IManipulator CreateNodeContextualMenu(string actionTitle, DialogueType dialogueType)
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction(actionTitle, 
                    actionEvent=>AddElement(CreateNode(
                        GetLocalMousePosition(actionEvent.eventInfo.localMousePosition), dialogueType)))
            );

            return contextualMenuManipulator;
        }
    }
}
