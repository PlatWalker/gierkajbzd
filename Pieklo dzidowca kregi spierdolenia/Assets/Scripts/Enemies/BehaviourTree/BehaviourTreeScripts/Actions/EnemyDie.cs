using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace jbzd.Enemies.BehaviourTree.Scripts.Actions
{
    [System.Serializable]
    public class EnemyDie : ActionNode
    {
        protected override void OnStart() {
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            if (blackboard.isDead == true)
            {
                if(blackboard.soundController) blackboard.soundController.PlayDying();

                if (blackboard.deathAnimationDone == false)
                {
                    context.animator.SetBool("isDying", true);
                    blackboard.deathAnimationDone = true;
                }
                blackboard.disappearTimer += Time.deltaTime;
                if (blackboard.disappearTimer >= blackboard.EnemyData.DisappearAfter) Object.Destroy(context.gameObject);
            }

            return State.Success;
        }
    }
}