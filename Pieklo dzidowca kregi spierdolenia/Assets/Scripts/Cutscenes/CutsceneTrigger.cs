using UnityEngine;
using jbzd.MainHero;
using UnityEngine.Playables;
using Zenject;

namespace jbzd.Cutscenes
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
                playableDirector.Play();
        }
        private void OnTriggerStay(Collider other)
        {
            if (other.tag == "Player")
                _manager.CanPlayerMove = false;
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player")
                _manager.CanPlayerMove = true;
        }
    }
}
