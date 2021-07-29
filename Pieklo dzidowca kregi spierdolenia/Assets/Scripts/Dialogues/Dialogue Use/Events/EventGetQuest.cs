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
