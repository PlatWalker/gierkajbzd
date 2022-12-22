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

    }
}