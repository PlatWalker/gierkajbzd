///<summary>
///Created by Kumdzio
///</summary>


public interface IDamageable
{
    int GetHealthPercentage { get; }

    void SetDamage(int damageAmount, DamageType damageType);

    void SetDamage(int damageAmount, DamageType damageType, float criticalMultiplier, float criticalChance);

}
