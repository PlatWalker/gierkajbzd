using System;
using System.Collections;
using jbzd.Common.Interfaces;
using jbzd.Enemies.Obsolete;
using UnityEngine;
using Zenject;
using jbzd.MainHero;
using static System.Math;
using jbzd.SavingSystem;
using Random = UnityEngine.Random;

namespace jbzd.Enemies.Level1.Pokrzywa
{
    [RequireComponent(typeof(Collider))]
    public class PokrzywaController : Enemy, IDamageable, ISaveable
    {
        [SerializeField] private TheKiwiCoder.BehaviourTree tree;
        [SerializeField] private EnemyDataContainer EnemyData = null;
        [SerializeField] private DamageController damageController;
        [SerializeField] private GameObject prefab;
        [SerializeField] private bool isInvincible;
        [SerializeField] private float invincibilityDurationSeconds;
        [SerializeField] private Material hitMaterial;
        
        [SerializeField]
        [Range(0.0f, 1.0f)]
        [Tooltip("Podstawowy czas rośnięcia pokrzyw")]
        private float baseTime;
        [SerializeField] 
        [Range(0.0f, 2.0f)]
        [Tooltip("Wielkość odchylenia od podstawowego czasu rośnięcia pokrzyw")]
        private float deviation;
        [SerializeField] 
        [Range(0.0f, 1.0f)]
        [Tooltip("Zwiększa czas rośnięcia pokrzyw o pierwiastek z ilości synów pomnożony przez ten parametr")]
        private float squareRootMultiplayer;
        [SerializeField] 
        [Range(0.0f, 1.0f)]
        [Tooltip("Zwiększa czas rośnięcia pokrzyw o ilość synów pomnożony przez ten parametr")]
        private float multiplayer;

        [SerializeField]
        [Range(0f, 10.0f)]
        [Tooltip("Długość trwania animacji rośnięcia")]
        private float moveDuration;

        [SerializeField]
        [Range(0.01f, 0.999f)]
        [Tooltip("Czas w trakcie którego pokrzywa nie rośnie, tylko leci")]
        private float whenStartGrowing = 0.8f;

        [SerializeField]
        [Range(0.01f, 0.999f)]
        [Tooltip("Początkowa wielkość pokrzywy wylatującej")]
        private float startingScale = 0.2f;

        [SerializeField]
        [Range(0.01f, 10f)]
        [Tooltip("Końcowa wielkość pokrzywy wylatującej")]
        private float endingScale = 1f;

        [SerializeField]
        [Range(0.01f, 10f)]
        [Tooltip("Wysokość skoku pokrzywy wylatującej")]
        private float jumpingHeight = 2.5f;

        [SerializeField]
        [Range(0.01f, 100f)]
        [Tooltip("Maksymalna odległość na jaką może się rozprzestrzenić pokrzywa")]
        private float maxSpreadDistance = 10f;

        private PlayerManager _playerManager;
        
        [Inject]
        public void Construct(PlayerManager playerManager)
        {
            _playerManager = playerManager;
        }
        
        public bool isMother = true;
        public int MaximumHealth => EnemyData.MaxHealth;

        [field:SerializeField]
        public int CurrentHealth { get; private set; }

        public bool IsInvincible { get; }

        public override event EnemyDied OnDeath;

        
        private TheKiwiCoder.Context _context;
        private SkinnedMeshRenderer _meshRenderer;
        private Collider _collider;
        private BoxCollider boxCollider;


        void Start()
        {

            _context = CreateBehaviourTreeContext();
            tree = tree.Clone();
            tree.Bind(_context);
            tree.blackboard._playerManager = _playerManager;
            tree.blackboard.isMother = isMother;
            tree.blackboard.Prefab = prefab;
            tree.blackboard.EnemyData = EnemyData;

            boxCollider = GetComponent<BoxCollider>();

            damageController = GetComponent<DamageController>();
            _meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
            Debug.Assert(_meshRenderer is not null, $"Mesh renderer component is missing on {name}");
            _collider = GetComponent<Collider>();
            
            damageController.SetUp(EnemyData.Damage);
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
            tree.blackboard.moveDuration = moveDuration;
            tree.blackboard.isInvincible = isInvincible;
            tree.blackboard.maxDimension = Mathf.Max(boxCollider.size.x, boxCollider.size.z)*endingScale;
            tree.blackboard.maxSpreadDistance = maxSpreadDistance;
            if (tree)
            {
                tree.Update();
            }
        }

        public void SetDamage(int damageAmount, Vector3 damageOriginPoint)
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


        private IEnumerator Grow(GameObject objectToMove, Vector3 startPosition, Vector3 targetPosition, float duration)
        {
            float elapsedTime = 0f;
            float growthspeed = (1-startingScale)/(1-whenStartGrowing);
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float journeyTime = elapsedTime / duration;
                objectToMove.transform.position = Vector3.Lerp(startPosition, targetPosition, journeyTime);
        
                objectToMove.transform.position = new Vector3(objectToMove.transform.position.x, objectToMove.transform.position.y + Mathf.Sin(journeyTime*3.14f)*jumpingHeight, objectToMove.transform.position.z);
                objectToMove.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, 1f);
                if(journeyTime < whenStartGrowing){
                    float multiplier = startingScale*endingScale;
                    objectToMove.transform.localScale = Vector3.one*multiplier;
                }
                else{
                    float multiplier = (startingScale+(journeyTime-whenStartGrowing)*growthspeed)*endingScale;
                    objectToMove.transform.localScale = Vector3.one*multiplier;
                }
                yield return null;
            }
            objectToMove.transform.position = targetPosition;
        }

        public void StartGrowing(Vector3 startPosition, Vector3 targetPosition, float duration)
        {
            StartCoroutine(Grow(gameObject, startPosition, targetPosition, duration));
        }
        public void LoadData(GameData gameData)
        {
            if (!isMother) Destroy(gameObject);
        }

        public void SaveData(ref GameData gameData) { }
        
    }
}