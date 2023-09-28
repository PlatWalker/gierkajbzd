using System.Collections.Generic;
using System.Linq;
using jbzd.Common;
using jbzd.Common.Interfaces;
using UnityEngine;

namespace jbzd.MainHero
{
    public class CanDealDamageWithWeapon : MonoBehaviour
    {
        [System.Serializable]
        public struct AnimationPairs {
            public AnimationClip animation;
            public BoxCollider colliderForDamage;
        }
        
        [Tooltip("Match colliders that are intended to deal damage and specify the animation during which this collider should be active.")]
        public List<AnimationPairs> animationPairsList;

        [Tooltip("If this character is player only enemies will be hit. If character is not player, it will hit player only.")]
        public bool isPlayer;
        
        [field:SerializeField]
        public int WeaponDamage { get; set; }

        private const string NAME_OF_TAG_FOR_COLLIDER = "ColliderForDamage";
        
#if UNITY_EDITOR
        public void OnValidate()
        {
            if (UnityEditorInternal.InternalEditorUtility.tags.Any(t => t == NAME_OF_TAG_FOR_COLLIDER)) return;
            
            Debug.LogError($"Missing '{NAME_OF_TAG_FOR_COLLIDER}' tag");
        }
#endif
        public void Start()
        {
            var animationEventHandler = GetComponentInChildren<AnimationEventHandler>();

            if (animationEventHandler is null)
            {
                Debug.LogError($"Missing {nameof(AnimationEventHandler)} in children object");
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
            
            animationEventHandler.OnEventFired += HandleEvent;
        }

        private void HandleEvent(AnimationEvent animationEvent)
        {
            var pairFromEvent = animationPairsList.FirstOrDefault(pair => pair.animation.name == animationEvent.animatorClipInfo.clip.name);

            if (pairFromEvent.animation is null || pairFromEvent.colliderForDamage is null)
            {
                Debug.LogError($"No animation in pair for passed animation event in {name}");
            }

            var damageCollider = pairFromEvent.colliderForDamage!;
            
            //10 is how much enemies we can hit at once. Why 10? I dont know, its random number. In case of emergency - change it xD
            var colliders = new Collider[10];
            var layerToGetHits = isPlayer ? "Enemies" : "Player";
            Physics.OverlapBoxNonAlloc(
                damageCollider.transform.TransformPoint(damageCollider.center),
                damageCollider.size / 2,
                colliders,
                damageCollider.transform.rotation,
                LayerMask.GetMask(layerToGetHits));

            foreach (var coll in colliders)
            {
                if(coll is null) return;

                coll.gameObject.TryGetComponent<IDamageable>(out var damageableEnemy);
                damageableEnemy.SetDamage(WeaponDamage, DamageType.CloseCombat);
            }
        }
    }
}