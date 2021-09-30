using System.Collections;
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

    //public void ChangeQuestState(Quest quest)
    //{
    //    questList.Remove(quest);
    //    completedQuest.Add(quest);
    //    onQuestChange.Invoke(questList, completedQuest);
    //}

    public Quest getQuestNo(int index)
    {
        //if (index < questList.Count)
            return questList[index];
        //else
        //    return completedQuest[index - questList.Count];
    }
}
