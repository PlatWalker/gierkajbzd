using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JG
{
    //now every object has this StateManager and inherits stats and components from this class

    public abstract class ObjectsStateManager : StateManager
    {
        [Header("References")]
        public Animator animator;
        public new Rigidbody rigidbody;

        [Header("Controller Values")]
        public float vertical;
        public float horizontal;
        public bool lockOn;
        public float delta;

        public override void Init()
        {
            animator = GetComponentInChildren<Animator>();
            rigidbody = GetComponentInChildren<Rigidbody>();
            animator.applyRootMotion = false;
        }

    }
}


