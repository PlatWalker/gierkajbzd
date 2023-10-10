using System;
using System.Linq;
using UnityEngine;
using Zenject;

namespace jbzd.QuestSystem.QuestStructureElements
{
    public class Actor : MonoBehaviour
    {
        [field:SerializeField]
        public ActorSO ActorData { get; set; }

        private QuestManager _questManager;
        
        [Inject]
        public void Constructor(QuestManager questManager)
        {
            _questManager = questManager;
        }

        private void Start()
        {
            var questsWithThisActor =
                _questManager.ActiveQuests.FindAll(quest => quest.QuestData.IsThereAnActor(ActorData));

            foreach (var quest in questsWithThisActor)
            {
                quest.Actors.Add(this);
            }

            _questManager.OnQuestActivation += activatedQuest =>
            {
                if (activatedQuest.QuestData.IsThereAnActor(ActorData) && activatedQuest.Actors.All(actor => actor != this))
                {
                    activatedQuest.Actors.Add(this);
                }
            };
        }
    }
}