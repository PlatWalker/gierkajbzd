using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using jbzd.Common;
using jbzd.Common.Interfaces;
using MyBox;
using UnityEngine;
using UnityEngine.VFX;

namespace jbzd.MainHero
{
    [RequireTag("Player")]
    public class CanDealDamageWithWeapon : MonoBehaviour
    {
        [Serializable]
        public struct AnimationPairs {
            public AnimationClip animation;
            public BoxCollider colliderForDamage;
        }
        
        [Tooltip("Match colliders that are intended to deal damage and specify the animation during which this collider should be active.")]
        public List<AnimationPairs> animationPairsList;

        [SerializeField]
        private VisualEffect firstComboPartEffect;
        [SerializeField]
        private VisualEffect secondComboPartEffect;
        [SerializeField]
        private VisualEffect thirdComboPartEffect;
        
        [field:SerializeField]
        public int WeaponDamage { get; set; }

        [field: Range(0, 0.8f)]
        [field: SerializeField]
        private float AnimationFreezeDurationSeconds { get; set; }
        
        private AnimationEventHandler _animationEventHandler;
        
        public void Start()
        {
            _animationEventHandler = GetComponentInChildren<AnimationEventHandler>();

            if (firstComboPartEffect is null || secondComboPartEffect is null || thirdComboPartEffect is null)
            {
                Debug.LogError("Missing one visual effect for combo");
                return;
            }
            
            if (_animationEventHandler is null)
            {
                Debug.LogError($"Missing {nameof(AnimationEventHandler)} in children object");
                return;
            }

            if (animationPairsList.Count == 0)
            {
                Debug.LogError($"No animation in pair for passed animation event in {name}");
                return;
            }
            
            foreach (var pair in animationPairsList)
            {
                if (pair.animation is null || pair.animation?.ToString() == "null")
                {
                    Debug.LogError($"Missing animation in pair in {name}");
                    return;
                }
                
                if (pair.colliderForDamage is null || pair.colliderForDamage?.ToString() == "null")
                {
                    Debug.LogError($"Missing collider for damage in pair in {name}");
                    return;
                }
            }
            
            _animationEventHandler.OnEventFired += HandleEvent;
        }

        private void HandleEvent(AnimationEvent animationEvent)
        {
            if (animationEvent.stringParameter is "vfx start")
            {
                if (animationEvent.intParameter is 0 or < 0)
                {
                    Debug.LogError($"Animation event handled in {gameObject} has string \"vfx start\" string but does not have positive int number parameter!");
                    return;
                }
                
                switch (animationEvent.intParameter)
                {
                    case 1:
                        firstComboPartEffect.Play();
                        break;
                    case 2:
                        secondComboPartEffect.Play();
                        break;
                    case 3:
                        thirdComboPartEffect.Play();
                        break;
                    default:
                        Debug.LogError($"There is no step {animationEvent.intParameter} in this combo. Change animation event int parameter");
                        break;
                }
                
            }
            
            var pairFromEvent = animationPairsList.FirstOrDefault(pair => pair.animation.name == animationEvent.animatorClipInfo.clip.name);

            if (pairFromEvent.colliderForDamage is null)
            {
                Debug.LogError($"{gameObject.name} has missing collider for damage or is mismatch between component and animation node in animation controller");
                return;
            }
            
            var damageCollider = pairFromEvent.colliderForDamage;
            
            //10 is how much enemies can holder of script hit at once. Why 10? I dont know, its random number. In case of emergency - change it xD
            var colliders = new Collider[10];
            
            Physics.OverlapBoxNonAlloc(
                damageCollider.transform.TransformPoint(damageCollider.center),
                Vector3.Scale(damageCollider.size, damageCollider.transform.lossyScale) * 0.5f,
                colliders,
                damageCollider.transform.rotation,
                LayerMask.GetMask("Enemies"));
            
            foreach (var coll in colliders)
            {
                if(coll is null) return;
                
                coll.gameObject.TryGetComponent<IDamageable>(out var damageableEnemy);

                if (damageableEnemy is null)
                {
                    Debug.LogError($"Hit target {coll.gameObject.name} does not implement {nameof(IDamageable)}, but it should");
                    return;
                }

                if (!damageableEnemy.IsInvincible)
                {
                    var animator = GetComponentInChildren<Animator>();
                    StartCoroutine(AnimationFreeze(animator));
                }
                
                damageableEnemy.SetDamage(WeaponDamage, gameObject.transform.position);
            }

            return;

            IEnumerator AnimationFreeze(Animator animator)
            {
                animator.speed = 0;
                yield return new WaitForSeconds(AnimationFreezeDurationSeconds);
                animator.speed = 1;
            }
        }

        private void OnDestroy()
        {
            _animationEventHandler.OnEventFired -= HandleEvent;
        }
    }
}