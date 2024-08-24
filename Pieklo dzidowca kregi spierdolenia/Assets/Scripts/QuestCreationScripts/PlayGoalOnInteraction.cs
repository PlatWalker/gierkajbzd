using jbzd.Common.Interfaces;
using jbzd.QuestSystem;
using jbzd.QuestSystem.QuestStructureElements;
using UnityEngine;
using Zenject;

namespace jbzd.QuestCreationScripts
{
    public class PlayGoalOnInteraction : MonoBehaviour, IInteractable
    {
        private QuestManager _questManager;
        
        [SerializeField]
        private GoalSO goalSo;
        
        [Inject]
        public void Constructor(QuestManager questManager)
        {
            _questManager = questManager;
        }

        public void OnInteract()
        {
            Debug.Assert(goalSo is not null, $"There is no goalSO attached to component in {gameObject.name}");
            
            _questManager.MakeActorPlay(goalSo);
        }
    }
}