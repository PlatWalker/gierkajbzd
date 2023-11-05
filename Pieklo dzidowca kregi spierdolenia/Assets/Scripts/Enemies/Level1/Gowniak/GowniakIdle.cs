using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace jbzd.Enemies.Level1.Gowniak
{
    [System.Serializable]
    public class GowniakIdle : ActionNode
    {
        protected override void OnStart() {
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {

            if (blackboard.isIdling)
            {
                blackboard.easyAnimator.SetBooleanTrue("Idle");

                if (blackboard.playerIsVisible)
                {
                    if (blackboard.aggroCommenced)
                    {
                        blackboard.isChasing = true;
                        blackboard.isIdling = false;
                    }
                    else
                    {
                        blackboard.isAggroing = true;
                        blackboard.isIdling = false;
                    }
                }
            }

            return State.Success;
        }
    }
}