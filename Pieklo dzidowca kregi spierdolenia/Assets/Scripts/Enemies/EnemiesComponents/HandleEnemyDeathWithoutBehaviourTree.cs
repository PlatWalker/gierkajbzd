using System;
using System.Collections;
using UnityEngine;

namespace jbzd.Enemies.EnemiesComponents
{
    [RequireComponent(typeof(CanBeDamaged))]
    public class HandleEnemyDeathWithoutBehaviourTree: MonoBehaviour
    {
        private Animator _animator;
        
        [SerializeField]
        private float destroyAfterSeconds;

        private CanBeDamaged _canBeDamaged;
        private static readonly int Die = Animator.StringToHash("Die");

        public void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            Debug.Assert(_animator, $"missing animator in {gameObject.name}");
            
            _canBeDamaged = GetComponent<CanBeDamaged>();
            _canBeDamaged.OnDeath += OnOnDeath;
        }

        private void OnOnDeath(CanBeDamaged enemy)
        {
            _animator.SetTrigger(Die);
            StartCoroutine(DestroyObject());
        }

        private IEnumerator DestroyObject()
        {
            yield return new WaitForSeconds(destroyAfterSeconds);
            Destroy(gameObject);
            yield return null;
        }

        public void OnDestroy()
        {
            _canBeDamaged.OnDeath -= OnOnDeath;
        }
    }
}