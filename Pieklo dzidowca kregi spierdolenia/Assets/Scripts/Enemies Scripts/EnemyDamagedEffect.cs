using UnityEngine;
using UnityEngine.AI;

namespace jbzdy.Enemies
{
    public class EnemyDamagedEffect : MonoBehaviour
    {
        [SerializeField] private float pushBackStrength = 4f;
        private EnemyDataContainer EnemyData;
        private NavMeshAgent NavAgent;
        private bool effectTrigged = false;
        private float timer;
        [SerializeField] private float maxEffectDuration = .1f;
        private Rigidbody rigidbody;
        // Start is called before the first frame update
        void Start()
        {
            EnemyData = GetComponent<EnemyController>().EnemyData;
            NavAgent = GetComponent<EnemyController>().NavAgent;
            rigidbody = GetComponent<Rigidbody>();
        }
        public void ApplyEffect()
        {
            
            Vector3 pushBackDirection = (transform.position - EnemyData.MainCharacterTransform.position);
            pushBackDirection = pushBackDirection.normalized;
            pushBackDirection *= pushBackStrength;
            rigidbody.isKinematic = false;
            if(!NavAgent)
            NavAgent= GetComponent<EnemyController>().NavAgent;
            NavAgent.isStopped = true; ;
            effectTrigged = true;
            rigidbody.AddForce(pushBackDirection, ForceMode.Impulse);
        }
        private void Update()
        {
            if (effectTrigged)
            {
                timer += Time.deltaTime;
                if (timer > maxEffectDuration)
                {
                    timer = 0f;
                    effectTrigged = false;
                    if(rigidbody) rigidbody.isKinematic = true;
                    NavAgent.velocity = Vector3.zero;
                    NavAgent.isStopped = false;
                    
                    //NavAgent.
                }
            }
        }

    }
}