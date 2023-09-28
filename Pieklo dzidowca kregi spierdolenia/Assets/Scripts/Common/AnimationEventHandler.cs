using UnityEngine;

namespace jbzd.Common
{
    /// <summary>
    /// Should be place on game object with animator. Helps to propagate an animation event.  
    /// </summary>
    public class AnimationEventHandler : MonoBehaviour
    {
        public delegate void EventFired(AnimationEvent animationEvent);
        public event EventFired OnEventFired;

        public void AnimationEventFiredHandler(AnimationEvent animationEvent) => OnEventFired?.Invoke(animationEvent);
    }
}