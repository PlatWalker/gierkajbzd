using UnityEngine;
using UnityEngine.Timeline;

namespace jbzd.Dialogues.Editor.Save
{
    public class CutsceneNodeEditorData : NodeEditorData
    {
        [field:SerializeField] public TimelineAsset TimelineAsset { get; set; }
    }
}
