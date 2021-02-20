using UnityEngine;
using UnityEngine.Events;
using jbzdy.DialogueSystem.Enums;
using System.Collections.Generic;
using jbzdy.DialogueSystem.Events;
using System;

namespace jbzdy.DialogueSystem.Actions
{
    public class DialogueTalk : DialogueGetData
    {
        [SerializeField] private DialogueController dialogueController;
        [SerializeField] private AudioSource audioSource;
        private EventStatCheck eventStatCheck;
        private DialogueNodeData currentDialogueNodeData;
        private StatCheckNodeData lastStatCheckNodeData;
        private DialogueNodeData lastDialogueNodeData;

        private List<StatCheckNodeData> statCheckNodeDatas = new List<StatCheckNodeData>();

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();  
        }

        public void StartDialogue()
        {
            CheckNodeType(GetNextNode(dialogueContainer.StartNodeDatas[0]));
            dialogueController.ShowDialogueUI(true);
        }

        private void CheckNodeType(BaseNodeData _baseNodeData)
        {
            switch (_baseNodeData)
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
                default:
                    break;
            }
        }


        private void RunNode(StartNodeData _nodeData)
        {
            CheckNodeType(GetNextNode(dialogueContainer.StartNodeDatas[0]));
        }

        private void RunNode(DialogueNodeData _nodeData)
        {
            if (currentDialogueNodeData != _nodeData)
            {
                lastDialogueNodeData = currentDialogueNodeData;
                currentDialogueNodeData = _nodeData;
            }

            dialogueController.SetText(_nodeData.Name, _nodeData.TextLanguages.Find(text => text.LanguageType == LanguageController.Instance.Language).LanguageGenericType);
            dialogueController.SetImage(_nodeData.Sprite, _nodeData.DialogueFaceImageType);
            MakeButtons(_nodeData.DialogueNodePorts);

            audioSource.clip = _nodeData.AudioClips.Find(clip => clip.LanguageType == LanguageController.Instance.Language).LanguageGenericType;
            audioSource.Play();
        }
        
        private void RunNode(EventNodeData _nodeData)
        {
            if (_nodeData.DialogueEventSO != null)
            {
                _nodeData.DialogueEventSO.RunEvent();
            }
            CheckNodeType(GetNextNode(_nodeData));
        }

        private void RunNode(StatCheckNodeData nodeData)
        {
            statCheckNodeDatas.Add(nodeData);

            Debug.Log(nodeData.statCheckType + " | " + nodeData.statCheckValue);
            CheckNodeType(GetNextNode(nodeData));
        }

        private void RunNode(EndNodeData _nodeData)
        {
            switch (_nodeData.EndNodeType)
            {
                case EndNodeType.End:
                    dialogueController.ShowDialogueUI(false);
                    break;
                case EndNodeType.Repeat:
                    CheckNodeType(GetNodeByGuid(currentDialogueNodeData.NodeGuid));
                    break;
                case EndNodeType.Goback:
                    CheckNodeType(GetNodeByGuid(lastDialogueNodeData.NodeGuid));
                    break;
                case EndNodeType.RetrunToStart:
                    CheckNodeType(GetNextNode(dialogueContainer.StartNodeDatas[0]));
                    break;
                default:
                    break;
            }
        }

        private void MakeButtons(List<DialogueNodePort> _nodePorts)
        {
            List<string> texts = new List<string>();
            List<UnityAction> unityActions = new List<UnityAction>();

            foreach (DialogueNodePort nodePort in _nodePorts)
            {
                texts.Add(nodePort.TextLanguages.Find(text => text.LanguageType == LanguageController.Instance.Language).LanguageGenericType);
                UnityAction tempAciton = null;
                tempAciton += () =>
                {
                    statCheckNodeDatas.Clear();
                    audioSource.Stop();
                    CheckNodeType(GetNodeByGuid(nodePort.InputGuid));
                };
                unityActions.Add(tempAciton);
            }

            dialogueController.SetButtons(texts, unityActions, statCheckNodeDatas);
        }
    }
}