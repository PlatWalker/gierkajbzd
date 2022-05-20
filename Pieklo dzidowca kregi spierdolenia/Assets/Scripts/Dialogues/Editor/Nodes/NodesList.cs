using System.Collections.Generic;
using jbzdy.DialogueSystem.Editor;
using jbzdy.DialogueSystem.NodeDatas;
using UnityEngine;

namespace jbzdy.DialogueSystem.Nodes
{
    public class NodeList
    {
        public List<NodeListEntry> List { get; private set; }
        public NodeList()
        {
            List = new List<NodeListEntry>();
            List.Add(new NodeListEntry("Start Node", new StartNode(), new StartNodeData()));
            List.Add(new NodeListEntry("Dialogue Node", new DialogueNode(), new DialogueNodeData()));
            List.Add(new NodeListEntry("Event Node", new EventNode(), new EventNodeData()));
            List.Add(new NodeListEntry("Stat Check Node", new StatCheckNode(), new StatCheckNodeData()));
            List.Add(new NodeListEntry("Give or Take Item Node", new GiveOrTakeItemNode(), new GiveOrTakeItemNodeData()));
            List.Add(new NodeListEntry("Item Check Node", new ItemCheckNode(), new ItemCheckNodeData()));
            List.Add(new NodeListEntry("End Node", new EndNode(), new EndNodeData()));
            List.Add(new NodeListEntry("Quest Check Node", new QuestCheckNode(), new QuestCheckNodeData()));
            List.Add(new NodeListEntry("Create Checkpoint Node", new CreateCheckpointNode(), new CreateCheckpointNodeData()));
            List.Add(new NodeListEntry("Checkpoint Splitter Node", new CheckpointSplitterNode(), new CheckpointSplitterNodeData()));
        }

        public BaseNode GenerateNodeBasedOnData(BaseNodeData nodeData, DialogueEditorWindow newEditorWindow, DialogueGraphView newGraphView)
        {
            foreach(NodeListEntry listEntry in List){
                if(nodeData.GetType() == listEntry.BaseNodeData.GetType())
                {
                    return listEntry.GenerateNodeFromData(newEditorWindow, newGraphView, nodeData);
                }
            }
            throw new KeyNotFoundException();
        }
    }
   
}