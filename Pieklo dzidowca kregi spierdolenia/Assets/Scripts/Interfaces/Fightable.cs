///<summary>
///Created by Kumdzio
///</summary>


public interface Fightable 
{
    void SetDamage(int damageAmount, DamageTypes damageType);

    void SetDamage(int damageAmount, DamageTypes damageType, int criticalMultiplier, float criticalChance);
}
