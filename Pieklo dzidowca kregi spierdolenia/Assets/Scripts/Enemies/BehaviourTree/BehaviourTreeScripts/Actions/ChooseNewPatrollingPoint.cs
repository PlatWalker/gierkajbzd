using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace jbzd.Enemies.BehaviourTree.Scripts.Actions
{    
    [System.Serializable]
    public class ChooseNewPatrollingPoint : ActionNode
    {
        protected override void OnStart() {
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {


            if (blackboard.generateNewPatrollingPoint)
            {
                Vector3 newPoint = Vector3.zero;
                bool correctPoint = false;
                RaycastHit hit;

                for (int i = 5; i > 0; i--)
                {
                    newPoint = new Vector3( Random.Range(context.transform.position.x - blackboard.EnemyData.PatrolMaxDistance, context.transform.position.x + blackboard.EnemyData.PatrolMaxDistance),
                                            context.transform.position.y,
                                            Random.Range(context.transform.position.z - blackboard.EnemyData.PatrolMaxDistance, context.transform.position.z + blackboard.EnemyData.PatrolMaxDistance));

                    if (Physics.Raycast((context.transform.position + new Vector3(0f, 1f, 0f)), (newPoint - context.transform.position), out hit, blackboard.EnemyData.AggroRadius))
                    {
                        if (hit.collider.transform.tag == "Terrain")
                        {
                            correctPoint = true;
                            break;
                        }
                    }
                    else
                    {
                        //Dont know why but collinding with terrain dont return its tag
                        correctPoint = true;
                        break;
                    }
                }
                if (!correctPoint) newPoint = context.transform.position;
                blackboard.moveToPosition = newPoint;
                blackboard.generateNewPatrollingPoint = false;
            }

            return State.Success;
        }
    }
}