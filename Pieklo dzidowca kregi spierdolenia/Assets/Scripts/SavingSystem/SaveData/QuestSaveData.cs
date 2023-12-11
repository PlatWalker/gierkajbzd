using System;
using System.Collections.Generic;

namespace jbzd.SavingSystem.SaveData
{
    [Serializable]
    public class QuestSaveData
    {
        public string questId;
        
        public bool isCompleted;
        public string idOfActiveTask;
        public List<string> idOfFinishedTasks;
        public List<string> idOfFinishedGoals;
        public List<string> idOfActors;

        public List<int> goalItemCollected = new();
        public List<int> goalCompletionItemCount = new();
        public List<string> goalName = new();
    }
}