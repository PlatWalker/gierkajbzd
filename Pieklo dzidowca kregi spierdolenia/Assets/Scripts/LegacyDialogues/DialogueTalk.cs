using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using jbzd.LegacyDialogues;
using jbzd.LegacyDialogues.NodesDatas;
using jbzd.NPC;


namespace jbzdy.DialogueSystem
{
    public class DialogueTalk : DialogueGetData
    {
        public DialogueController DialogueControler { get; set; }
        
        private AudioSource _audioSource = default;
        public AudioSource AudioSource { get => _audioSource; set => _audioSource = value; }

        public DialogueNodeData CurrentDialogueNodeData { get; set; }
        public DialogueNodeData LastDialogueNodeData { get; set; }

        public List<StatCheckNodeData> StatCheckNodeDatas { get; set; } = new ();
        public bool IsTalking { get; set; } = false;

        private void Awake()
        {
            if(!TryGetComponent(out _audioSource))
            {
                Debug.LogWarning("Some NPC doesn't have audio source. Sounds wont be played");
            }
        }

        private void Start()
        {
        }

        public void StartDialogue(DialogueContainerSO dialogueContainer)
        {
            DialogueControler = DialogueController.Instance;
            CurrentDialogue = dialogueContainer;

            if (IsTalking != true)
            {
                IsTalking = true;
                RunNode(GetNextNode(CurrentDialogue.GetStartNodeData()));
                DialogueControler.ShowDialogueUI(true);
            }
        }

        public void EndDialogue()
        {
            IsTalking = false;
            DialogueControler.ShowDialogueUI(false);
            GetComponent<NpcController>().StopInteract();
        }

        public void RunNode(BaseNodeData baseNodeData)
        {
            baseNodeData.RunNode(this);
        }

        public void MakeButtons(List<DialogueNodePort> nodePorts)
        {
            List<string> texts = new List<string>();
            List<UnityAction> unityActions = new List<UnityAction>();

            foreach (DialogueNodePort nodePort in nodePorts)
            {
                texts.Add(nodePort.Text);
                UnityAction tempAciton = null;
                tempAciton += () =>
                {
                    StatCheckNodeDatas.Clear();

                    if (AudioSource is not null)
                    {
                        AudioSource.Stop();
                    }
                    RunNode(GetNodeByGuid(nodePort.InputGuid));
                };
                unityActions.Add(tempAciton);
            }

            DialogueControler.SetButtons(texts, unityActions, StatCheckNodeDatas);
        }

    } 
}