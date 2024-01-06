using jbzd.QuestSystem.QuestStructureElements;
using jbzd.Cutscenes;
using jbzd.QuestSystem;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using Zenject;
using MyBox;

namespace jbzd.QuestCreationScripts
{ 
    [RequireLayer("Player Triggers")]
    public class CutsceneTriggerOnGoal : MonoBehaviour
    {
        [field: SerializeField] private GoalSO _goal;
        
        public PlayableDirector playableDirector;
        private CutscenesManager _cutscenesManager;
        private QuestManager _questManager;

        [Inject]
        public void Construct(QuestManager questManager, CutscenesManager cutscenesManager)
        {
            _questManager = questManager;
            _cutscenesManager = cutscenesManager;
        }

        private void OnTriggerEnter(Collider other)
        {

            if (_goal != null &&
                other.gameObject.TryGetComponent<Actor>(out Actor actor) &&
                _goal.ActorsData.Contains(actor.ActorData))
            {
                _questManager.MakeActorPlay(_goal);
                TimelineAsset timelineAsset = playableDirector.playableAsset as TimelineAsset;
                _cutscenesManager.PlayCutscene(timelineAsset);
            }
        }
    }
}
