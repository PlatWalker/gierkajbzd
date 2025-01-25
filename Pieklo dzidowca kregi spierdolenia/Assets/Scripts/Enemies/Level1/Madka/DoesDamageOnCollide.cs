using System.Linq;
using jbzd.Common.Interfaces;
using UnityEngine;

namespace jbzd.Enemies.Level1.Madka
{
    [RequireComponent(typeof(Collider))]
    public class DoesDamageOnCollide: MonoBehaviour
    {
        [SerializeField]
        private int damageAmount = 5;
        
        private bool _doesCollided;
        
        public void OnCollisionEnter(Collision other)
        {
            if (_doesCollided) return;
            
            _doesCollided = true;

            if (other.gameObject.TryGetComponent<IDamageable>(out var player))
            {
                player.SetDamage(damageAmount, other.contacts.First().point);
            }
        }
    }
}