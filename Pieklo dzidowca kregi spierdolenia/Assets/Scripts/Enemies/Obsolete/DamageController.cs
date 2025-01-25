using System;
using jbzd.Common.Interfaces;
using UnityEngine;

namespace jbzd.Enemies.Obsolete
{
    [Obsolete]
    public class DamageController : MonoBehaviour
    {
        private int _damage;
   
        public bool DamageDealed { get;  set; }

        public void SetUp(int newDamage)
        {
            _damage = newDamage;
        }
        public void SetUp(int newDamage, Vector3 damageOriginPoint)
        {
            SetUp(newDamage);
        }

        public void OnTriggerEnter(Collider collision)
        {
            DealDamage(collision);
        }

        public void OnTriggerStay(Collider other)
        {
            DealDamage(other);
        }

        public void OnCollisionEnter(Collision collision)
        {
            DealDamage(collision.collider);
        }

        public void OnCollisionStay(Collision collision)
        {
            DealDamage(collision.collider);
        }

        private void DealDamage(Collider collision)
        {
            if (DamageDealed) return;
            IDamageable hittenObjectScript;
            if (collision.gameObject.TryGetComponent<IDamageable>(out hittenObjectScript))
            {
                //TODO: zrobic kolizje za pomoca kolizji layerow a nie szukac po tagach
                if (collision.transform.tag == "Enemy") return;
                //Debug.Log("hit made by: " + transform.name + " to: " + collision.gameObject.name);
                hittenObjectScript.SetDamage(_damage, transform.position);
                DamageDealed = true;
            }
        }
    }
}
