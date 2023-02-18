using System.Collections.Generic;
using System.Linq;
using jbzd.Common.RunnerThing;
using jbzd.Dialogues.RuntimeData;
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

            _currentGroup = _data.CurrentGroup;

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

        public void NextNode()
        {
            var nextNodeId = CurrentNode.Choices[0].NextDialogue;
                
            if (nextNodeId == null)
            {
                OnDialogueEnded?.Invoke();
                return;
            }
            
            RunNode(nextNodeId);
        }
        
        private NodeRuntimeData GetStartNode()
        {
            return GetGroupAndNodes(_currentGroup).Value.First(x => x.IsStartingDialogue);
        }

        private KeyValuePair<GroupRuntimeData,List<NodeRuntimeData>> GetGroupAndNodes(string title)
        {
            return _data.UtilityGroup.FirstOrDefault(x => x.Key.GroupName == title);
        }

        private NodeRuntimeData FindNode(string nextNodeId)
        {
            var nodes = GetGroupAndNodes(_currentGroup).Value;
            return nodes.Find(x => x.NodeId == nextNodeId);
        }

        public void ChangeLastGroup(string group)
        {
            _data.CurrentGroup = group;
        }
        
        public void EndDialogue()
        {
            _currentGroup = null;
            CurrentNode = null;
            
            OnDialogueEnded?.Invoke();
        }
    }
}
