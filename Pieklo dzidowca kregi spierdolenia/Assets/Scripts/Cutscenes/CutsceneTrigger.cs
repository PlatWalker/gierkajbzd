using UnityEngine;
using UnityEngine.Playables;

namespace jbzd.Cutscenes
{
    [RequireComponent(typeof(Collider))]
    public class CutsceneTrigger : MonoBehaviour
    {
        public PlayableDirector playableDirector;

        public void OnTriggerEnter(Collider other)
        {
            playableDirector.Play();
        }
    }
}
