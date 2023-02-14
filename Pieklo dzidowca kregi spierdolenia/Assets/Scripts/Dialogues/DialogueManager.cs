using System;
using System.Collections.Generic;
using System.Linq;
using jbzd.Common.RunnerThing;
using jbzd.Dialogues.RuntimeData;
using jbzd.UI;
using UnityEngine;
using Zenject;

namespace jbzd.Dialogues
{
    public class DialogueManager : MonoBehaviour
    {
        private ContainerSO _data;
        private string _currentGroup;
        public NodeRuntimeData CurrentNode;
        
        public delegate void DialogueEventOccured();

        public event DialogueEventOccured OnDialogueStarted;
        public event DialogueEventOccured OnDialogueEnded;

        private RunnerFactory _factory;

        [Inject]
        public void Constructor(RunnerFactory runnerFactory)
        {
            _factory = runnerFactory;
        }
        
        public void StartDialogue(ContainerSO data)
        {
            
            _data = data;
            _data.LoadGroups();
            
            var startingGroup = ((EndGroupRuntimeData)GetGroupAndNodes("Start").Value.First()).SelectedGroup;
            
            if (startingGroup == null)
            {
                Debug.LogError("Użyty dialog ma niepoprawnie zdefiniowany początek - nie można znaleźć grupy 'Start'");
                return;
            }
            
            _currentGroup = FindCurrentDialogueGroup(startingGroup);

            OnDialogueStarted?.Invoke();
            
            RunNode(GetStartNode());
        }

        private void RunNode(NodeRuntimeData node)
        {
            var runner = _factory.Create(node);
            CurrentNode = node;
            runner.Run();
        }
        
        public void RunNode(string node)
        {
            RunNode(FindNode(node));
        }

        private string FindCurrentDialogueGroup(string group)
        {
            var groupAndNodes = GetGroupAndNodes(group);
            return groupAndNodes.Key.WasGroupUsed ? FindCurrentDialogueGroup(GetEndGroup(groupAndNodes.Value).SelectedGroup) : group;
        }
        
        private NodeRuntimeData GetStartNode()
        {
            return GetGroupAndNodes(_currentGroup).Value.First(x => x.IsStartingDialogue);
        }

        private KeyValuePair<GroupRuntimeData,List<NodeRuntimeData>> GetGroupAndNodes(string title)
        {
            return _data.UtilityGroup.FirstOrDefault(x => x.Key.GroupName == title);
        }

        private EndGroupRuntimeData GetEndGroup(List<NodeRuntimeData> nodeList)
        {
            return (EndGroupRuntimeData)nodeList.Find(x => x.NodeType == NodeType.EndGroup);
        }

        private NodeRuntimeData FindNode(string nextNodeId)
        {
            var nodes = GetGroupAndNodes(_currentGroup).Value;
            return nodes.Find(x => x.NodeId == nextNodeId);
        }

        public void ChangeGroupStatus(string groupTitle = null, bool status = true)
        {
            groupTitle ??= _currentGroup;

            GetGroupAndNodes(groupTitle).Key.WasGroupUsed = status;
        }

        public void EndDialogue()
        {
            _currentGroup = null;
            CurrentNode = null;
            
            OnDialogueEnded?.Invoke();
        }
    }
}
