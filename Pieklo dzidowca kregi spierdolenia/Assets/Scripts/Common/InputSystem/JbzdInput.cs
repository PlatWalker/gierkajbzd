using System;
using System.Collections.Generic;
using UnityEngine;

namespace jbzd.Common.InputSystem
{
    public abstract class JbzdInput
    {
        public delegate void ClickNotify();

        /// <summary>
        /// Property that holds pairs of action and button which should invoke such action.
        /// </summary>
        protected List<KeyActionPair> ButtonMappings { get; set; }
        
        /// <summary>
        /// Method that is called every frame. Update dynamic inputs such as position of mouse on screen.
        /// Moreover using <see cref="ButtonMappings"/> for invoking actions. 
        /// </summary>
        public virtual void UpdateInputs()
        {
            if (!Input.anyKeyDown) return;
            
            foreach (var buttonMapping in ButtonMappings)
            {
                if (Input.GetKeyDown(buttonMapping.KeyMapping)) buttonMapping.Action();
            }
        }

        public abstract void Start();

        protected struct KeyActionPair
        {
            public KeyCode KeyMapping;
            public Action Action;
        }
    }
}