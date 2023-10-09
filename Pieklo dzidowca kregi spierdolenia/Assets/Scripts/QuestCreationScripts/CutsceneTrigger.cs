using jbzd.MainHero;
using UnityEngine;
using UnityEngine.Playables;
using Zenject;

namespace jbzd.QuestCreationScripts
{ 
    [RequireComponent(typeof(Collider))]
    public class CutsceneTrigger : MonoBehaviour
    {
        private PlayerManager _manager;
        public PlayableDirector playableDirector;
        
        [Inject]
        public void Construct(PlayerManager manager)
        {
            _manager = manager;
        }
        public void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                playableDirector.Play();
            }
        }
    }
}
