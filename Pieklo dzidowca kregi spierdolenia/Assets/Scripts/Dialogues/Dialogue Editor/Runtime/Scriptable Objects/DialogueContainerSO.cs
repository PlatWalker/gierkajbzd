using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using jbzdy.DialogueSystem.Enums;
using UnityEditor.Experimental.GraphView;

/// <summary>
/// Napisane przez sharashino
/// 
/// ScriptableObject zawierający cały dialog w wersji graficznej jak i tekstowej
/// </summary>
namespace jbzdy.DialogueSystem.SO
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Dialogue/New Dialogue")]
    public class DialogueContainerSO : ScriptableObject
    {
        public List<NodeLinkData> NodeLinkDatas = new List<NodeLinkData>();

        public List<DialogueNodeData> DialogueNodeDatas = new List<DialogueNodeData>();
        public List<EndNodeData> EndNodeDatas = new List<EndNodeData>();
        public List<StartNodeData> StartNodeDatas = new List<StartNodeData>();
        public List<EventNodeData> EventNodeDatas = new List<EventNodeData>();
        public List<StatCheckNodeData> StatCheckNodeDatas = new List<StatCheckNodeData>();
        public List<BaseNodeData> AllNodes
        {
            get
            {
                List<BaseNodeData> tmp = new List<BaseNodeData>();
                tmp.AddRange(DialogueNodeDatas);
                tmp.AddRange(EndNodeDatas);
                tmp.AddRange(StartNodeDatas);
                tmp.AddRange(EventNodeDatas);
                tmp.AddRange(StatCheckNodeDatas);

                return tmp;
            }
        }
    }

    [System.Serializable]
    public class NodeLinkData
    {
        public string BaseNodeGuid;
        public string TargetNodeGuid;
    }

    [System.Serializable]
    public class BaseNodeData
    {
        public string NodeGuid;
        public Vector2 Position;
    }

    [System.Serializable]
    public class DialogueNodeData : BaseNodeData
    {
        public List<DialogueNodePort> DialogueNodePorts;
        public DialogueFaceImageType DialogueFaceImageType;
        public List<LanguageGeneric<AudioClip>> AudioClips;
        public List<LanguageGeneric<string>> TextLanguages;
        public Sprite Sprite;
        public string Name;
    }

    [System.Serializable]
    public class EndNodeData : BaseNodeData
    {
        public EndNodeType EndNodeType;
    }

    [System.Serializable]
    public class StartNodeData : BaseNodeData
    {

    }

    [System.Serializable]
    public class EventNodeData : BaseNodeData
    {
        public DialogueEventSO DialogueEventSO;
    }

    [System.Serializable]
    public class StatCheckNodeData : BaseNodeData
    {
        public StatCheckType statCheckType;
        public int statCheckValue;
    }

    [System.Serializable]
    public class LanguageGeneric<T>
    {
        public LanguageType LanguageType;
        public T LanguageGenericType;
    }

    [System.Serializable]
    public class DialogueNodePort
    {
        public string PortGuid;
        public string InputGuid;
        public string OutputGuid;
        public Port MyPort;
        public TextField TextField;
        public List<LanguageGeneric<string>> TextLanguages = new List<LanguageGeneric<string>>();
    }
}
