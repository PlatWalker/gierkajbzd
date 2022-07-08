using UnityEngine;

namespace jbzd.Quests
{
    /*
     * Ta klasa nie moze byc interfejsem. Wyswietlanie w GUI za pomoca "EditorGUILayout" nie obsluguje
     * wystawiania interfejsow.
    */ 
    public abstract class QuestCompleteEvent : MonoBehaviour
    {
        public abstract void OnQuestComplete(QuestGoal questGoal);
    }
}