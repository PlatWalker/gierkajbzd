using jbzdy.DialogueSystem;
using UnityEngine;

namespace jbzd.LegacyDialogues.NodesDatas
{
    [System.Serializable]
    public class CreateCheckpointNodeData : BaseNodeData
    {
        [SerializeField]
        public string CheckpointTag;
        [SerializeField]
        public string CheckpointName;
        [SerializeField]
        public int Level;

        public override void RunNode(DialogueTalk dialogueTalk)
        {
            DialoguesCheckPointsSO container = GameManager.Instance.dialoguesCheckPointsSO;
            container.CheckpointsList.Find(checkpoint =>
            {
                return checkpoint.Level == Level && checkpoint.Name.Equals(CheckpointName) && checkpoint.Tag.Equals(CheckpointTag);
            }).isSet = true;
            dialogueTalk.GetNextNode(this).RunNode(dialogueTalk);
        }
    }
}
