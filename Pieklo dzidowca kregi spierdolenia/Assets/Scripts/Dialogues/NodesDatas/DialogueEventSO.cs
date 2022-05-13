using UnityEngine;

namespace jbzdy.DialogueSystem.NodeDatas
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