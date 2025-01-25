using System;
using System.Collections.Generic;
using System.Linq;
using jbzd.Common;
using jbzd.Common.Interfaces;
using MyBox;
using UnityEngine;
using UnityEngine.VFX;

namespace jbzd.Enemies.EnemiesComponents
{
    [RequireTag("Enemy")]
    [RequireLayer("Enemies")]
    public class CanAttackPlayer : MonoBehaviour
    {
        public BoxCollider colliderForDamage;
        
        [Serializable]
        public struct AnimationAttackConfig
        {
            [Tooltip("Is not used anywhere, it's just for nicely naming for readability in inspector")]
            public string friendlyConfigName;
            [Tooltip("Animation during damage will be dealt to player")]
            public AnimationClip animation;
            [Tooltip("Damage amount applied during animation")]
            public int animationDamage;
            [Tooltip("Vfx that will be fired on animation event with string variable \"vfx start\"")]
            public VisualEffect attackVfx;
        }
        
        [Tooltip("For each animation dealing damage, setup animation amount of damage and optionally attack vfx")]
        public List<AnimationAttackConfig> animationAttackConfigs;
        
        private AnimationEventHandler _animationEventHandler;
        
        public void Awake()
        {
            _animationEventHandler = GetComponentInChildren<AnimationEventHandler>();

            if (_animationEventHandler is null)
            {
                Debug.LogError($"Missing {nameof(AnimationEventHandler)} in children object");
                return;
            }

            if (animationAttackConfigs.Count == 0)
            {
                Debug.LogError($"No animation attack configs in {name}");
                return;
            }
            
            foreach (var config in animationAttackConfigs)
            {
                Debug.Assert(config.animation, $"Missing animation in config member in {gameObject.name}");
            }
            
            Debug.Assert(colliderForDamage, $"Missing collider for damage in {gameObject.name}");
            
            _animationEventHandler.OnEventFired += HandleEvent;
        }

        private void HandleEvent(AnimationEvent animationEvent)
        {
            var config = animationAttackConfigs.FirstOrDefault(config => config.animation.name == animationEvent.animatorClipInfo.clip.name);

            if (config.animation is null)
            {
                Debug.LogError($"Animation {animationEvent.animatorClipInfo.clip.name} have animation event " +
                               $"but there is no config for it in {gameObject.name}");
                return;
            }
            
            if (colliderForDamage is null) return;

            if (config.attackVfx && animationEvent.stringParameter is "vfx start")
            {
                config.attackVfx.Play();
                return;
            }
            
            //one collider only because there is only one player.
            var colliders = new Collider[1];
            Physics.OverlapBoxNonAlloc(
                colliderForDamage.transform.TransformPoint(colliderForDamage.center),
                Vector3.Scale(colliderForDamage.size, colliderForDamage.transform.lossyScale) * 0.5f,
                colliders,
                colliderForDamage.transform.rotation,
                LayerMask.GetMask("Player"));
            
            foreach (var coll in colliders)
            {
                if(coll is null) return;

                coll.gameObject.TryGetComponent<IDamageable>(out var damageableEnemy);

                if (damageableEnemy is null)
                {
                    Debug.LogError($"Hit target {coll.gameObject.name} does not implement {nameof(IDamageable)}, but it should");
                    return;
                }
                
                damageableEnemy.SetDamage(config.animationDamage, transform.position);
            }
        }

        public void OnDestroy()
        {
            _animationEventHandler.OnEventFired -= HandleEvent;
        }
    }
}