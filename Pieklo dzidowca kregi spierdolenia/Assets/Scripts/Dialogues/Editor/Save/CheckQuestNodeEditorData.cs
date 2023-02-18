using jbzd.QuestSystem.QuestStructureElements;
using UnityEngine;

namespace jbzd.Dialogues.Editor.Save
{
    public class CheckQuestNodeEditorData : NodeEditorData
    {
        [field: SerializeField] public QuestSO Quest { get; set; }
    }
}
