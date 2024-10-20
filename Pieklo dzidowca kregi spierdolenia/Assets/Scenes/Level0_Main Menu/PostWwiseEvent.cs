using UnityEngine;

namespace jbzd
{
    public class PostWwiseEvent : MonoBehaviour
    {
        public AK.Wwise.Event MyEvent;

        public void PlayEvent()
        {
            MyEvent.Post(gameObject);
        }

        public void StopEvent()
        {
            MyEvent.Stop(gameObject);
        }
    }
}
