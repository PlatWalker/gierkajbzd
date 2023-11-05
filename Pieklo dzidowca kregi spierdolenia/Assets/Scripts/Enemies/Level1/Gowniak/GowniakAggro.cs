using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;


namespace jbzd.Enemies.Level1.Gowniak
{
    [System.Serializable]
    public class GowniakAggro : ActionNode
    {
        private float _aggroMaxTime = 2f;
        protected override void OnStart() {
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {


            if (blackboard.isAggroing)
            {
                blackboard.multiUseTimer += Time.deltaTime;
                blackboard.easyAnimator.SetBooleanTrue("Aggro");

                if (blackboard.multiUseTimer > _aggroMaxTime)
                {
                    blackboard.multiUseTimer = 0;
                    blackboard.isChasing = true;
                    blackboard.aggroCommenced = true;
                    blackboard.isAggroing = false;
                }
                if (!blackboard.playerIsVisible)
                {
                    blackboard.multiUseTimer = 0;
                    blackboard.isIdling = true;
                    blackboard.isAggroing = false;
                }
            }

            return State.Success;
        }
    }
}