using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jbzdy.DialogueSystem.NodeDatas
{

    [System.Serializable]
    public class StartNodeData : BaseNodeData
    {
        public override void RunNode(DialogueTalk dialogueTalk)
        {
            dialogueTalk.RunNode(dialogueTalk.GetNextNode(dialogueTalk.CurrentDialogue.GetStartNodeData()));
        }
    }
}