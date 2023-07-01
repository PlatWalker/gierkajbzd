using System.Collections;
using jbzd.Common.Interfaces;
using TheKiwiCoder;
using UnityEngine;
using UnityEngine.Serialization;

namespace jbzd.Enemies.Level1.Pokrzywa
{
    [RequireComponent(typeof(Collider))]
    public class PokrzywaController : Enemy, IDamageable
    {
        [SerializeField] private BehaviourTree tree;
        [SerializeField] private EnemyDataContainer PokrzywaData = null;
        [SerializeField] private DamageController damageController;
        [SerializeField] private bool isInvincible;
        [SerializeField] private float invincibilityDurationSeconds;
        [SerializeField] private Material hitMaterial;
        public int MaximumHealth { get => PokrzywaData.MaxHealth; }
        [field:SerializeField]
        public int CurrentHealth { get; private set; }
        
        public override event EnemyDied OnDeath;
        
        private Context _context;
        private SkinnedMeshRenderer _meshRenderer;
        private Collider _collider;

        void Start()
        {
            _context = CreateBehaviourTreeContext();
            tree = tree.Clone();
            tree.Bind(_context);

            damageController = GetComponent<DamageController>();
            _meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
            Debug.Assert(_meshRenderer is not null, $"Mesh renderer component is missing on {name}");
            _collider = GetComponent<Collider>();
            
            damageController.SetUp(PokrzywaData.Damage);
            tree.blackboard.damageController = damageController;
            CurrentHealth = MaximumHealth;
            tree.blackboard.currentHealth = CurrentHealth;
            GetComponent<Animator>().SetFloat("idlingSpeed", Random.Range(0.5f, 1.5f));
            _meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
            _collider = GetComponent<Collider>();
        }

        void Update()
        {
            if (tree)
            {
                tree.Update();
            }
        }

        public void SetDamage(int damageAmount, DamageType damageType)
        {
            if(isInvincible) return;
            
            CurrentHealth -= damageAmount;
            tree.blackboard.currentHealth = CurrentHealth;

            if (CurrentHealth <= 0 )
            {
                _collider.enabled = false;
                OnDeath?.Invoke();
            }
            
            //TODO: tymczasowe oznaczenie zadawania obrazn
            StartCoroutine(BecomeHit());
            StartCoroutine(BecomeInvincible());
        }

        public void SetDamage(int damageAmount, DamageType damageType, float criticalMultiplier, float criticalChance)
        {
            if (Random.Range(0.0f, 1.0f) <= criticalChance)
            {
                damageAmount = (int)(damageAmount * criticalMultiplier);
            }
            SetDamage(damageAmount, damageType);
        }

        private IEnumerator BecomeHit()
        {
            var oldMaterial = _meshRenderer.material;
            
            _meshRenderer.material = hitMaterial;
            
            yield return new WaitForSeconds(invincibilityDurationSeconds);

            _meshRenderer.material = oldMaterial;
        }
        
        private IEnumerator BecomeInvincible()
        {
            isInvincible = true;
            yield return new WaitForSeconds(invincibilityDurationSeconds);
            isInvincible = false;
        }
        
        private Context CreateBehaviourTreeContext() {
            return Context.CreateFromGameObject(gameObject);
        }

        private void OnDrawGizmosSelected() {
            if (!tree) {
                return;
            }

            BehaviourTree.Traverse(tree.rootNode, (n) => {
                if (n.drawGizmos) {
                    n.OnDrawGizmos();
                }
            });
        }
    }
}