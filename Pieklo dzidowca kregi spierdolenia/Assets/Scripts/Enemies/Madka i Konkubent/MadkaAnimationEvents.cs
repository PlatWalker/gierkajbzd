using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MadkaAnimationEvents : MonoBehaviour
{
    public bool throwAnimationFinished { get; set; }
    private void ThrowAnimationFinished() => throwAnimationFinished = true;
}
