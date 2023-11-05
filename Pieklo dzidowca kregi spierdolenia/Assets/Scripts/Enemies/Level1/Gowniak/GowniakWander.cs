using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace jbzd.Enemies.Level1.Gowniak
{
    [System.Serializable]
    public class GowniakWander : ActionNode
    {
        protected override void OnStart() {
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {

            if (blackboard.isWandering)
            {
                blackboard.multiUseTimer += Time.deltaTime;
                
                blackboard.speed = blackboard.EnemyData.MovementSpeed;
                blackboard.stopDistance = blackboard.EnemyData.AttackRadius;
                if (!blackboard.moved)
                {
                    blackboard.moveEnemy = true;
                    return State.Success;
                }
                blackboard.moved = false;

                if (Vector3.Distance(context.transform.position, blackboard.moveToPosition) <= blackboard.EnemyData.AttackRadius)
                {
                    blackboard.easyAnimator.SetBooleanTrue("Idle");
                }
                else
                {
                    blackboard.easyAnimator.SetBooleanTrue("Move");
                }

                if (blackboard.playerIsVisible)
                {
                    blackboard.isChasing = true;
                    blackboard.isWandering = false;
                    return State.Success;
                }

                if (Vector3.Distance(context.transform.position, blackboard.spawnPoint) <= blackboard.spawnWanderRadius)
                {
                    blackboard.isIdling = true;
                    blackboard.isWandering = false;
                    return State.Success;
                }

                if (blackboard.multiUseTimer >= blackboard.wanderEveryXSeconds)
                {
                    blackboard.multiUseTimer = 0f;
                    blackboard.GenerateNewDestination = true;
                    blackboard.wanderTowardsSpawn = true;
                }
            }

            return State.Success;
        }
    }
}