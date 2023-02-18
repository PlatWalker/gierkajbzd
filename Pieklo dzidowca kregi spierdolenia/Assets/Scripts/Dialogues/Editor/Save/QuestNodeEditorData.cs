using jbzd.QuestSystem.QuestStructureElements;
using UnityEngine;

namespace jbzd.Dialogues.Editor.Save
{
    public class QuestNodeEditorData : NodeEditorData
    {
        [field: SerializeField] public QuestSO Quest { get; set; }
        [field: SerializeField] public bool IsQuestStart { get; set; }
    }
}
