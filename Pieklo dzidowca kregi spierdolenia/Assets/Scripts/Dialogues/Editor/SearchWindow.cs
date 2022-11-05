using System;
using System.Collections;
using System.Collections.Generic;
using jbzd.Dialogues.Editor.Nodes;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace jbzd.Dialogues.Editor
{
    public class SearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private DialogueGraphView _graphView;
        private Texture2D _indentationIcon;
        
        public void Initialize(DialogueGraphView graphView)
        {
            _graphView = graphView;
            _indentationIcon = new Texture2D(1, 1);
            _indentationIcon.SetPixel(0, 0, Color.clear);
            _indentationIcon.Apply();
        }
        
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> searchTreeEntries = new List<SearchTreeEntry>()
            {
                new SearchTreeGroupEntry(new GUIContent("Create Element")),
                new SearchTreeGroupEntry(new GUIContent("Dialogue Group"), 1),
                new SearchTreeEntry(new GUIContent("Single Group", _indentationIcon))
                {
                    level = 2,
                    userData = new Group()
                },
                new SearchTreeGroupEntry(new GUIContent("Dialogue Node"), 1),
            };

            foreach (var type in Enum.GetValues(typeof(NodeType)))
            {
                searchTreeEntries.Add(new SearchTreeEntry(new GUIContent(type.ToString(), _indentationIcon))
                {
                    level = 2,
                    userData = type
                });
            }

            return searchTreeEntries;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            var localMousePosition = _graphView.GetLocalMousePosition(context.screenMousePosition, true);
            var userData = SearchTreeEntry.userData;
            
            if (Enum.IsDefined(typeof(NodeType), userData.ToString()))
            {
                var choiceNode = _graphView.CreateNode(localMousePosition, 
                    (NodeType)Enum.Parse(typeof(NodeType), userData.ToString()));
                    
                _graphView.AddElement(choiceNode);
                
                return true;
            }

            if (userData.GetType() == typeof(DialogueGroup))
            {
                DialogueGroup group = _graphView.CreateGroup(localMousePosition, "Grupa");
                    
                _graphView.AddElement(group);
                
                return true;
            }

            return false;
            
        }
    }
}
