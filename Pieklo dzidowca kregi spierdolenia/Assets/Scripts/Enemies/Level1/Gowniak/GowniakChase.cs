using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace jbzd.Enemies.Level1.Gowniak
{
    [System.Serializable]
    public class GowniakChase : ActionNode
    {
        protected override void OnStart() {
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {


            if (blackboard.isChasing)
            {
                blackboard.moveToPosition = blackboard.EnemyData.MainCharacterTransform.position;
                blackboard.speed = blackboard.EnemyData.MovementSpeed;
                blackboard.stopDistance = blackboard.EnemyData.AttackRadius;
                blackboard.moveEnemy = true;

                blackboard.easyAnimator.SetBooleanTrue("Move");

                if (blackboard.distanceToMainChar <= blackboard.EnemyData.AttackRadius)
                {
                    blackboard.isAttacking = true;
                    blackboard.isChasing = false;
                    return State.Success;
                }
                if (!blackboard.playerIsVisible)
                {
                    blackboard.moveToPosition = context.transform.position;
                    blackboard.isWandering = true;
                    blackboard.isChasing = false;
                }
            }

            return State.Success;
        }
    }
}