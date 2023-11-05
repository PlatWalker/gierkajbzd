using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace jbzd.Enemies.Level1.Gowniak
{
    [System.Serializable]
    public class GowniakAttack : ActionNode
    {
        protected override void OnStart() {
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {

            if (blackboard.isAttacking)
            {    
                blackboard.easyAnimator.SetBooleanTrue("Attack");

                float clipTime = context.animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                if (clipTime%1 < 0.2f)
                {
                    blackboard.LHCollider.DamageDealed = false;
                    blackboard.RHCollider.DamageDealed = false;
                }

                if (!blackboard.playerIsVisible)
                {
                    blackboard.moveToPosition = context.transform.position;
                    blackboard.isWandering = true;
                    blackboard.isAttacking = false;
                    return State.Success;
                }
                if (blackboard.distanceToMainChar > blackboard.EnemyData.AttackRadius)
                {
                    blackboard.isChasing = true;
                    blackboard.isAttacking = false;
                }
            }

            return State.Success;
        }
    }
}