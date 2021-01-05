///<summary>
///Created by Kumdzio
///</summary>


public interface IDamageable
{

    int MaxHealth { get; }

    int CurrentHealth { get; }

    float GetHealthPercentage();

    void SetDamage(int damageAmount, DamageType damageType);

    void SetDamage(int damageAmount, DamageType damageType, int criticalMultiplier, float criticalChance);

}
