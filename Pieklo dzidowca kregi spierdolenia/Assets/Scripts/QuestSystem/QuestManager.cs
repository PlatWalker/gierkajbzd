using System.Collections.Generic;
using System.Linq;
using jbzd.QuestSystem.QuestStructureElements;
using UnityEngine;

namespace jbzd.QuestSystem
{
    public class QuestManager : MonoBehaviour
    {
        public List<Quest> ActiveQuests { get; } = new();
        public List<Quest> AllQuestsOnActiveMap { get; } = new();
        
        public void StartQuest(Quest questToStart)
        {
            Debug.Log($"Quest {questToStart.name} started");
            ActiveQuests.Add(questToStart);
            questToStart.ActiveTask = questToStart.QuestData.Tasks.First(task => task.Order == 0);
        }

        public void EndQuest(Quest questToEnd)
        {
            Debug.Log($"Quest {questToEnd.name} ended");
            ActiveQuests.Remove(questToEnd);
            questToEnd.IsCompleted = true;
            questToEnd.ActiveTask = null;
        }
        
        /// <summary>
        /// Makes actor included in <paramref name="goalToAct"/> play a scenario.  
        /// </summary>
        /// <param name="goalToAct">Goal with actors and scenario</param>
        public void MakeActorPlay(GoalSO goalToAct)
        {
            TaskSO inTask = null;

            var numberOfActiveQuests = ActiveQuests.Count;
            
            foreach (var quest in ActiveQuests.Where(quest => quest.QuestData.IsThereAGoal(goalToAct, out inTask)))
            {
                if (quest.FinishedTasks.Contains(inTask) || quest.ActiveTask != inTask) return;
                
                quest.MakeActorPlayInThisQuest(goalToAct);
                
                if (numberOfActiveQuests != ActiveQuests.Count) return;
            }
        }
    }
}