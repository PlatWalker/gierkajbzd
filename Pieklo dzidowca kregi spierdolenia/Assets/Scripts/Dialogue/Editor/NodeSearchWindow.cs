using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using UnityEngine.UIElements;

public class NodeSearchWindow : ScriptableObject, ISearchWindowProvider
{
    private DialogueGraphView graphView;
    private EditorWindow window;
    private Texture2D icon;

    public void Init(EditorWindow window, DialogueGraphView graphView)
    {
        this.window = window;
        this.graphView = graphView;

        //hehe
        icon = new Texture2D(1, 1);
        icon.SetPixel(0, 0, new Color(0, 0, 0, 0));
    }

    public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
    {
        var tree = new List<SearchTreeEntry>
        {
            new SearchTreeGroupEntry(new GUIContent("Create Elements"), 0),
            new SearchTreeGroupEntry(new GUIContent("Dialogue"), 1),
            new SearchTreeEntry(new GUIContent("Dialogue Node"))
            {
                userData = new DialogueNode(), level = 2
            }
        };

        return tree;
    }

    public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
    {
        Vector2 worldMousePos = window.rootVisualElement.ChangeCoordinatesTo(window.rootVisualElement.parent, context.screenMousePosition - window.position.position);
        Vector2 localMousePos = graphView.contentViewContainer.WorldToLocal(worldMousePos);

        switch (SearchTreeEntry.userData)
        {
            case DialogueNode dialogueNode:
                graphView.CreateNode("New Dialogue Node", localMousePos);
                return true;
            default:
                return false;
        }
    }
}
