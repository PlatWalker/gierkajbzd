using System.Collections.Generic;
using jbzd.Common.RunnerThing;
using jbzd.UI;
using UnityEngine;

namespace jbzd.Dialogues.RuntimeData
{

    public class DialogueRuntimeData : NodeRuntimeData
    {
        [field: SerializeField] [field: TextArea()] public string Text { get; set; }
        [field: SerializeField] public Sprite NpcImage { get; set; }
        [field: SerializeField] public Sprite PlayerImage { get; set; }

        public DialogueRuntimeData(string text, List<ChoiceRuntimeData> choices, NodeType nodeType,
            bool isStartingDialogue, Sprite npcImage, Sprite playerImage, string id)
        {
            Text = text;
            Choices = choices;
            NodeType = nodeType;
            IsStartingDialogue = isStartingDialogue;
            NpcImage = npcImage;
            PlayerImage = playerImage;
            NodeId = id;
        }

        [RunMethod]
        public void Run(UserInterfaceManager uiManager)
        {
            var controller = uiManager.GetUIController<DialogueUIController>();
            
            controller.ShowText(Text);
            controller.SetChoices(Choices);
            controller.SetNpcImage(NpcImage);
            controller.SetPlayerImage(PlayerImage);
        }
    }
}
