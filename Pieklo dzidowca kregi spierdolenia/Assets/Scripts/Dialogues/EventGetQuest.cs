using UnityEngine;
using jbzdy.DialogueSystem.NodeDatas;


namespace jbzdy.DialogueSystem
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Dialogue/New Quest Event", fileName = "Quest Event")]
    public class EventGetQuest : DialogueEventSO
    {
        private Quest quest;

        public override void RunEvent()
        {
            base.RunEvent();
            quest.GetQuest();
        }

        public override void RunEvent(Object questSO)
        {
            quest = (Quest)questSO;
            base.RunEvent();
            quest.GetQuest();
        }

    }
}
