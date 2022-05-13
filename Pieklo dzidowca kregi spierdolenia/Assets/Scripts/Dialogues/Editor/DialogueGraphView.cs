using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace jbzdy.DialogueSystem.Editor
{
    public class DialogueGraphView : GraphView
    {
        private string styleSheetsName = "GraphViewStyleSheet";
        private DialogueEditorWindow editorWindow;
        private NodeSearchWindow searchWindow;

        public DialogueGraphView(DialogueEditorWindow newEditorWindow)
        {
            editorWindow = newEditorWindow;

            StyleSheet tmpStyleSheet = Resources.Load<StyleSheet>(styleSheetsName);
            styleSheets.Add(tmpStyleSheet);

            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new FreehandSelector());

            GridBackground grid = new();
            Insert(0, grid);
            grid.StretchToParentSize();

            AddSearchWindow();
        }

        private void AddSearchWindow()
        {
            searchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
            searchWindow.Configure(editorWindow, this);
            nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new();
            Port startPortView = startPort;

            ports.ForEach((port) =>
            {
                Port portView = port;

                if (startPortView != portView && startPortView.node != portView.node && startPortView.direction != port.direction)
                {
                    compatiblePorts.Add(port);
                }
            });

            return compatiblePorts;
        }
    }
}

