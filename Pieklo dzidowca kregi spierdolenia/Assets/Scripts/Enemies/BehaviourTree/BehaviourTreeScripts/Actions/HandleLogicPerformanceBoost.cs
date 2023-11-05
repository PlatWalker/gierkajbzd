using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace jbzd.Enemies.BehaviourTree.Scripts.Actions
{
    [System.Serializable]
    public class HandleLogicPerformanceBoost : ActionNode
    {
        protected override void OnStart() {
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {

            //Beta version of performance booster
            blackboard.updateLogicFrame = false;

            if (blackboard.framesCounter == blackboard.EnemyData.UpdateLogicEveryXFrames)
            {
                blackboard.framesCounter = 0;
                blackboard.updateLogicFrame = true;
            }
            else
            {
                blackboard.framesCounter++;
            }

            //code below is strongly undebuggable -you have to remember that distance to main char is 
            //updating/counted again only every (see: updateLogicEveryXFrames) frames
            if (blackboard.updateLogicFrame)
            {
                blackboard.distanceToMainChar = Vector3.Distance(context.gameObject.transform.position, blackboard.EnemyData.MainCharacterTransform.position);
            }

            return State.Success;
        }
    }
}