using System;
using UnityEngine;
using UnityEngine.AI;

namespace jbzd.Enemies.Obsolete
{
    [Obsolete]
    public class EnemyDamagedEffect : MonoBehaviour
    {
        [SerializeField] private float pushBackStrength = 4f;
        private EnemyDataContainer EnemyData;
        private NavMeshAgent NavAgent;
        private bool effectTrigged = false;
        private float timer;
        [SerializeField] private float maxEffectDuration = .1f;
        private Rigidbody rb;

        void Start()
        {
            EnemyData = GetComponent<EnemyController>().EnemyData;
            NavAgent = GetComponent<EnemyController>().NavAgent;
            rb = GetComponent<Rigidbody>();
        }
        public void ApplyEffect()
        {
            
            Vector3 pushBackDirection = (transform.position - EnemyData.MainCharacterTransform.position);
            pushBackDirection = pushBackDirection.normalized;
            pushBackDirection *= pushBackStrength;
            rb.isKinematic = false;
            if(!NavAgent) NavAgent= GetComponent<EnemyController>().NavAgent;
            NavAgent.isStopped = true; ;
            effectTrigged = true;
            rb.AddForce(pushBackDirection, ForceMode.Impulse);
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
                    if(rb) rb.isKinematic = true;
                    NavAgent.velocity = Vector3.zero;
                    NavAgent.isStopped = false;
                    
                    //NavAgent.
                }
            }
        }

    }
}