using System;
using System.Collections.Generic;
using jbzd.Common;
using UnityEngine;
using UnityEngine.VFX;

namespace jbzd.Enemies.Level1.Gowniak
{
    public class ClawController: MonoBehaviour
    {
        [SerializeField] private List<VisualEffect> vfxs;

        private AnimationEventHandler _animationEventHandler;
        
        public void Start()
        {
            _animationEventHandler = GetComponentInChildren<AnimationEventHandler>();

            if (_animationEventHandler is null)
            {
                Debug.LogError($"Missing {nameof(AnimationEventHandler)} in children object");
                return;
            }
            
            _animationEventHandler.OnEventFired += HandleEvent;
        }

        private void HandleEvent(AnimationEvent animationEvent)
        {
            if (animationEvent.stringParameter is not "vfx start") return;
            foreach (var vfx in vfxs)
            {
                vfx.Play();
            }
        }

        public void OnDestroy()
        {
            _animationEventHandler.OnEventFired -= HandleEvent;
        }
    }
}