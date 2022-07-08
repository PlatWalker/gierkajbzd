using System.Collections.Generic;
using jbzd.Quests;
using UnityEngine;

//TODO Czy to na pewno ma byc przypiete do gracza? Moze jednak warto byloby dac to do gamemanagera/questmanager?
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

    public Quest GetQuestNo(int index)
    {
        return questList[index];
    }
}
