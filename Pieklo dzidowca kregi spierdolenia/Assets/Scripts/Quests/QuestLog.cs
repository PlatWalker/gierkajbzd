using System.Collections.Generic;
using UnityEngine;

public class QuestLog : MonoBehaviour
{
    private List<Quest> questList;

    public delegate void OnQuestChange(List<Quest> activeQuests);
    public static event OnQuestChange onQuestChange;

    public void Initialize()
    {
        questList = new List<Quest>();
    }

    public void AddQuest(Quest quest)
    {
        questList.Add(quest);
        onQuestChange.Invoke(questList);
    }

    public void RemoveQuest(Quest quest)
    {
        Debug.Log("usuwa");
        questList.Remove(quest);
        onQuestChange.Invoke(questList);
    }

    public void CheckQuestObjective()
    {
        foreach(Quest quest in questList)
        {
            quest.CheckGoals(false);
        }
    }

    public void ChangeQuestName()
    {
        onQuestChange.Invoke(questList);
    } 

    public Quest getQuestNo(int index)
    {
            return questList[index];
    }
}
