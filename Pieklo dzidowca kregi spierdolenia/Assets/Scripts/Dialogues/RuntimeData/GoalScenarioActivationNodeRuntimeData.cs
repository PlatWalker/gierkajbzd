using jbzd.Common.RunnerThing;
using jbzd.QuestSystem;
using jbzd.QuestSystem.QuestStructureElements;
using UnityEngine;

namespace jbzd.Dialogues.RuntimeData
{
    public class GoalScenarioActivationNodeRuntimeData : NodeRuntimeData
    {
        [field: SerializeField] public GoalSO Goal { get; set; }

        [RunMethod]
        public void Run(QuestManager questManager, DialogueManager dialogueManager)
        {
            questManager.MakeActorPlay(Goal);
            
            dialogueManager.NextNode();
        }
    }
}