using jbzd.Enemies;
using UnityEngine;
using UnityEngine.AI;
using TheKiwiCoder;
using jbzd.Common.Interfaces;
using System.Collections;


namespace jbzd.Enemies.Level1.Gowniak
{
    public class GowniakController : EnemyController, IDamageable
    {
        [Header("Gowniak specific")]
        [SerializeField] private TheKiwiCoder.BehaviourTree tree;
        [SerializeField] private float spawnWanderRadius = 15f;
        [SerializeField] private float wanderEveryXSeconds = 3f;
        [SerializeField] private bool isInvincible;
        [SerializeField] private float invincibilityDurationSeconds;
        [SerializeField] private Material hitMaterial;
		[SerializeField] DamageController RHCollider = null;
		[SerializeField] DamageController LHCollider = null;
        private Context _context;
        private SkinnedMeshRenderer _meshRenderer;
        private Collider _collider;
        private EnemyDamagedEffect pushBackEffect;

        override protected void Start()
        {
            _context = CreateBehaviourTreeContext();
            tree = tree.Clone();
            tree.Bind(_context);

            base.Start();
            easyAnimator = new EasyAnimatorController(GetComponent<Animator>(), new string[] { });
            NavAgent = GetComponent<NavMeshAgent>();
			RHCollider.SetUp(EnemyData.Damage);
			LHCollider.SetUp(EnemyData.Damage);
            pushBackEffect = GetComponent<EnemyDamagedEffect>();
            
            tree.blackboard.NavAgent = NavAgent;
            tree.blackboard.easyAnimator = easyAnimator;
            tree.blackboard.RHCollider = RHCollider;
            tree.blackboard.LHCollider = LHCollider;
            tree.blackboard.spawnPoint = SpawnPoint;
            CurrentHealth = MaximumHealth;
            tree.blackboard.currentHealth = CurrentHealth;
            tree.blackboard.wanderEveryXSeconds = wanderEveryXSeconds;
            tree.blackboard.spawnWanderRadius = spawnWanderRadius;

            GetComponent<Animator>().SetFloat("idlingSpeed", Random.Range(0.5f, 1.5f));
            _meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
            Debug.Assert(_meshRenderer is not null, $"Mesh renderer component is missing on {name}");
            _collider = GetComponent<Collider>();
        }

        override protected void Update()
        {
            if (tree)
            {
                tree.Update();
            }
        }

        protected override bool HandleTriggerByEnemy(Vector3 target)
        {
            return false;
        }

        public override void SetDamage(int damageAmount, DamageType damageType)
        {
            if(isInvincible) return;
            
            CurrentHealth -= damageAmount;
            tree.blackboard.currentHealth = CurrentHealth;
            
            //TODO: tymczasowe oznaczenie zadawania obrazn
            StartCoroutine(BecomeHit());
            StartCoroutine(BecomeInvincible());
        }

        public override void SetDamage(int damageAmount, DamageType damageType, float criticalMultiplier, float criticalChance)
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

        Context CreateBehaviourTreeContext()
        {
            return Context.CreateFromGameObject(gameObject);
        }
    }
}