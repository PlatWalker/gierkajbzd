using System.Collections.Generic;
using System.Linq;
using jbzd.Dialogues.RuntimeData;
using UnityEngine;

namespace jbzd.Dialogues
{
    public class DialogueManager : MonoBehaviour
    {

        [SerializeField] private ContainerSO data;
        private string _currentGroup;

        private void OnEnable()
        {
            if (data == null)
            {
                Debug.LogError("NPC nie ma przypisanego dialogu!");
                return;
            }
            
            data.LoadGroups();
            
            var startingGroup = ((EndGroupRuntimeData)GetGroupAndNodes("Start").Value.First()).SelectedGroup;
            
            if (startingGroup == null)
            {
                Debug.LogError("Użyty dialog ma niepoprawnie zdefiniowany początek - nie można znaleźć grupy 'Start'");
                return;
            }
            
            _currentGroup = FindCurrentDialogueGroup(startingGroup); 
        }

        private string FindCurrentDialogueGroup(string group)
        {
            var groupAndNodes = GetGroupAndNodes(group);
            return groupAndNodes.Key.WasGroupUsed ? FindCurrentDialogueGroup(GetEndGroup(groupAndNodes.Value).SelectedGroup) : group;
        }
        
        public NodeRuntimeData GetStartNode()
        {
            return GetGroupAndNodes(_currentGroup).Value.First(x => x.IsStartingDialogue);
        }

        private KeyValuePair<GroupRuntimeData,List<NodeRuntimeData>> GetGroupAndNodes(string title)
        {
            return data.UtilityGroup.FirstOrDefault(x => x.Key.GroupName == title);
        }

        private EndGroupRuntimeData GetEndGroup(List<NodeRuntimeData> nodeList)
        {
            return (EndGroupRuntimeData)nodeList.Find(x => x.NodeType == NodeType.EndGroup);
        }
    }
}
