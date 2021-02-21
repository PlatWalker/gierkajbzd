using UnityEngine;
using jbzdy.DialogueSystem.SO;

namespace jbzdy.DialogueSystem.Events
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Dialogue/New Quest Event", fileName = "Quest Event")]
    public class EventGetQuest : DialogueEventSO
    {
        public string questName;

        public override void RunEvent()
        {
            base.RunEvent();
            GetQuest();
        }

        private void GetQuest()
        {
            Debug.Log("Nowy quest:" +questName);
        }
    }
}
