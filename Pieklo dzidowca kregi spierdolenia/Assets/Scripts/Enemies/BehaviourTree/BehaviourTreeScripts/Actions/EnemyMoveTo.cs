using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace jbzd.Enemies.BehaviourTree.Scripts.Actions
{
    [System.Serializable]
    public class EnemyMoveTo : ActionNode
    {
        protected override void OnStart() {
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {

            if (blackboard.moveEnemy)
            {
                if (blackboard.NavAgent.radius*2 <= blackboard.stopDistance)
                {
                    blackboard.stopDistance -= blackboard.NavAgent.radius*2;
                }
                else
                {
                    blackboard.stopDistance = 0f;
                }

                if (blackboard.NavAgent.stoppingDistance != blackboard.stopDistance)
                {
                    blackboard.NavAgent.stoppingDistance = blackboard.stopDistance;
                }


                if (blackboard.speed > 0)
                {
                    blackboard.NavAgent.speed = blackboard.speed;
                    blackboard.NavAgent.SetDestination(blackboard.moveToPosition);
                }
                else
                {
                    Vector3 direction = new Vector3(blackboard.moveToPosition.x - context.transform.position.x,
                                        0,
                                        blackboard.moveToPosition.z - context.transform.position.z);
                    direction = Vector3.Normalize(direction);
                    Vector3 newDirection = Vector3.RotateTowards(context.gameObject.transform.forward, direction, blackboard.EnemyData.RotationSpeed /10000, 0.0f);
                    context.transform.rotation = Quaternion.LookRotation(newDirection);
                }
                blackboard.moveEnemy = false;
                blackboard.moved = true;
            }

            return State.Success;
        }
    }
}