using jbzd.MainHero;
using jbzd.QuestSystem;
using jbzd.QuestSystem.QuestStructureElements;
using UnityEngine;
using UnityEngine.Playables;
using Zenject;

namespace jbzd.QuestCreationScripts
{
    [RequireComponent(typeof(Collider))]
    public class CutsceneTriggerAfterQuest : MonoBehaviour
    {
        [field: SerializeField] private Quest quest; 
        [field: SerializeField] private TaskSO task;
        
        private QuestManager _questManager;
        [field: SerializeField] private GoalSO _goal;
        
        private PlayerManager _manager;
        public PlayableDirector playableDirector;
        
        [Inject]
        public void Construct(QuestManager questManager, PlayerManager playerManager)
        {
            _questManager = questManager;
            _manager = playerManager;
        }
        
        private bool Check()
        {
            if (task == null)
            {
                return quest.IsCompleted;
            }

            return quest.ActiveTask == task;
        }    
        
        private void OnTriggerEnter(Collider other)
        {
            
            if (_goal != null &&
                other.gameObject.TryGetComponent<Actor>(out Actor actor) &&
                _goal.ActorsData.Contains(actor.ActorData))
            {
                _questManager.MakeActorPlay(_goal);
                playableDirector.Play();
                return;
            }
            
            if (!Check()) return;
            if(other.CompareTag("Player")) playableDirector.Play();
        }
    }
}
