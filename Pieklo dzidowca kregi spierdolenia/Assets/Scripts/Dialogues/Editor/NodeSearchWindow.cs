using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using jbzdy.DialogueSystem.Nodes;
using UnityEditor.Experimental.GraphView;


namespace jbzdy.DialogueSystem.Editor
{
    public class NodeSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private DialogueEditorWindow editorWindow;
        private DialogueGraphView graphView;
        private NodeList list;

        private Texture2D pic;

        public void Configure(DialogueEditorWindow newEditorWindow, DialogueGraphView newGraphView)
        {
            list = new NodeList();
            editorWindow = newEditorWindow;
            graphView = newGraphView;

            pic = new Texture2D(1, 1);
            pic.SetPixel(0, 0, new Color(0, 0, 0, 0));
            pic.Apply();
        }


        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> tree = new List<SearchTreeEntry>
            {
                new SearchTreeGroupEntry(new GUIContent("Dialogue Nodes"),1),
            };

            foreach (NodeListEntry entry in list.List)
            {
                tree.Add(AddNodeSearch(entry.Name, entry.BaseNode));
            }

            return tree;
        }

        private SearchTreeEntry AddNodeSearch(string nodeName, BaseNode baseNode)
        {
            SearchTreeEntry tempEntry = new SearchTreeEntry(new GUIContent(nodeName, pic))
            {
                level = 2,
                userData = baseNode
            };

            return tempEntry;
        }

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            Vector2 mousePosition = editorWindow.rootVisualElement.ChangeCoordinatesTo
                (
                editorWindow.rootVisualElement.parent, context.screenMousePosition - editorWindow.position.position
                );

            Vector2 graphMousePosition = graphView.contentViewContainer.WorldToLocal(mousePosition);

            return ((BaseNode)searchTreeEntry.userData).DrawNode(editorWindow, graphView, graphMousePosition);
        }
    }
}