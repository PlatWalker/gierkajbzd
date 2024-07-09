using System;
using jbzd.QuestSystem;
using jbzd.QuestSystem.QuestStructureElements;
using UnityEngine;
using Zenject;

namespace jbzd.QuestCreationScripts
{
    public class GoalTriggerOnCollision: MonoBehaviour
    {
        private QuestManager _questManager;
        [field:SerializeField]
        public GoalSO Goal { get; set; }
        [Inject]
        public void Constructor(QuestManager questManager)
        {
            _questManager = questManager;
        }
        
        public void OnTriggerEnter(Collider other)
        {
            _questManager.MakeActorPlay(Goal);
        }
    }
}