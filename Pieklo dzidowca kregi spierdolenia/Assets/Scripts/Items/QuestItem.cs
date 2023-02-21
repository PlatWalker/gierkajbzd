using System;
using jbzd.QuestSystem;
using jbzd.QuestSystem.QuestStructureElements;
using UnityEngine;
using Zenject;

namespace jbzd.Items
{
    public class QuestItem : Item
    {
        private QuestManager _questManager;
        [field: SerializeField] private GoalSO GoalToAct { get; set; }

        [Inject]
        public void Construct(QuestManager questManager)
        {
            _questManager = questManager;
        }
        
        public override void OnInteract()
        {
            base.OnInteract();
            
            _questManager.MakeActorPlay(GoalToAct);
        }
    }
}