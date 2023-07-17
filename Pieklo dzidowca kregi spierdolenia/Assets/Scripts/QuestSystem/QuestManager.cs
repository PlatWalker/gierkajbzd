using System.Collections.Generic;
using System.Linq;
using jbzd.QuestSystem.QuestStructureElements;
using UnityEngine;
using jbzd.Common.Extensions;
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

            if (questToStart.QuestData.Tasks.Count == 0)
            {
                Debug.Log($"Quest {questToStart.name} does not have tasks");
                return;
            }
            
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
            var questsWithGoalToAct = 
                ActiveQuests.Where(quest => quest.QuestData.IsThereAGoal(goalToAct, out inTask)).ToList();
            
            if (questsWithGoalToAct.Count is 0)
            {
                Debug.Log($"Goal: {goalToAct.name} have tried to act but there were no active quests with such goal");
                return;
            }
            
            foreach (var quest in questsWithGoalToAct)
            {
                //TODO need to be tested, why we are returning if we are iterating through quests?
                if (quest.FinishedTasks.Contains(inTask))
                {
                    Debug.Log($"Goal that trying to act ({goalToAct.name})," +
                              $" is in task{inTask.name} that is already finished");
                    return;
                }

                //TODO need to be tested, why we are returning if we are iterating through quests?
                if (quest.ActiveTask != inTask)
                {
                    Debug.Log($"Goal that is trying to act ({goalToAct.name})," +
                              $" is in task that is not currently active");
                    return;
                }
                
                quest.MakeActorPlayInThisQuest(goalToAct);
                
                // if acting goal finished, and it was last goal in quest thus completing it - quit loop
                if (numberOfActiveQuests != ActiveQuests.Count) return;
            }
        }
    }
}