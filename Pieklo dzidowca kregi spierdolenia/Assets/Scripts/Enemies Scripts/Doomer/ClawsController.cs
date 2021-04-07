using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawsController : MonoBehaviour
{
    private int damage=0;
    private DamageType damageType=DamageType.CloseCombat;
    //private bool canDealDamage = false;
    public void SetUp(int newDamage)
    {
        damage = newDamage;
    }

    public void OnTriggerEnter(Collider collision)
    {
        IDamageable hittenObjectScript;
        if (collision.gameObject.TryGetComponent<IDamageable>(out hittenObjectScript))
        {
            hittenObjectScript.SetDamage(damage, damageType);
        }
        //canDealDamage = false;
    }

    public void CanDealDamage(bool canIt)
    {
        //canDealDamage = canIt;
    }
  
}
