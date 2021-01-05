///<summary>
///Created by Kumdzio
///</summary>

using UnityEngine;

public interface IFight : IDamageable
{

    Transform MainCharacterTransform { get; }

    float AggroRadius { get; }

    float AttackRadius { get; }

}
