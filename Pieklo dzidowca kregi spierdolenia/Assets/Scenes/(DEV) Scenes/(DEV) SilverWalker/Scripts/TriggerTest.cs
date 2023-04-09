using System;
using jbzd.MainHero;
using jbzd.QuestSystem;
using jbzd.QuestSystem.QuestStructureElements;
using UnityEngine;
using Zenject;

namespace jbzd
{
    public class TriggerTest : MonoBehaviour
    {
        private QuestManager _questManager;

        public GoalSO goalToAct;
        
        [Inject]
        public void Constructor(QuestManager questManager)
        {
            _questManager = questManager;
        }
        
        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent<PlayerManager>(out _))
            {
                _questManager.MakeActorPlay(goalToAct);
            }
        }
    }
}
