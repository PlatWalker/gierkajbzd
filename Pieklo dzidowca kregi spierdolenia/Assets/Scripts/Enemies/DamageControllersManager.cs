using System.Collections;
using System.Collections.Generic;
using jbzd.Common.Interfaces;
using UnityEngine;

public class DamageControllersManager : MonoBehaviour
{
    private int damage=0;
    private DamageType damageType=DamageType.CloseCombat;
    private float criticalMultiplier = 0;
    private float criticalChance = 0; 



    public List<DamageTrigger> DamageTriggers = new List<DamageTrigger>();
    public bool DamageDealed = false;



    private void Awake()
    {
        foreach (var trigger in DamageTriggers)
        {
            trigger.Initialize(this);
        }
    }
    public void SetUp(int newDamage, DamageType newDamageType,float newCritChance, float newCritMultiplier)
    {
        damage = newDamage;
        damageType = newDamageType;
        criticalChance = newCritChance;
        criticalMultiplier = newCritMultiplier;
    }

    public void DealDamage(Collider collision)
    {
        if(DamageDealed) return;
        IDamageable hittenObjectScript;
        if (collision.gameObject.TryGetComponent<IDamageable>(out hittenObjectScript))
        {
            if (collision.transform.tag == "Enemy") return;
            hittenObjectScript.SetDamage(damage, damageType, criticalMultiplier, criticalChance);
            DamageDealed = true;
        }
    }
}
