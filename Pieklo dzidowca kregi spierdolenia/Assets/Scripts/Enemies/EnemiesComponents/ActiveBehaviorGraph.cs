using System.Linq;
using Unity.Behavior;
using UnityEngine;

namespace jbzd.Enemies.EnemiesComponents
{
    [RequireComponent(typeof(BehaviorGraphAgent))]
    public class ActiveBehaviorGraph: MonoBehaviour
    {
        public void MakeAgentActive()
        {
            var behaviourGraphAgent = GetComponent<BehaviorGraphAgent>();

            var variable = behaviourGraphAgent.Graph.BlackboardReference.Blackboard.Variables.FirstOrDefault(variable => variable.Name == "isActive");

            if (variable is null)
            {
                Debug.LogError($"Missing variable isActive in blackboard of game object {gameObject.name}");
                return;
            }
            
            variable.ObjectValue = true;
        }
    }
}