using UnityEngine;
using jbzdy.DialogueSystem.SO;
using jbzdy.DialogueSystem.Enums;

namespace jbzdy.DialogueSystem.Events
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Dialogue/New StatCheck Event", fileName = "StatCheck Event")]
    public class EventStatCheck : DialogueEventSO
    {
        public StatCheckType statCheckType;
        public int statCheckValue;

        public override void RunEvent()
        {
            base.RunEvent();
            StatCheck();
        }

        private void StatCheck()
        {
            Debug.Log("Nie jesteś zbyt silny");
        }
    }
}