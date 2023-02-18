using System.Linq;
using jbzd.Common.RunnerThing;
using jbzd.QuestSystem;
using jbzd.QuestSystem.QuestStructureElements;
using UnityEngine;

namespace jbzd.Dialogues.RuntimeData
{
    public class QuestRuntimeData : NodeRuntimeData
    {
        [field: SerializeField] public QuestSO Quest { get; set; }
        [field: SerializeField] public bool IsQuestStart { get; set; }

        [RunMethod]
        public void Run(QuestManager questManager, DialogueManager dialogueManager)
        {
            var quest = questManager.AllQuestsOnActiveMap.FirstOrDefault(x => x.QuestData == Quest);
            
            Debug.Assert(quest != null, "Nie znaleziono questa na tym poziomie, który by pasował do tego podanego w dialogu");
            
            if(IsQuestStart)
                questManager.StartQuest(quest);
            else
                questManager.EndQuest(quest);
            
            dialogueManager.NextNode();
        }
    }
}
