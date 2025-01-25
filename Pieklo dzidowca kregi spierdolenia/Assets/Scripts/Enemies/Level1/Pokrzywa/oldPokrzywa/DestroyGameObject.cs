using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class DestroyGameObject : ActionNode
{
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        
        if (blackboard.isDead)
        {
            if (blackboard.deathAnimationDone == false)
            {
                context.animator.SetBool("isDying", true);
                blackboard.deathAnimationDone = true;
            }
            blackboard.disappearTimer += Time.deltaTime;
            if (blackboard.disappearTimer >= blackboard.EnemyData.DisappearAfter)Object.Destroy(this.context.gameObject);
        }

        return State.Success;
    }
}
