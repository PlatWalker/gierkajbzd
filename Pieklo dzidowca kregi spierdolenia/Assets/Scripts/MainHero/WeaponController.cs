using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private int damageAmount = 10;
    private DamageType typeOfDamage = DamageType.CloseCombat;

    private float CritChance = 0.0f;
    private float CritMultiplier = 1.0f;

    private Animator characterAnimator;

    public void Awake()
    {
        characterAnimator = transform.parent.parent.parent.parent.parent.parent.parent.GetComponentInParent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<IDamageable>(out IDamageable hittenObjectScript) && characterAnimator.GetBool("Attack"))
        {
            hittenObjectScript.SetDamage(damageAmount, typeOfDamage, CritMultiplier, CritChance);
        }
    }
}
