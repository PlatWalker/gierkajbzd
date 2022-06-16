using System.Collections;
using System.Collections.Generic;
using jbzd.Common.Interfaces;
using UnityEngine;

namespace jbzdy.Enemies
{
    public class PokrzywaController : MonoBehaviour, IDamageable
    {
        [SerializeField] private EnemyDataContainer PokrzywaData=null;
        [SerializeField] private float damageDealRatio = 1f;
        [SerializeField] private DamageController damageController;
        public int MaximumHealth { get => PokrzywaData.MaxHealth; }
        public int CurrentHealth { get; private set; }

        private float damageTimer = 0.0f;
        private float disappearTimer = 0.0f;
        // Start is called before the first frame update
        void Start()
        {
            damageController = GetComponent<DamageController>();
            damageController.SetUp(PokrzywaData.Damage);
            CurrentHealth = MaximumHealth;
            GetComponent<Animator>().SetFloat("idlingSpeed", Random.Range(0.5f, 1.5f));
        }

        // Update is called once per frame
        void Update()
        {
            if (CurrentHealth > 0)
            {
                damageTimer += Time.deltaTime;
                if (damageTimer >= damageDealRatio)
                {
                    damageController.DamageDealed = false;
                    damageTimer = 0f;
                }
            }
            else
            {
                GetComponent<Animator>().SetBool("isDying",true);
                disappearTimer += Time.deltaTime;
                if (disappearTimer >= PokrzywaData.DisappearAfter) Destroy(this.gameObject);
            }
        }

        private void HandleGrownig()
        {

        }

        public void SetDamage(int damageAmount, DamageType damageType)
        {
            //for now damage types are ignored
            CurrentHealth -= damageAmount;
        }

        public void SetDamage(int damageAmount, DamageType damageType, float criticalMultiplier, float criticalChance)
        {
            if (Random.Range(0.0f, 1.0f) <= criticalChance)
            {
                damageAmount = (int)(damageAmount * criticalMultiplier);
            }
            this.SetDamage(damageAmount, damageType);
        }
    }
}