using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace jbzd.Enemies.BehaviourTree.Scripts.Actions
{
    [System.Serializable]
    public class GenerateNewDestination : ActionNode
    {
        const int _tryXtimes = 10;

        protected override void OnStart() {
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {


            if (blackboard.GenerateNewDestination)
            {
                Vector3 newPoint = new Vector3(context.gameObject.transform.position.x, context.gameObject.transform.position.y, context.gameObject.transform.position.z);

                if (blackboard.wanderTowardsSpawn && (blackboard.tryCounter < _tryXtimes) && (Vector3.Distance(newPoint, blackboard.spawnPoint) >= Vector3.Distance(context.gameObject.transform.position, blackboard.spawnPoint)))
                {
                    blackboard.tryCounter++;
                    blackboard.generateNewPatrollingPoint = true;

                    return State.Success;

                }
                else if (blackboard.tryCounter < _tryXtimes && Vector3.Distance(newPoint, blackboard.spawnPoint) > blackboard.spawnWanderRadius)
                {
                    blackboard.tryCounter++;
                    blackboard.generateNewPatrollingPoint = true;

                    return State.Success;
                }

                if (blackboard.tryCounter == _tryXtimes)
                {
                    blackboard.tryCounter = 0;
                    if (blackboard.wanderTowardsSpawn)
                    {
                        if (Vector3.Distance(newPoint, blackboard.spawnPoint) > Vector3.Distance(context.gameObject.transform.position, blackboard.spawnPoint))
                        {
                            blackboard.moveToPosition = context.gameObject.transform.position;
                            blackboard.wanderTowardsSpawn = false;
                            
                            return State.Success;
                        }
                    }
                    else
                    {
                        if (Vector3.Distance(newPoint, blackboard.spawnPoint) > blackboard.spawnWanderRadius)
                        {
                            blackboard.moveToPosition = context.gameObject.transform.position;
                            return State.Success;
                        }
                    }
                }
                blackboard.moveToPosition = newPoint;
                blackboard.GenerateNewDestination = false;
                blackboard.moveEnemy = true;
            }

            return State.Success;
        }
    }
}