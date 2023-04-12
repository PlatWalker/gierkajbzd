using System.Collections;
using System.Collections.Generic;
using jbzd.Common.Interfaces;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class IsDead : ActionNode
{
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {      
        if (blackboard.currentHealth > 0) 
        {
            blackboard.isDead = false;
        }
        else
        {
            blackboard.isDead = true;
        }

        return State.Success;
    }
}
