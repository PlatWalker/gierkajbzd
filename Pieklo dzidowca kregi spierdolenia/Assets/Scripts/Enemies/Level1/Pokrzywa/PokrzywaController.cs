using System.Collections;
using jbzd.Common.Interfaces;
using UnityEngine;
using Zenject;
using jbzd.MainHero;
using jbzd.SavingSystem;
using Random = UnityEngine.Random;

namespace jbzd.Enemies.Level1.Pokrzywa
{
    [RequireComponent(typeof(Collider))]
    public class PokrzywaController : Enemy, IDamageable, ISaveable
    {
        [SerializeField] private TheKiwiCoder.BehaviourTree tree;
        [SerializeField] private EnemyDataContainer PokrzywaData = null;
        [SerializeField] private DamageController damageController;
        [SerializeField] private bool isInvincible;
        [SerializeField] private float invincibilityDurationSeconds;
        [SerializeField] private Material hitMaterial;



        
        [SerializeField]
        [Range(0.0f, 10.0f)]
        [Tooltip("Wzór: baseTime + Sqrt(Ilość_synów) * squareRootMultiplayer + Ilość_synów * multiplayer + Random.Range(-deviation, deviation)")]
        private float baseTime;
        [SerializeField] 
        [Range(0.0f, 10.0f)]
        [Tooltip("Wzór: baseTime + Sqrt(Ilość_synów) * squareRootMultiplayer + Ilość_synów * multiplayer + Random.Range(-deviation, deviation)")]
        private float deviation;
        [SerializeField] 
        [Range(0.0f, 5.0f)]
        [Tooltip("Wzór: baseTime + Sqrt(Ilość_synów) * squareRootMultiplayer + Ilość_synów * multiplayer + Random.Range(-deviation, deviation)")]
        private float squareRootMultiplayer;
        [SerializeField] 
        [Range(0.0f, 1.0f)]
        [Tooltip("Wzór: baseTime + Sqrt(Ilość_synów) * squareRootMultiplayer + Ilość_synów * multiplayer + Random.Range(-deviation, deviation)")]
        private float multiplayer;


        private PlayerManager _playerManager;
        
        [Inject]
        public void Construct(PlayerManager playerManager)
        {
            _playerManager = playerManager;
        }
        
        public bool isMother = true;
        public int MaximumHealth => PokrzywaData.MaxHealth;

        [field:SerializeField]
        public int CurrentHealth { get; private set; }
        
        public override event EnemyDied OnDeath;

        
        private TheKiwiCoder.Context _context;
        private SkinnedMeshRenderer _meshRenderer;
        private Collider _collider;

        void Start()
        {

            _context = CreateBehaviourTreeContext();
            tree = tree.Clone();
            tree.Bind(_context);
            tree.blackboard._playerManager = _playerManager;
            tree.blackboard.isMother = isMother;

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
            tree.blackboard.deviation = deviation;
            tree.blackboard.baseTime = baseTime;
            tree.blackboard.multiplayer = multiplayer;
            tree.blackboard.squareRootMultiplayer = squareRootMultiplayer;
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
        
        private TheKiwiCoder.Context CreateBehaviourTreeContext() {
            return TheKiwiCoder.Context.CreateFromGameObject(gameObject);
        }

        private void OnDrawGizmosSelected() {
            if (!tree) {
                return;
            }

            TheKiwiCoder.BehaviourTree.Traverse(tree.rootNode, (n) => {
                if (n.drawGizmos) {
                    n.OnDrawGizmos();
                }
            });
        }

        public void LoadData(GameData gameData)
        {
            if (!isMother) Destroy(gameObject);
        }

        public void SaveData(ref GameData gameData) { }
    }
}