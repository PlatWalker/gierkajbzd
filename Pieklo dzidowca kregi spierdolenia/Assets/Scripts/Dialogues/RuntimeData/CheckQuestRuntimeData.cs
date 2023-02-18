using System.Linq;
using jbzd.Common.RunnerThing;
using jbzd.QuestSystem;
using jbzd.QuestSystem.QuestStructureElements;
using UnityEngine;

namespace jbzd.Dialogues.RuntimeData
{
    public class CheckQuestRuntimeData : NodeRuntimeData
    {
        [field: SerializeField] public QuestSO Quest { set; get; }

        [RunMethod]
        public void Run(QuestManager questManager, DialogueManager dialogueManager)
        {
            var quest = questManager.AllQuestsOnActiveMap.FirstOrDefault(x => x.QuestData == Quest);
            
            Debug.Assert(quest != null, "Nie znaleziono questa na tym poziomie, który by pasował do tego podanego w dialogu");
            
            var nextNodeId = quest.IsCompleted ? Choices[0].NextDialogue : Choices[1].NextDialogue;
            
            dialogueManager.RunNode(nextNodeId);
        }
    }
}
