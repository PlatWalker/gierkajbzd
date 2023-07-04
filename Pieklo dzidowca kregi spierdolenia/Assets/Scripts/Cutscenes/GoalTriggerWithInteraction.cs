using jbzd.Common.Interfaces;
using jbzd.QuestSystem;
using jbzd.QuestSystem.QuestStructureElements;
using UnityEngine;
using Zenject;

namespace jbzd.Cutscenes
{
    public class GoalTriggerWithInteraction : MonoBehaviour, IInteractable
    {
        [field:SerializeField]public GoalSO GoalToPlay { get; set; }

        private QuestManager _questManager;
        
        [Inject]
        public void Constructor(QuestManager questManager)
        {
            _questManager = questManager;
        }
        
        public void OnInteract()
        {
            _questManager.MakeActorPlay(GoalToPlay);
        }
    }
}