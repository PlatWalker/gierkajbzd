using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// by SilverWalker
/// </summary>

public class WeaponController : MonoBehaviour
{
    private int damageAmount = 10;
    private DamageType typeOfDamage = DamageType.CloseCombat;

    private float CritChance = 0.0f;
    private float CritMultiplier = 1.0f;

    private Animator characterAnimator;

    public void Awake()
    {
        characterAnimator = GameManager.Instance.PlayerObject.GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<IDamageable>(out IDamageable hittenObjectScript) && characterAnimator.GetBool("Attack"))
        {
            hittenObjectScript.SetDamage(damageAmount, typeOfDamage, CritMultiplier, CritChance);
        }
    }
}
