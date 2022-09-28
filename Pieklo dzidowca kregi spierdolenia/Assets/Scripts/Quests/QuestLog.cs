using System.Collections.Generic;

namespace jbzd.Quests
{
    public class QuestLogManager
    {
        private readonly List<Quest> _questList = new();

        public delegate void QuestChanged(List<Quest> activeQuests);
        public static event QuestChanged OnQuestChange;

        public void ChangeQuestName() => OnQuestChange?.Invoke(_questList);

        public Quest GetQuestNo(int index) => _questList[index];
        
        public void AddQuest(Quest quest)
        {
            _questList.Add(quest);
            OnQuestChange?.Invoke(_questList);
        }

        public void RemoveQuest(Quest quest)
        {
            _questList.Remove(quest);
            OnQuestChange?.Invoke(_questList);
        }

        public void CheckQuestObjective()
        {
            foreach(var quest in _questList)
            {
                quest.CheckGoals(false);
            }
        }
    }
}
