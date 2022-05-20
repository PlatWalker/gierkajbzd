using UnityEngine;

namespace jbzdy.DialogueSystem.NodeDatas
{
    public class CheckpointSplitterNodeData : BaseNodeData
    {
        [SerializeField]
        public string CheckpointTag = string.Empty;
        [SerializeField]
        public string CheckpointName = string.Empty;
        [SerializeField]
        public int Level = 0;
        [SerializeField]
        public string PositiveResultGuid = string.Empty;
        [SerializeField]
        public string NegativeResultGuid = string.Empty;
        public override void RunNode(DialogueTalk dialogueTalk)
        {
            DialoguesCheckPointsSO container = GameManager.Instance.dialoguesCheckPointsSO;
           
            bool isSet = container.CheckpointsList.Find(checkpoint =>
            CheckpointName.Equals(checkpoint.Name)
            ).isSet;

            if (isSet)
            {
                dialogueTalk.GetNodeByGuid(PositiveResultGuid).RunNode(dialogueTalk);
            }
            else
            {
                dialogueTalk.GetNodeByGuid(NegativeResultGuid).RunNode(dialogueTalk);
            }
        }
    }
}