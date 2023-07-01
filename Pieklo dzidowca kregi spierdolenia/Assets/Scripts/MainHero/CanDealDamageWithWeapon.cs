using System.Linq;
using jbzd.Common.Interfaces;
using UnityEngine;

namespace jbzd.MainHero
{
    public class CanDealDamageWithWeapon : MonoBehaviour
    {
        private BoxCollider _collider;
        [field:SerializeField]
        public int WeaponDamage { get; set; }
        public void Start()
        {
            var animationEventHandler = GetComponentInChildren<PlayerEventHandler>();

            var trailCollider = GetComponentsInChildren<BoxCollider>().FirstOrDefault(coll => coll.CompareTag("ColliderForDamage"));
            
            if (animationEventHandler is null)
            {
                Debug.LogError($"Missing {nameof(PlayerEventHandler)} in children object");
                return;
            }
            
            if (trailCollider is null)
            {
                Debug.LogError("Missing game object with tag 'ColliderForDamage' in children");
                return;
            }

            _collider = trailCollider;
            animationEventHandler.OnEventFired += HandleEvent;
        }

        private void HandleEvent()
        {
            //10 is how much enemies we can hit at once. Why 10? I dont know, its random number. In case of emergency - change it xD
            var colliders = new Collider[10];
            Physics.OverlapBoxNonAlloc(
                _collider.transform.TransformPoint(_collider.center),
                _collider.size / 2,
                colliders,
                _collider.transform.rotation,
                LayerMask.GetMask("Enemies"));

            foreach (var coll in colliders)
            {
                if(coll is null) return;

                coll.gameObject.TryGetComponent<IDamageable>(out var damageableEnemy);
                damageableEnemy.SetDamage(WeaponDamage, DamageType.CloseCombat);
            }
        }
    }
}