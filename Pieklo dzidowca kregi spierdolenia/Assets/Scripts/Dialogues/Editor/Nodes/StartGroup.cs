using System;
using jbzd.Dialogues;
using jbzd.Dialogues.Editor.Nodes;

using UnityEngine;

namespace jbzd.Dialogues.Editor.Nodes
{
    public class StartGroup:DialogueGroup
    {
        public StartGroup(DialogueGraphView graphView):base(graphView)
        {
            title = "Start";
            name = "Start";
        }

        public StartGroup(DialogueGraphView graphView, Guid id):base(graphView)
        {
            title = "Start";
            name = "Start";
            
            if (id != default)
            {
                ID = id;
            }
        }
    }
}