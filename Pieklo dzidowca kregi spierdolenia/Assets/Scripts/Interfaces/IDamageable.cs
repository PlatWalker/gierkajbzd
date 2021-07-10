///<summary>
///Created by Kumdzio
///</summary>


public interface IDamageable
{
    int MaximumHealth { get; }
    int CurrentHealth { get; }

    /// <summary>
    /// This method is called by damage dealer when it wants to deal damage to instance which is implementing
    /// this interface. In other words you have to implement this method if your object is receiving damage.
    /// When you want to deal damage to some object you just have to call this method taken from that object.
    /// </summary>
    void SetDamage(int damageAmount, DamageType damageType);

    /// <summary>
    /// Same as overloaded function but here you have to take care of critical multiplying damage.
    /// Standard operation for this is written in EnemyController.
    /// Value of critical chance have to be between 0.0f and 1.0f.
    /// </summary>
    void SetDamage(int damageAmount, DamageType damageType, float criticalMultiplier, float criticalChance);

}
