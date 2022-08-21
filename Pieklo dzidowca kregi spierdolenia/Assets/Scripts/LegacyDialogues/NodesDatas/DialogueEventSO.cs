using UnityEngine;

namespace jbzd.LegacyDialogues.NodesDatas
{
    [System.Serializable]
    public class DialogueEventSO : ScriptableObject
    {
        public virtual void RunEvent()
        {

        }

        public virtual void RunEvent(Object someObject)
        {

        }
    }
}