using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "Assets/Resources/Quests/New Quest", menuName = "Quest")]
public class Quest : ScriptableObject 
{
    public bool isActive { get; set; }
    public bool isCompleted { get; set; }

    public string title;
    public string description;
    public int expReward;
    public int goldReward;

    public int startAtLevel;

    public List<Task> tasks = new List<Task>();

    public int currentTask { get; set; } = 0;

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
        if (isActive)     
        {
            CheckGoals();
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
                foreach (ItemGoal goal in tasks[currentTask].goals.OfType<ItemGoal>())
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

        if(isCompleted)
            GameManager.Instance.PlayerObject.GetComponent<QuestLog>().ChangeQuestName();

    }

}
