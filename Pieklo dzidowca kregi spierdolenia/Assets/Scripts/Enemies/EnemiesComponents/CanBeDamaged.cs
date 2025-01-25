using System.Collections;
using jbzd.Common.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace jbzd.Enemies.EnemiesComponents
{
    public class CanBeDamaged: MonoBehaviour, IDamageable
    {
        public delegate void EnemyDied(CanBeDamaged enemy);
        public event EnemyDied OnDeath;
        
        public int CurrentHealth
        {
            get => currentHealth;
            private set
            {
                _healthBar.fillAmount = (float) value / MaximumHealth;
                currentHealth = value;
            }
        }

        [field: SerializeField] public int MaximumHealth { get; private set; } = 1;
        [field:SerializeField] public bool IsInvincible { get; private set; }
        [field:SerializeField] public bool IsDead { get; private set; }
        
        [SerializeField] private int currentHealth;
        [SerializeField] private float invincibilityDuration = 0.4f;
        [SerializeField] private GameObject healthBarPrefab;

        private Image _healthBar;

        public void Awake()
        {
            var imagesComponents = Instantiate(healthBarPrefab, transform).GetComponentsInChildren<Image>();

            foreach (var image in imagesComponents)
            {
                if (image.type == Image.Type.Filled)
                {
                    _healthBar = image;
                }
            }
            
            CurrentHealth = MaximumHealth;
        }

        public void SetDamage(int damageAmount, Vector3 attackPointOfOrigin)
        {
            if(IsInvincible) return;
            
            CurrentHealth -= damageAmount;

            if (CurrentHealth <= 0)
            {
                Die();
            }
            
            StartCoroutine(ApplyInvincibility());
        }
        
        private IEnumerator ApplyInvincibility()
        {
            IsInvincible = true;
            yield return new WaitForSeconds(invincibilityDuration);
            IsInvincible = false;
        }

        private void Die()
        {
            if (IsDead) return;
            
            Debug.Log("Enemy: " + gameObject.name + " has died");
            IsDead = true;
            OnDeath?.Invoke(this);
        }
    }
}