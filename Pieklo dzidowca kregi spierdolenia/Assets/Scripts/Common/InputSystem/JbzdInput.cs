using System.Collections.Generic;
using UnityEngine;

namespace jbzd.Common.InputSystem
{
    public abstract class JbzdInput
    {
        public delegate void ClickNotify();
        
        protected List<KeyActionPair> ButtonMappings;
        
        public virtual void UpdateInputs()
        {
            if (!Input.anyKeyDown) return;
            
            foreach (var buttonMapping in ButtonMappings)
            {
                if (Input.GetKeyDown(buttonMapping.KeyMapping)) buttonMapping.Action?.Invoke();
            }
        }

        public abstract void Start();

        protected struct KeyActionPair
        {
            public KeyCode KeyMapping;
            public ClickNotify Action;
        }
    }
}