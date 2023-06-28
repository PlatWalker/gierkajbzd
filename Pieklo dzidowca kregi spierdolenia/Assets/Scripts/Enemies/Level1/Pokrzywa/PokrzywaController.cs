using jbzd.Common.Interfaces;
using TheKiwiCoder;
using UnityEngine;

namespace jbzd.Enemies.Level1.Pokrzywa
{
    public class PokrzywaController : Enemy, IDamageable
    {
        [SerializeField] private BehaviourTree tree;
        [SerializeField] private EnemyDataContainer PokrzywaData=null;
        [SerializeField] private DamageController damageController;
        public int MaximumHealth { get => PokrzywaData.MaxHealth; }
        [field:SerializeField]
        public int CurrentHealth { get; private set; }
        
        public override event EnemyDied OnDeath;
        
        private Context _context;

        void Start()
        {
            _context = CreateBehaviourTreeContext();
            tree = tree.Clone();
            tree.Bind(_context);

            damageController = GetComponent<DamageController>();
            damageController.SetUp(PokrzywaData.Damage);
            tree.blackboard.damageController = damageController;
            CurrentHealth = MaximumHealth;
            tree.blackboard.currentHealth = CurrentHealth;
            GetComponent<Animator>().SetFloat("idlingSpeed", Random.Range(0.5f, 1.5f));
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
            //for now damage types are ignored
            CurrentHealth -= damageAmount;
            tree.blackboard.currentHealth = CurrentHealth;

            if (CurrentHealth <= 0)
            {
                OnDeath?.Invoke();
            }
        }

        public void SetDamage(int damageAmount, DamageType damageType, float criticalMultiplier, float criticalChance)
        {
            if (Random.Range(0.0f, 1.0f) <= criticalChance)
            {
                damageAmount = (int)(damageAmount * criticalMultiplier);
            }
            this.SetDamage(damageAmount, damageType);
        }

        Context CreateBehaviourTreeContext() {
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