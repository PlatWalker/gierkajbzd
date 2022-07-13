using UnityEditor;
using UnityEngine;

namespace jbzd.Quests
{
    [System.Serializable]
    public abstract class QuestGoal : ScriptableObject 
    {
        public string Description { get; set; }
        public bool completed;// nie moze byc property bo edytor dostaje dałna
        public int RequiredAmount { get; set; } = 1;
        public int CurrentAmount { get; set; }

        private QuestCompleteEvent _questCompleteEvent;

        public virtual void InGameInit()
        {
            //TODO To chyba nie jest juz potrzebne? 
            _questCompleteEvent = GameManager.Instance.QuestController;
        }

        public virtual void ScriptableObjectInit()
        {
            completed = false;
            CurrentAmount = 0;
        }
        
        protected bool IsReached()
        {

            if (CurrentAmount >= RequiredAmount)
            {
                Complete();
                return true;
            }

            return false;
        }

        private void Complete()
        {
            completed = true;
            _questCompleteEvent.OnQuestComplete(this);
            Debug.Log("Koniec podzadania");
        }

        public override string ToString()
        {
            return Description + ": <i>" + CurrentAmount + "/" + RequiredAmount + "</i>\n";
        }
#if UNITY_EDITOR
        public virtual void GoalCustomEditor()
        {
            EditorGUILayout.LabelField("Opis");
            Description = EditorGUILayout.TextArea(Description, GUILayout.Height(20));
            RequiredAmount = EditorGUILayout.IntField("Potrzebna ilość", RequiredAmount);
        }
#endif
    }
}

