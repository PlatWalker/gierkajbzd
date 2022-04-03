using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PimpekAnimationEvents : MonoBehaviour
{
    public bool startJumping { get; set; }
    
    private void AnimationCallForJump() => startJumping = true;
}
