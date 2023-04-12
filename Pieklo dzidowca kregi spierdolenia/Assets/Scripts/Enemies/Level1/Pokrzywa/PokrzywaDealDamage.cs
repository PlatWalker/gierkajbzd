using System.Collections;
using System.Collections.Generic;
using jbzd.Common.Interfaces;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class PokrzywaDealDamage : ActionNode
{
    public float damageDealRatio = 1f;
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        blackboard.dealDamageTimer += Time.deltaTime;
        if (blackboard.dealDamageTimer >= damageDealRatio)
        {
            blackboard.damageController.DamageDealed = false;
            blackboard.dealDamageTimer = 0f;
        }
        return State.Success;
    }
}
