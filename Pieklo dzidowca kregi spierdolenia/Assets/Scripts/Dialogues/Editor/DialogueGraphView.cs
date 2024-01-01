using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using jbzd.Dialogues.Editor.Nodes;
using jbzd.Dialogues.Editor.Save;
using jbzd.Dialogues.Editor.Utilities;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace jbzd.Dialogues.Editor
{
    public class DialogueGraphView : GraphView, IValidate
    {
        private readonly DialogueEditorWindow _editorWindow;
        private SearchWindow _searchWindow;
        private readonly Dictionary<string, Type> _nodesByName;
        public ObservableCollection<DialogueGroup> Groups { get; set; }
        public List<string> WarningInfos { get; set; } = new();

        public DialogueGraphView(DialogueEditorWindow editorWindow)
        {
            _editorWindow = editorWindow;
            _nodesByName = new Dictionary<string, Type>();
            Groups = new ObservableCollection<DialogueGroup>();
        }

        public void Init()
        {
            AddManipulators();
            AddSearchWindow();
            AddGridBackground();
            
            OnGraphViewChanged();

            AddStyles();
            
            var nodeTypes = Assembly.GetAssembly(typeof(BasicNode)).GetTypes()
                .Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(BasicNode)));

            foreach (var type in nodeTypes)
            {
                _nodesByName.Add(type.Name, type);
            }
        }

        public StartGroup CreateStartGroup(Vector2 position = default, Guid id = default)
        {
            StartGroup group = new StartGroup(this, id);
            group.SetPosition(new Rect(position, Vector2.zero));
       
            var node = CreateNode(new Vector2(0, 0), NodeType.EndGroup);
            
            group.AddElement(node);
            AddElement(group);
            AddElement(node);
            
            node.inputContainer.RemoveAt(0);
            
            group.SetEnabled(false);
            
            Groups.Add(group);

            return group;
        }

        private void AddManipulators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            
            this.AddManipulator(CreateNodeContextualMenu("Add Node (Single Choice)", NodeType.SingleChoice));
            this.AddManipulator(CreateNodeContextualMenu("Add Node (Multiple Choice)", NodeType.MultipleChoice));
            this.AddManipulator(CreateNodeContextualMenu("Add Node (End Group)", NodeType.EndGroup));
            
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

        public BasicNode CreateNode(Vector2 position, NodeType nodeType, bool shouldDraw = true)
        {
            var typeName = $"{nodeType}Node";
            
            if (_nodesByName.ContainsKey(typeName))
            {
                Type type = _nodesByName[typeName];

                BasicNode node = (BasicNode)Activator.CreateInstance(type);

                node.Initialize(position, this);
                
                if(shouldDraw)
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

        public DialogueGroup CreateGroup(Vector2 localMousePosition, string title)
        {
            title = AvailableGroupTitle(title);
            
            DialogueGroup group = new DialogueGroup(this)
            {
                title = title
            };
            
            group.SetPosition(new Rect(localMousePosition, Vector2.zero));

            Groups.Add(group);

            return group;
        }

        public string AvailableGroupTitle(string title, bool exists = false)
        {

            var tempTitle = title;
            var number = 0;

            if (exists)
            {
                if (Groups.Count(x => x.title == tempTitle) == 1)
                    return title;
            }
            
            while (Groups.Any(x => x.title == tempTitle))
            {
                tempTitle = title+number;
                number++;
            }

            return tempTitle;
        }

        private IManipulator CreateNodeContextualMenu(string actionTitle, NodeType nodeType)
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction(actionTitle, 
                    actionEvent=>AddElement(CreateNode(
                        GetLocalMousePosition(actionEvent.eventInfo.localMousePosition), nodeType)))
            );

            return contextualMenuManipulator;
        }

        private void OnGraphViewChanged()
        {
            graphViewChanged = (changes) =>
            {

                if (changes.edgesToCreate != null)
                {
                    foreach (var edge in changes.edgesToCreate)
                    {
                        var nextNode = (BasicNode)edge.input.node;

                        var choiceData = (ChoiceEditorData)edge.output.userData;

                        choiceData.NodeID = nextNode.ID;
                    }
                }

                if (changes.elementsToRemove != null)
                {
                    var edgeType = typeof(Edge);
                    var groupType = typeof(DialogueGroup);

                    foreach (var element in changes.elementsToRemove)
                    {
                        if (element.GetType() == edgeType)
                        {
                            var edge = (Edge)element;
                            var choiceData = (ChoiceEditorData)edge.output.userData;
                            choiceData.NodeID = null;
                        }

                        if (element.GetType() == groupType)
                        {
                            Groups.Remove((DialogueGroup)element);
                        }
                    }
                }

                return changes;
            };
        }

        public bool IsRuleViolated()
        {
            WarningInfos.Clear();
            var basicNodes = nodes.OfType<BasicNode>().ToList();

            if (basicNodes.Any(node => node.Choices.Any(choice => basicNodes.Where(x => x.ID == choice.NodeID).Any(x => x.GroupID != node.GroupID))))
            {
                WarningInfos.Add("Jeden z węzłów jest połączony z węzłem z innej grupy.");
            }

            if (Groups.Any(x => x.Nodes.Count(node => node.IsStartingNode()) > 1))
            {
                WarningInfos.Add("W grupie znajduje się więcej niż 1 węzęł rozpoczynający.");
            }

            if (nodes.OfType<EndGroupNode>().Any(node => !Groups.Any(group => group.name == node.SelectedGroup)))
            {
                WarningInfos.Add("End node wskazuje na nieistniejącą grupę.");
            }

            var visualElements = nodes.Concat<VisualElement>(Groups);

            for (var i = 0 ; i < visualElements.Count() ;i++)
            {
                var visualElement = visualElements.ElementAt(i).worldBound;
                var x = visualElement.x;
                var y = visualElement.y;
                var xMax = visualElement.xMax;
                var yMax = visualElement.yMax;

                for (var j = 0; j < visualElements.Count(); j++)
                {
                    if (i == j) continue;

                    if(visualElements.ElementAt(i) is BasicNode node)
                    {
                        if(visualElements.ElementAt(j) is DialogueGroup group)
                        {
                            if (node.GroupID == group.ID) continue;
                        }
                    }

                    var secondElement = visualElements.ElementAt(j).worldBound;
                    var x2 = secondElement.x;
                    var y2 = secondElement.y;
                    var xMax2 = secondElement.xMax;
                    var yMax2 = secondElement.yMax;

                    if (x>x2&&y>y2&&xMax<xMax2&&yMax<yMax2) {
                        WarningInfos.Add("Jeden z elementów jest przysłonięty i przez to niewidoczny.");
                        return true;
                    }
                }
            }

            return WarningInfos.Count() > 0;
        }
    }
}
