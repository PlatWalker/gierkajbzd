using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class DialogueContainer : ScriptableObject
{
    public List<NodeLinkData> NodeLinks = new List<NodeLinkData>();
    public List<DialogueNodeData> DialogueNodeDatas = new List<DialogueNodeData>();
}
