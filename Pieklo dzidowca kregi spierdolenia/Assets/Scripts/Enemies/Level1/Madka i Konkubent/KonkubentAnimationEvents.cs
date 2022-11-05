using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KonkubentAnimationEvents : MonoBehaviour
{
    public bool aggroAnimDone { get; set; }

    public bool moveForward { get; set; }

    public bool chargeLoop { get; set; }

    public bool baseballHit { get; set; }

    private void FinishAggroAnim() => aggroAnimDone = true;

    private void MoveForward() => moveForward = true;

    private void ChargeLoop() => chargeLoop = true;

    private void BaseballHit() => baseballHit = true;
}
