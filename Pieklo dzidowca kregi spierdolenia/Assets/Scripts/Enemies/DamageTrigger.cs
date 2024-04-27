using System.Collections;
using System.Collections.Generic;
using jbzd.Common.Interfaces;
using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
    private DamageControllersManager damageControllersManager;

    public void Initialize(DamageControllersManager damageControllersManager)
    {
        this.damageControllersManager = damageControllersManager;
    }

    public void OnTriggerEnter(Collider collision)
    {
        damageControllersManager.DealDamage(collision);
    }

    public void OnTriggerStay(Collider other)
    {
        damageControllersManager.DealDamage(other);
    }

    public void OnCollisionEnter(Collision collision)
    {
        damageControllersManager.DealDamage(collision.collider);
    }

    public void OnCollisionStay(Collision collision)
    {
        damageControllersManager.DealDamage(collision.collider);
    }


}
