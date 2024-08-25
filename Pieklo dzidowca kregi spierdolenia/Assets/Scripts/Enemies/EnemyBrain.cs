using jbzd.Common.Interfaces;
using jbzd.MinorSystems.Cutscenes;
using jbzdy.Enemies;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Timeline;
using Zenject;



namespace jbzd.Enemies
{    
    public class EnemyBrain : MonoBehaviour, IDamageable
    {
        public delegate void EnemyDied(EnemyBrain enemy);
        public event EnemyDied OnDeath;
		public virtual int CurrentHealth { get; set; } = 10;
		public virtual int MaximumHealth { get; set; } = 10;

        private CutscenesManager _cutsceneManager;


        [Inject]
        public void Construct(CutscenesManager cutscenesManager)
        {  
            _cutsceneManager = cutscenesManager;
            _cutsceneManager.OnCutsceneStarted += Dissapear;
            _cutsceneManager.OnCutsceneEnded += Reappear;
        }

        public void Die()
        {
            Debug.Log("Enemy: " + this.gameObject.name + " has died");
            OnDeath?.Invoke(this);
            Destroy(this.gameObject);
        }

        public void Dissapear(TimelineAsset timelineAsset){
            gameObject.SetActive(false);
        }

        public void Reappear(TimelineAsset timelineAsset){
            gameObject.SetActive(true);
        }

        public virtual void SetDamage(int damageAmount, DamageType damageType)
        {
            CurrentHealth -= damageAmount;
        }

        public virtual void SetDamage(int damageAmount, DamageType damageType, float criticalMultiplier, float criticalChance)
        {
            if (Random.Range(0.0f, 1.0f) <= criticalChance)
            {
                damageAmount = (int)(damageAmount * criticalMultiplier);
            }
            this.SetDamage(damageAmount, damageType);
        }
    }

}