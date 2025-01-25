using UnityEngine;

namespace jbzd.Common.Interfaces
{
    public interface IDamageable
    {
        int MaximumHealth { get; }
        int CurrentHealth { get; }
        bool IsInvincible { get; }
        
        void SetDamage(int damageAmount, Vector3 attackPointOrigin);
    }
}
