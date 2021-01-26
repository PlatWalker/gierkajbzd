using UnityEngine;
/// <summary>
/// Created by Kumdzio
/// </summary>
public class BulletController : MonoBehaviour
{
    private bool damageDealt;
    public float TimeToDestruction { get; private set; }
    private float timeAfterHit;
    public int DamageAmount { get; private set; }
    public DamageType TypeOfDamage { get; private set; }
    public float CritChance { get; private set; }
    public float CritMultiplier { get; private set; }


    public void SetUp(float timeToDestruction, int damageAmount, DamageType damageType)
    {
        DamageAmount = damageAmount;
        TimeToDestruction = timeToDestruction;
        TypeOfDamage = damageType;
    }
    public void SetUp(float timeToDestruction, int damageAmount, DamageType damageType,
        float critMultiplier, float critChance)
    {
        DamageAmount = damageAmount;
        TimeToDestruction = timeToDestruction;
        TypeOfDamage = damageType;
        CritChance = critChance;
        CritMultiplier = critMultiplier;
    }
    // Start is called before the first frame update
    void Start()
    {
        timeAfterHit = 0.0f;
        damageDealt = false;
        CritChance = 0.0f;
        CritMultiplier = 1.0f;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!damageDealt)
        {
            IDamageable hittenObjectScript;
            if(collision.gameObject.TryGetComponent<IDamageable>(out hittenObjectScript))
            {
                hittenObjectScript.SetDamage(DamageAmount, TypeOfDamage, CritMultiplier, CritChance);
            }
            damageDealt = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (damageDealt)
        {
            if (timeAfterHit >= TimeToDestruction)
            {
                Destroy(this.gameObject);
            }
            else
            {
                timeAfterHit += Time.deltaTime;
            }
        }
    }
}
