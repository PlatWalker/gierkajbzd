using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace jbzd.Enemies.BehaviourTree.Scripts.Actions
{
    [System.Serializable]
    public class PlayerVisibilityCheck : ActionNode
    {
        protected override void OnStart() {
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {

            RaycastHit hit;
            LayerMask NotEnemiesMask = ~LayerMask.GetMask("Enemies");

            if (Physics.Raycast(context.transform.position + new Vector3(0f, 1f, 0f), blackboard.EnemyData.MainCharacterTransform.position - context.transform.position, out hit, blackboard.EnemyData.AggroRadius, NotEnemiesMask))
            {
                if (hit.transform == blackboard.EnemyData.MainCharacterTransform)
                {
                    blackboard.playerIsVisible = true;
                    return State.Success;
                }
            }
            blackboard.playerIsVisible = false;

            return State.Success;
        }
    }
}