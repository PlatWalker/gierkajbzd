using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;

namespace jbzd.Dialogues.Editor.Nodes
{
    public class DialogueGroup : Group
    {
        public Guid ID { get; set; }
        public List<BasicNode> Nodes { get; set; }
        private DialogueGraphView GraphView { get; set; }
        public event EventHandler GroupRenamed;
        
        public DialogueGroup(DialogueGraphView graphView)
        {
            GraphView = graphView;
            ID = Guid.NewGuid();
            Nodes = new List<BasicNode>();
        }

        protected override void OnElementsAdded(IEnumerable<GraphElement> elements)
        {
            var graphElements = elements.ToList();
            base.OnElementsAdded(graphElements);
            
            foreach (var element in graphElements.Where(element => element is BasicNode))
            {
                ((BasicNode)element).GroupID = ID;
                Nodes.Add((BasicNode)element);
            }
        }

        protected override void OnElementsRemoved(IEnumerable<GraphElement> elements)
        {
            base.OnElementsAdded(elements);
            
            foreach (GraphElement element in elements)
            {
                if (element is not BasicNode)
                {
                    continue;
                }
                ((BasicNode)element).GroupID = null;
                Nodes.Remove((BasicNode)element);
            }
        }

        protected override void OnGroupRenamed(string oldName, string newName)
        {
            EventHandler handler = GroupRenamed;
            title = GraphView.AvailableGroupTitle(newName, true);
            handler?.Invoke(this, EventArgs.Empty);
        }
        
    }
}
