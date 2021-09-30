using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Assets/Resources/Quests/New Quest", menuName = "Quest")]
public class Quest : ScriptableObject 
{
    public bool isActive;
    public bool isCompleted = false;

    public string title;
    public string description;
    public int expReward;
    public int goldReward;

    public List<Task> tasks = new List<Task>();

    public int currentTask = 0;

    [System.Serializable]
    public class Task
    {
        public List<QuestGoal> goals = new List<QuestGoal>();
        public int order = 0;
        public bool showMore = true;
    }

    private void OnEnable()
    {
        isActive = false;
        isCompleted = false;
        currentTask = 0;
    }

    public void GetQuest()
    {
        isCompleted = false;
        isActive = true;
        currentTask = 0;

        InitializeTask(currentTask);

        GameManager.Instance.PlayerObject.GetComponent<QuestLog>().AddQuest(this);
    }

    private void InitializeTask(int index)
    {
        foreach (QuestGoal goal in tasks[index].goals)
        {
            goal.Init();
        }
    }

    public void UpdateQuest()
    {
        if (!isActive && !isCompleted)       //quest do wzięcia
        {
            
        }
        else if (isActive && !isCompleted)     //quest aktywny, w środku
        {
            CheckGoals();
            //questPanel.UpdateQuestsLog();
        }
        else if (isActive && isCompleted)     //quest aktywny, w środku z ukończonym taskiem
        {
            CheckGoals();
            //questPanel.UpdateQuestsLog();
        }

        if (!isActive && isCompleted)         //quest skończony
        {
            //questPanel.RemoveQuestFromPanel(quest);
        }
    }

    private void Complete()
    {
        isActive = false;
        GameManager.Instance.PlayerObject.GetComponent<QuestLog>().RemoveQuest(this);
        Debug.Log("koniec questa ");
    }

    private void CompleteTask()
    {       
        isCompleted = false;

        InitializeTask(currentTask);

        Debug.Log("task wyzej");
    }

    public void CheckGoals(bool fromNpc = true)
    {
        if(currentTask < tasks.Count)
        {
            foreach (QuestGoal goal in tasks[currentTask].goals)
            {
                if (!goal.Completed) return;
            }

            if (fromNpc)
            {
                foreach(ItemGoal goal in tasks[currentTask].goals)
                {
                    goal.RemoveListeners();
                }

                currentTask++;

                if (currentTask == tasks.Count)
                    Complete();
                else
                    CompleteTask();
            }
            else
                isCompleted = true;

            Debug.Log("koniec zadania");
        }

        //if(currentTask == tasks.Count)
        //{

        //    if (fromNpc)
        //        Complete();

        //    isCompleted = true;

        //    Debug.Log("koniec questa " + currentTask + " " + tasks.Count);
        //}


        if(isCompleted)
            GameManager.Instance.PlayerObject.GetComponent<QuestLog>().ChangeQuestName();

    }

}
