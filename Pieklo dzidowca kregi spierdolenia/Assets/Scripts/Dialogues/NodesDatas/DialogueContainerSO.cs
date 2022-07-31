using UnityEngine;
using System.Collections.Generic;


namespace jbzdy.DialogueSystem.NodeDatas
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Dialogue/New Dialogue")]
    public class DialogueContainerSO : ScriptableObject
    {
        [field: SerializeField]
        public List<NodeLinkData> NodeLinkDatas { get; set; } = new();
        
        [SerializeReference]
        public List<BaseNodeData> NodeDatas = new();

        public BaseNodeData GetStartNodeData()
        {
            return NodeDatas.Find(nodeData => nodeData is StartNodeData);
        }
    }
}
