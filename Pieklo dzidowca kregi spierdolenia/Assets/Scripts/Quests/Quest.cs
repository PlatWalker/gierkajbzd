using System.Collections.Generic;
using System.Linq;
using jbzd.Quests.QuestGoals;
using UnityEngine;
using Zenject;

namespace jbzd.Quests
{
    [CreateAssetMenu(fileName = "New Quest", menuName = "Quest")]
    public class Quest : ScriptableObject
    {
        public bool IsActive { get; private set; }
        public bool IsCompleted { get; private set; }

        public string title;
        public string description;
        public int expReward;
        public int goldReward;

        public int startAtLevel;

        public List<Task> tasks = new();

        public int CurrentTask { get; private set; }
        
        private QuestLogManager _questLogManager;
    
        [Inject]
        public void Construct(QuestLogManager questLogManager)
        {
            _questLogManager = questLogManager;
        }
        
        [System.Serializable]
        public class Task
        {
            public List<QuestGoal> goals = new List<QuestGoal>();
            public int order = 0;
            public bool showMore = true;
        }

        private void OnEnable()
        {
            IsActive = false;
            IsCompleted = false;
            CurrentTask = 0;

            foreach (var goal in tasks.SelectMany(task => task.goals))
            {
                goal.ScriptableObjectInit();
            }
        }

        public void GetQuest()
        {
            IsCompleted = false;
            IsActive = true;
            CurrentTask = 0;

            InitializeTask(CurrentTask);
            _questLogManager.AddQuest(this);
        }

        private void InitializeTask(int index)
        {
            foreach (var goal in tasks[index].goals)
            {
                goal.InGameInit();
            }
        }

        public void UpdateQuest()
        {
            if (IsActive)     
            {
                CheckGoals();
            }

        }

        private void CompleteQuest()
        {
            IsActive = false;
            _questLogManager.RemoveQuest(this);
            Debug.Log("koniec questa ");
        }

        private void CompleteTask()
        {       
            IsCompleted = false;

            InitializeTask(CurrentTask);

        }

        public void CheckGoals(bool fromNpc = true)
        {

            if (CurrentTask < tasks.Count)
            {
                foreach (var goal in tasks[CurrentTask].goals)
                    if (!goal.completed)
                        return;

                if (fromNpc)
                {
                    //TODO czemu mamy usuwanie czegos z itemgoala w klasie odpowiadajacej za ogolne questy?
                    foreach (var goal in tasks[CurrentTask].goals.OfType<ItemGoal>()) goal.RemoveListeners();

                    CurrentTask++;

                    if (CurrentTask == tasks.Count)
                        CompleteQuest();
                    else
                        CompleteTask();
                }
                else
                {
                    IsCompleted = true;
                }

                Debug.Log("koniec zadania");
            }
            else
            {
                Debug.LogError("Obecna ilosc skonczonych taskow jest wieksza od ich ogolnej ilosci!");
            }

            if (IsCompleted) _questLogManager.ChangeQuestName();
        }

    }
}
