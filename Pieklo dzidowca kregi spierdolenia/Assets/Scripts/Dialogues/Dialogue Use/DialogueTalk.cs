using UnityEngine;
using UnityEngine.Events;
using jbzdy.DialogueSystem.SO;
using jbzdy.DialogueSystem.Enums;
using System.Collections.Generic;

/// <summary>
/// Napisane przez sharashino
/// 
/// Skrypt odpowiadający za odczytywanie danych z ScriptableObjectu i wrzucanie ich do okna z dialogiem (DialogueController)
/// </summary>
namespace jbzdy.DialogueSystem.Actions
{
    public class DialogueTalk : DialogueGetData
    {
        [SerializeField] private DialogueController dialogueControler = default;
        [SerializeField] private AudioSource audioSource = default;
       
        private DialogueNodeData currentDialogueNodeData;
        private DialogueNodeData lastDialogueNodeData;

        private List<StatCheckNodeData> statCheckNodeDatas = new List<StatCheckNodeData>();
        private List<ItemCheckNodeData> itemCheckNodeDatas = new List<ItemCheckNodeData>();

        private bool isTalking = false;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();  
        }

        public void StartDialogue()
        {
            if(isTalking != true)
            {
                isTalking = true;
                CheckNodeType(GetNextNode(dialogue.StartNodeDatas[0]));
                dialogueControler.ShowDialogueUI(true);
            }
        }

        public void EndDialogue()
        {
            isTalking = false;
            dialogueControler.ShowDialogueUI(false);
            GetNodeByGuid(dialogue.EndNodeDatas[0].NodeGuid);
        }

        private void CheckNodeType(BaseNodeData baseNodeData)
        {
            switch (baseNodeData)
            {
                case StartNodeData nodeData:
                    RunNode(nodeData);
                    break;
                case DialogueNodeData nodeData:
                    RunNode(nodeData);
                    break;
                case EventNodeData nodeData:
                    RunNode(nodeData);
                    break;
                case EndNodeData nodeData:
                    RunNode(nodeData);
                    break;
                case StatCheckNodeData nodeData:
                    RunNode(nodeData);
                    break;
                case ItemCheckNodeData nodeData:
                    RunNode(nodeData);
                    break;
                default:
                    break;
            }
        }

        private void RunNode(StartNodeData nodeData)
        {
            CheckNodeType(GetNextNode(dialogue.StartNodeDatas[0]));
        }

        private void RunNode(DialogueNodeData nodeData)
        {
            if (currentDialogueNodeData != nodeData)
            {
                lastDialogueNodeData = currentDialogueNodeData;
                currentDialogueNodeData = nodeData;
            }

            dialogueControler.SetText(nodeData.Name, nodeData.TextLanguages.Find(text => text.LanguageType == LanguageController.Instance.Language).LanguageGenericType);
            dialogueControler.SetImage(nodeData.Sprite, nodeData.DialogueFaceImageType);
            MakeButtons(nodeData.DialogueNodePorts);

            audioSource.clip = nodeData.AudioClips.Find(clip => clip.LanguageType == LanguageController.Instance.Language).LanguageGenericType;
            audioSource.Play();
        }
        
        private void RunNode(EventNodeData nodeData)
        {
            if (nodeData.DialogueEventSO != null)
            {
                nodeData.DialogueEventSO.RunEvent();
            }

            CheckNodeType(GetNextNode(nodeData));
        }

        private void RunNode(StatCheckNodeData nodeData)
        {
            statCheckNodeDatas.Add(nodeData);

            CheckNodeType(GetNextNode(nodeData));
        }

        private void RunNode(ItemCheckNodeData nodeData)
        {
            itemCheckNodeDatas.Add(nodeData);

            CheckNodeType(GetNextNode(nodeData));
        }

        private void RunNode(EndNodeData nodeData)
        {
            switch (nodeData.EndNodeType)
            {
                case EndNodeType.End:
                    dialogueControler.ShowDialogueUI(false);
                    break;
                case EndNodeType.Repeat:
                    CheckNodeType(GetNodeByGuid(currentDialogueNodeData.NodeGuid));
                    break;
                case EndNodeType.Goback:
                    CheckNodeType(GetNodeByGuid(lastDialogueNodeData.NodeGuid));
                    break;
                case EndNodeType.RetrunToStart:
                    CheckNodeType(GetNextNode(dialogue.StartNodeDatas[0]));
                    break;
                default:
                    break;
            }

            EndDialogue();
        }

        private void MakeButtons(List<DialogueNodePort> nodePorts)
        {
            List<string> texts = new List<string>();
            List<UnityAction> unityActions = new List<UnityAction>();

            foreach (DialogueNodePort nodePort in nodePorts)
            {
                texts.Add(nodePort.TextLanguages.Find(text => text.LanguageType == LanguageController.Instance.Language).LanguageGenericType);
                UnityAction tempAciton = null;
                tempAciton += () =>
                {
                    statCheckNodeDatas.Clear();
                    itemCheckNodeDatas.Clear();

                    audioSource.Stop();
                    CheckNodeType(GetNodeByGuid(nodePort.InputGuid));
                };
                unityActions.Add(tempAciton);
            }

            dialogueControler.SetButtons(texts, unityActions, statCheckNodeDatas, itemCheckNodeDatas);
        }
    }
}