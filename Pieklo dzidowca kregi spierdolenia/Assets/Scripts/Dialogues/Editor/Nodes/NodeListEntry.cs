using jbzdy.DialogueSystem.Editor;
using jbzdy.DialogueSystem.NodeDatas;

namespace jbzdy.DialogueSystem.Nodes
{
    public class NodeListEntry
    {
        public BaseNode BaseNode { get; private set; }
        public string Name { get; private set; }
        public BaseNodeData BaseNodeData { get; private set; }
        public NodeListEntry(string name,BaseNode node, BaseNodeData data)
        {
            Name = name;
            BaseNode = node;
            BaseNodeData = data;
        }

        public BaseNode GenerateNodeFromData(DialogueEditorWindow newEditorWindow, DialogueGraphView newGraphView,BaseNodeData data)
        {
            BaseNode newNode = BaseNode.CreateNewNode(newEditorWindow, newGraphView);
            newNode.LoadDataIntoNode(data);
            return newNode;
        }
    }
}