using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jbzdy.DialogueSystem.NodeDatas
{
    [System.Serializable]
    public class DialogueNodeData : BaseNodeData
    {
        [field: SerializeField]
        public List<DialogueNodePort> DialogueNodePorts { get; set; }
        [field: SerializeField]
        public AudioClip AudioClip { get; set; }
        [field: SerializeField]
        public string Text { get; set; }
        [field: SerializeField]
        public Sprite npcSprite { get; set; }
        [field: SerializeField]
        public Sprite playerSprite { get; set; }
        [field: SerializeField]
        public string Name { get; set; }

        public override void RunNode(DialogueTalk dialogueTalk)
        {
            if (dialogueTalk.CurrentDialogueNodeData != this)
            {
                dialogueTalk.LastDialogueNodeData = dialogueTalk.CurrentDialogueNodeData;
                dialogueTalk.CurrentDialogueNodeData = this;
            }

            dialogueTalk.DialogueControler.SetText(Name, Text);
            dialogueTalk.DialogueControler.SetImage(playerSprite, npcSprite);

            dialogueTalk.MakeButtons(DialogueNodePorts);
            if (dialogueTalk.AudioSource is not null)
            {
                dialogueTalk.AudioSource.clip = AudioClip;
                dialogueTalk.AudioSource.Play();
            }
        }
    }
}
