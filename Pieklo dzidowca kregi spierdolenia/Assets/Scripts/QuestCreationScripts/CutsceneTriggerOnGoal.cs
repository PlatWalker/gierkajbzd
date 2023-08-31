using jbzd.QuestSystem.QuestStructureElements;
using jbzd.QuestSystem;
using UnityEngine;
using UnityEngine.Playables;
using Zenject;

namespace jbzd.QuestCreationScripts
{
    public class CutsceneTriggerOnGoal : MonoBehaviour
    {
        [field: SerializeField] private GoalSO _goal;
        
        public PlayableDirector playableDirector;
        private QuestManager _questManager;

        [Inject]
        public void Construct(QuestManager questManager)
        {
            _questManager = questManager;
        }

        private void OnTriggerEnter(Collider other)
        {

            if (_goal != null &&
                other.gameObject.TryGetComponent<Actor>(out Actor actor) &&
                _goal.ActorsData.Contains(actor.ActorData))
            {
                _questManager.MakeActorPlay(_goal);
                playableDirector.Play();
            }
        }
    }
}
