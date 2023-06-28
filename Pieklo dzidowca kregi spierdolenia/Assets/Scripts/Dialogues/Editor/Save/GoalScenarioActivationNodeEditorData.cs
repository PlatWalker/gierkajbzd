using jbzd.QuestSystem.QuestStructureElements;
using UnityEngine;

namespace jbzd.Dialogues.Editor.Save
{
    public class GoalScenarioActivationNodeEditorData : NodeEditorData
    {
        [field: SerializeField] public GoalSO Goal { get; set; }
    }
}