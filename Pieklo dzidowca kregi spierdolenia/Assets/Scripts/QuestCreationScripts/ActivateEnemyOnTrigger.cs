using System.Collections.Generic;
using jbzd.Common;
using jbzd.Enemies.EnemiesComponents;
using UnityEngine;

namespace jbzd.QuestCreationScripts
{
    [RequireComponent(typeof(Collider))]
    public class ActivateEnemyOnTrigger: MonoBehaviour
    {
        public List<ActiveBehaviorGraph> enemiesThatCanBeActived = new();

        [SerializeField]
        [JbzdReadOnly]
        private bool isTriggered;
        
        public void OnTriggerEnter(Collider other)
        {
            if (isTriggered) return;

            isTriggered = true;
            
            foreach (var activeBehaviorGraph in enemiesThatCanBeActived)
            {
                activeBehaviorGraph.MakeAgentActive();
            }
        }
    }
}