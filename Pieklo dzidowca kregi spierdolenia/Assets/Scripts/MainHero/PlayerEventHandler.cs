using UnityEngine;

namespace jbzd.MainHero
{
    public class PlayerEventHandler : MonoBehaviour
    {
        public delegate void EventFired();
        public event EventFired OnEventFired;

        public void WeaponDamageEventFired() => OnEventFired?.Invoke();
    }
}