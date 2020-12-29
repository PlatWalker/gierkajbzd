///<summary>
///Created by Kumdzio
///</summary>


public interface IDamageable
{

    int MaxHealth { get; }

    int CurrentHealth { get; }

    float GetHealthPercentage();

}
