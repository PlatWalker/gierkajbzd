using System;
using UnityEngine;
using System.Collections.Generic;

namespace jbzdy.DialogueSystem.DataContainers
{
    [Serializable]
    public class DialogueContainer : ScriptableObject
    {
        public List<NodeLinkData> NodeLinks = new List<NodeLinkData>();
        public List<DialogueNodeData> DialogueNodeDatas = new List<DialogueNodeData>();
        public List<ExposedProperty> ExposedProperties = new List<ExposedProperty>();
        public List<CommentBlockData> CommentBlockDatas = new List<CommentBlockData>();
    }
}