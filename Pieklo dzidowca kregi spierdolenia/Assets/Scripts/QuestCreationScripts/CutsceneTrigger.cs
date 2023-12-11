using UnityEngine;
using UnityEngine.Playables;

namespace jbzd.QuestCreationScripts
{ 
    [RequireComponent(typeof(Collider))]
    public class CutsceneTrigger : MonoBehaviour
    {
        public PlayableDirector playableDirector;

        public void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                playableDirector.Play();
            }
        }
    }
}
