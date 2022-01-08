using UnityEngine;
using jbzdy.DialogueSystem.SO;

/// <summary>
/// Napisane przez sharashino
/// 
/// Event w dialogu odpowiadający za otrzymanie questa
/// </summary>
namespace jbzdy.DialogueSystem.Events
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
