using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour
{
    private int damage=0;
    private DamageType damageType=DamageType.CloseCombat;
    private float criticalMultiplier = 0;
    private float criticalChance = 0;
    public bool DamageDealed { get;  set; } = false;

    public void SetUp(int newDamage)
    {
        damage = newDamage;
    }
    public void SetUp(int newDamage,DamageType newDamageType)
    {
        SetUp(newDamage);
        damageType = newDamageType;
    }
    public void SetUp(int newDamage, DamageType newDamageType,float newCritChance, float newCritMultiplier)
    {
        SetUp(newDamage, newDamageType);
        criticalChance = newCritChance;
        criticalMultiplier = newCritMultiplier;
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (DamageDealed) return;
        IDamageable hittenObjectScript;
        if (collision.gameObject.TryGetComponent<IDamageable>(out hittenObjectScript))
        {
            if (collision.transform.tag == "Enemy") return;
            //Debug.Log("hit made by: " + transform.name);
            hittenObjectScript.SetDamage(damage, damageType,criticalMultiplier,criticalChance);
            DamageDealed = true;
        }
    }
  
}
